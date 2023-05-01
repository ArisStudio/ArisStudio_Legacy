using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.AsGameObject;
using ArisStudio.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Core
{
    public class MainManager : Singleton<MainManager>
    {
        private AsCharacterManager asCharacterManager;
        private AsAudioManager asAudioManager;
        private AsImageManager asImageManager;
        private AsSceneManager asSceneManager;
        private AsDialogueManager asDialogueManager;
        private AsComponentsManager asComponentsManager;
        private AsSelectButtonManager asSelectButtonManager;

        private int asCommandListStrength;
        private List<string> asCommandList = new List<string>();

        // List
        private readonly Dictionary<string, int> targetList = new Dictionary<string, int>();
        private readonly Dictionary<string, string> nameIdList = new Dictionary<string, string>();

        public bool IsPlaying { private get; set; }

        // Auto Play
        public bool IsAuto { private get; set; }
        private float autoTimer;
        private float autoTime = 2.3f;

        private bool isWait;
        private float waitTimer;
        private float waitTime;

        private int runLineIndex;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !IsPlaying) IsPlaying = true;

            if (isWait)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    waitTimer = 0;
                    isWait = false;
                    IsPlaying = true;
                }
            }

            if (IsAuto)
            {
                autoTimer += Time.deltaTime;
                if (autoTimer >= autoTime)
                {
                    autoTimer = 0;
                    IsPlaying = true;
                }
            }

            if (IsPlaying && runLineIndex < asCommandListStrength) RunAsCommand(asCommandList[runLineIndex]);
        }

        private void Start()
        {
            asCharacterManager = AsCharacterManager.Instance;
            asAudioManager = AsAudioManager.Instance;
            asImageManager = AsImageManager.Instance;
            asSceneManager = AsSceneManager.Instance;
            asDialogueManager = AsDialogueManager.Instance;
            asComponentsManager = AsComponentsManager.Instance;
            asSelectButtonManager = AsSelectButtonManager.Instance;
        }

        private void Initialize()
        {
            nameIdList.Clear();
            targetList.Clear();

            asCharacterManager.AsCharacterInit();
            asAudioManager.AsAudioInit();
            asImageManager.AsImageInit();
            asSceneManager.AsSceneInit();
            asDialogueManager.AsDialogueInit();
            asComponentsManager.AsComponentsInit();
            asSelectButtonManager.AsSelectButtonInit();

            // Play State
            IsPlaying = false;
            runLineIndex = 0;

            DebugConsole.Instance.PrintLog("\n<#ffa500>Initialize</color>");
        }

        public void LoadStory()
        {
            // StartCoroutine(SetTextData(Path.Combine(textDataPath, $"{loadTxtInputField.text}.txt")));
            StartCoroutine(LoadStory(SettingsManager.Instance.currentStoryFilePath));
        }

        public void SwitchStory(string storyFileName)
        {
            StartCoroutine(LoadStory(Path.Combine(SettingsManager.Instance.currentStoryFilePath, $"{storyFileName}.txt")));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadStory(string storyPath)
        {
            UnityWebRequest www = UnityWebRequest.Get(storyPath);
            yield return www.SendWebRequest();
            // textsData = www.downloadHandler.text.Split('\n');
            asCommandList = www.downloadHandler.text.Split('\n').ToList();
            asCommandList.RemoveAll(string.IsNullOrEmpty);

            // textsLength = textsData.Length;
            asCommandListStrength = asCommandList.Count;
            // PreLoad(textsData);
            PreLoad(asCommandList.ToArray());

            DebugConsole.Instance.PrintLog($"Load Story Data: <#00ff00>{storyPath}</color>");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void RunAsCommand(string asCommand)
        {
            try
            {
                SolveCommand(asCommand);
            }
            catch (Exception e)
            {
                DebugConsole.Instance.PrintLog(e.Message);
#if UNITY_EDITOR
                Debug.LogException(e);
#endif
            }
            finally
            {
                runLineIndex++;
            }
        }

        public void PreLoad(string text)
        {
            var asCommand = AsCommand.Parse(text);

            switch (asCommand[1])
            {
                case "spr":
                    asCharacterManager.LoadAsCharacter(asCommand, false);
                    nameIdList.Add(asCommand[2], "char");
                    break;
                case "sprC": // Legacy
                case "sprc":
                case "spr_c":
                    asCharacterManager.LoadAsCharacter(asCommand, true);
                    nameIdList.Add(asCommand[2], "char");
                    break;

                case "bg":
                case "mg":
                case "fg":
                    asImageManager.LoadAsImage(asCommand, asCommand[1]);
                    nameIdList.Add(asCommand[2], "image");
                    break;

                case "bgm":
                case "sfx":
                    asAudioManager.LoadAsAudio(asCommand, asCommand[1]);
                    nameIdList.Add(asCommand[2], "audio");
                    break;
            }
        }

        private void PreLoad(string[] texts)
        {
            Initialize();

            for (int lineIndex = 0; lineIndex < texts.Length; lineIndex++)
            {
                string text = texts[lineIndex];

                if (text.StartsWith("target")) targetList.Add(text.Replace("target", "").Trim(), lineIndex);

                else if (text.StartsWith("load")) PreLoad(text);
            }
        }

        public void JumpTarget(string targetName)
        {
            runLineIndex = targetList[targetName];
            DebugConsole.Instance.PrintLog($"Jump to target: <#00ff00>{targetName}</color>");
            autoTimer = 0;
            IsPlaying = true;
        }

        private void ShowAllTargets()
        {
            foreach (KeyValuePair<string, int> target in targetList)
                DebugConsole.Instance.PrintLog($"Target: <#00ff00>{target.Key}</color> at line <#00ff00>{target.Value}</color>");
        }

        public void SolveCommand(string text)
        {
            if (text.Trim() == string.Empty || text.StartsWith("//") || text.StartsWith("load")) return;

            if (text.StartsWith("="))
            {
                IsPlaying = false;
                return;
            }

#if UNITY_EDITOR
            Debug.Log($"Command: {text}");
#endif
            var command = AsCommand.Parse(text);

            if (nameIdList.ContainsKey(command[0])) text = $"{nameIdList[command[0]]} {text}";

            command = AsCommand.Parse(text);

            switch (command[0])
            {
                // special commands
                case "wait":
                    waitTime = float.Parse(command[1]);
                    IsPlaying = false;
                    break;
                case "targets":
                    ShowAllTargets();
                    break;
                case "jump":
                    JumpTarget(command[1]);
                    break;
                case "auto":
                    autoTime = float.Parse(command[1]);
                    break;
                case "switch":
                    SwitchStory(command[1]);
                    break;

                // select commands
                case "btn":
                case "button":
                case "select":
                    asSelectButtonManager.SetButton(command);
                    IsPlaying = false;
                    break;

                // dialogue commands
                case "text":
                case "t":
                case "txt":
                case "mt":
                case "middle_text":
                case "bt":
                case "bottom_text":
                    asDialogueManager.AsDialogueCommand(command);
                    IsPlaying = false;
                    break;

                case "tc":
                    asDialogueManager.AsDialogueCommand(command);
                    break;

                case "th":
                    asCharacterManager.TextWithHighlight(command[1]);
                    asDialogueManager.AsDialogueCommand(command);
                    IsPlaying = false;
                    break;

                case "thc":
                    asCharacterManager.TextWithHighlight(command[1]);
                    asDialogueManager.AsDialogueCommand(command);
                    break;

                // components commands
                case "label":
                    asComponentsManager.SetLabel(command[1]);
                    break;

                case "banner":
                    asComponentsManager.BannerCommand(command);
                    IsPlaying = command[1] == "hide";
                    break;

                // scene commands
                case "scene":
                    asSceneManager.AsSceneCommand(command);
                    break;

                // image commands
                case "bg":
                case "mg":
                case "fg":
                case "image":
                    asImageManager.AsImageCommand(command);
                    break;

                // audio commands
                case "bgm":
                case "sfx":
                case "audio":
                    asAudioManager.AsAudioCommand(command);
                    break;

                // character commands
                case "spr":
                case "char":
                    asCharacterManager.AsCharacterCommand(command);
                    break;
            }
        }
    }
}
