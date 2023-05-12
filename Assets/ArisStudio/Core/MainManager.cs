using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.AsGameObject;
using ArisStudio.UI;
using ArisStudio.Utils;
using UnityEngine;
#if ENABLE_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.Networking;

namespace ArisStudio.Core
{
    /// <summary>
    /// Main logic that control the story flow.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Main Manager [Singleton]")]
    public class MainManager : Singleton<MainManager>
    {
        private AsCharacterManager asCharacterManager;
        private AsAudioManager asAudioManager;
        private AsImageManager asImageManager;
        private AsSceneManager asSceneManager;
        private AsDialogueManager asDialogueManager;
        private AsComponentsManager asComponentsManager;
        private AsSelectButtonManager asSelectButtonManager;
        SettingsMenuUI settingsMenu;

        private int asCommandListLength;
        private List<string> asCommandList = new List<string>();

        // List
        private readonly Dictionary<string, int> targetList = new Dictionary<string, int>(); // targetName, lineIndex (i.e, t1, 7)
        private readonly Dictionary<string, string> nameIdList = new Dictionary<string, string>(); // aliasName, assetType (i.e, hifumi, char)
        private readonly Dictionary<string, string> aliasList = new Dictionary<string, string>();

        // This UDictionary only for debugging purposes
        [UDictionary.Split(85, 15)]
        [Header("DEBUG")]
        [SerializeField] private TargetList m_TargetList;
        [Serializable] public class TargetList : UDictionary<string, int> { }

        [UDictionary.Split(80, 20)]
        [SerializeField] private NameIdList m_NameIdList;
        [Serializable] public class NameIdList : UDictionary<string, string> { }

        [UDictionary.Split(30, 70)]
        [SerializeField] private AliasList m_AliasList;
        [Serializable] public class AliasList : UDictionary<string, string> { }

        // Play state
        [SerializeField] KeyCode[] m_ProgressStoryKeys = { KeyCode.Space, KeyCode.Return, KeyCode.RightArrow };
        public bool IsPlaying { get; private set; }
        public bool IsSelectChoice { get; set; } // Does selecting a choice button?

        // Autoplay
        public bool IsAuto { get; set; }
        public float autoTime { get; private set; } = 2.3f;
        public float autoTimer { get; set; }

        // Wait state
        private bool isWait;
        private float waitTime;
        private float waitTimer;

        private int runLineIndex;

        #region Unity built-in methods

        private void Awake()
        {
            asCharacterManager = AsCharacterManager.Instance;
            asAudioManager = AsAudioManager.Instance;
            asImageManager = AsImageManager.Instance;
            asSceneManager = AsSceneManager.Instance;
            asDialogueManager = AsDialogueManager.Instance;
            asComponentsManager = AsComponentsManager.Instance;
            asSelectButtonManager = AsSelectButtonManager.Instance;
            settingsMenu = FindObjectOfType<SettingsMenuUI>();
        }

        private void Update()
        {
            ProgressStoryInputs();

            if (IsAuto)
            {
                autoTimer += Time.deltaTime;

                if (autoTimer >= autoTime || runLineIndex == 0)
                {
                    autoTimer = 0;
                    IsPlaying = true;
                }
            }

            if (IsSelectChoice)
            {
                IsPlaying = false;
                return;
            }

            if (isWait)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer < waitTime || waitTime <= -1) return; // -1 means unlimited

                waitTimer = 0;
                autoTimer = 0;
                isWait = false;
                IsPlaying = true;
            }

            if (IsPlaying && runLineIndex < asCommandListLength)
                RunAsCommand(asCommandList[runLineIndex]);

            if (runLineIndex == asCommandListLength)
            {
                IsPlaying = false;
                autoTimer = 0;
                waitTime = 0;
            }
        }

        #endregion // Unity built-in methods

        /// <summary>
        /// Initialize before starting the story.
        /// </summary>
        private void Initialize()
        {
            nameIdList.Clear();
            targetList.Clear();
            aliasList.Clear();

            m_TargetList.Clear();
            m_NameIdList.Clear();
            m_AliasList.Clear();

            asCharacterManager.AsCharacterInit();
            asAudioManager.AsAudioInit();
            asImageManager.AsImageInit();
            asSceneManager.AsSceneInit();
            asDialogueManager.AsDialogueInit();
            asComponentsManager.AsComponentsInit();
            asSelectButtonManager.AsSelectButtonInit();

            // Play State
            IsPlaying = false;
            IsSelectChoice = false;
            autoTimer = 0;
            isWait = false;
            waitTime = 0;
            waitTimer = 0;
            runLineIndex = 0;

            DebugConsole.Instance.PrintLog("\n<#ffa500>Initialize</color>");
        }

        /// <summary>
        /// Load a story.
        /// </summary>
        public void LoadStory()
        {
            StartCoroutine(LoadStory(SettingsManager.Instance.currentStoryFilePath));
        }

        /// <summary>
        /// Switch to another story from the same directory or a subdirectory of the currently running story.
        /// </summary>
        /// <param name="storyFileName"></param>
        private void SwitchStory(string storyFileName)
        {
            StartCoroutine(LoadStory(Path.Combine(Path.GetDirectoryName(SettingsManager.Instance.currentStoryFilePath), storyFileName)));
        }

        /// <summary>
        /// Load a story file.
        /// </summary>
        /// <param name="storyPath"></param>
        /// <returns></returns>
        private IEnumerator LoadStory(string storyPath)
        {
            UnityWebRequest www = UnityWebRequest.Get(storyPath);
            yield return www.SendWebRequest();

            asCommandList = www.downloadHandler.text.Split('\n').ToList();
            asCommandList.RemoveAll(string.IsNullOrEmpty); // remove item that's empty
            asCommandList = asCommandList.Where(item => !item.StartsWith("//")).ToList(); /// remove item that start with '//'

            asCommandListLength = asCommandList.Count;
            PreLoad(asCommandList.ToArray());

            DebugConsole.Instance.PrintLog($"Load Story: <#00ff00>{storyPath}</color>");
        }

        public void ProgressStory()
        {
            if (!IsPlaying) IsPlaying = true;
        }

        /// <summary>
        /// Progress the story using the defined inputs.
        /// </summary>
        private void ProgressStoryInputs()
        {
            if (!IsPlaying)
            {
#if ENABLE_NEW_INPUT_SYSTEM
                foreach (KeyCode key in m_ProgressStoryKeys)
                    if (Keyboard.current.GetKeyDown(key)) IsPlaying = true;
#else
                foreach (KeyCode key in m_ProgressStoryKeys)
                    if (Input.GetKeyDown(key)) IsPlaying = true;
#endif
            }
        }

        /// <summary>
        /// Switch Auto state, activate/deactivate.
        /// </summary>
        /// <param name="state"></param>
        public void AutoState(bool state)
        {
            autoTimer = 0; // reset the timer that maybe used by another operation.
            IsAuto = state;

            // Change the visual according to the AutoState
            if (settingsMenu == null || settingsMenu.m_AutoActivate == null || settingsMenu.m_AutoDeactivate == null) return;
            settingsMenu.m_AutoActivate.gameObject.SetActive(!state);
            settingsMenu.m_AutoDeactivate.gameObject.SetActive(state);
        }

        /// <summary>
        /// Run a command from asCommandList.
        /// </summary>
        /// <param name="asCommand"></param>
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

        /// <summary>
        /// Run corresponding functions for each command that start with "load".
        /// </summary>
        /// <param name="loadCommand"></param>
        public void PreLoad(string loadCommand)
        {
            /*
            * Examples:
            * load spr hifumi hifumi_spr
            * load bg Classroom Classroom.jpg
            * load bgm MainTheme theme.ogg
            */

            string[] asCommand = AsCommand.Parse(loadCommand);

            // spr, bg, bgm...
            switch (asCommand[1])
            {
                // Character
                case "spr": // default spr
                    asCharacterManager.LoadAsCharacter(asCommand, false);
                    nameIdList.Add(asCommand[2], "char");
                    m_NameIdList.Add(asCommand[2], "char");
                    break;
                case "sprC": // Legacy
                case "sprc": // communicate spr
                case "spr_c":
                    asCharacterManager.LoadAsCharacter(asCommand, true);
                    nameIdList.Add(asCommand[2], "char");
                    m_NameIdList.Add(asCommand[2], "char");
                    break;

                // Image
                case "bg":
                case "mg":
                case "fg":
                    asImageManager.LoadAsImage(asCommand, asCommand[1]);
                    nameIdList.Add(asCommand[2], "image");
                    m_NameIdList.Add(asCommand[2], "image");
                    break;

                // Audio
                case "bgm":
                case "sfx":
                    asAudioManager.LoadAsAudio(asCommand, asCommand[1]);
                    nameIdList.Add(asCommand[2], "audio");
                    m_NameIdList.Add(asCommand[2], "audio");
                    break;
            }
        }

        /// <summary>
        /// Fire up initialization and preload.
        /// </summary>
        /// <param name="asCommands"></param>
        private void PreLoad(string[] asCommands)
        {
            Initialize();

            // For every command...
            for (int lineIndex = 0; lineIndex < asCommands.Length; lineIndex++)
            {
                string asCommand = asCommands[lineIndex];
                string[] command = AsCommand.Parse(asCommand);

                switch (command[0])
                {
                    case "load":
                        PreLoad(asCommand);
                        break;
                    case "target":
                        targetList.Add(command[1], lineIndex); // key: target name, value: line index
                        m_TargetList.Add(command[1], lineIndex); // debug list
                        break;
                    case "alias":
                        aliasList.Add(command[1], command[2]); // slisd name, alias content
                        m_AliasList.Add(command[1], command[2]); // Debug list
                        break;
                }
            }
        }
        /// <summary>
        /// Jump to specific a target.
        /// </summary>
        /// <param name="targetName"></param>
        public void JumpTarget(string targetName)
        {
            runLineIndex = targetList[targetName];
            DebugConsole.Instance.PrintLog($"Jump to target: <#00ff00>{targetName}</color>");
            autoTimer = 0;
            IsPlaying = true;
        }

        /// <summary>
        /// Show all defined targets. Debugging only.
        /// </summary>
        private void ShowAllTargets()
        {
            foreach (KeyValuePair<string, int> target in targetList)
                DebugConsole.Instance.PrintLog($"Target: <#00ff00>{target.Key}</color> at line <#00ff00>{target.Value}</color>");
        }

        /// <summary>
        /// Resolve a command and do their corresponding function.
        /// </summary>
        /// <param name="textCommand"></param>
        public void SolveCommand(string textCommand)
        {
            if (textCommand.Trim() == string.Empty || textCommand.StartsWith("//") || textCommand.StartsWith("load")) return;

            if (textCommand.StartsWith("="))
            {
                IsPlaying = false;
                return;
            }

#if UNITY_EDITOR
            Debug.Log($"Command: {textCommand}");
#endif

            string[] command = AsCommand.Parse(textCommand);

            if (nameIdList.ContainsKey(command[0]))
                textCommand = $"{nameIdList[command[0]]} {textCommand}";

            foreach (KeyValuePair<string, string> alias in aliasList.Where(alias => textCommand.Contains(alias.Key)))
            {
                // if current command contain alias name, replace the command with the alias value.
                textCommand = textCommand.Replace(alias.Key, alias.Value);
            }

            command = AsCommand.Parse(textCommand);

            switch (command[0])
            {
                // special commands
                case "wait":
                    isWait = true;
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
                case "select":
                    asSelectButtonManager.SetButton(command);
                    IsSelectChoice = true;
                    break;

                // dialogue commands
                case "txt": // Legacy
                case "t":
                case "mt":
                case "middle_text":
                case "bt":
                case "bottom_text":
                    asDialogueManager.AsDialogueCommand(command);
                    IsPlaying = false;
                    break;

                case "text":
                case "tc":
                case "mtc":
                case "btc":
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
                case "screen": // Legacy
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
