using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArisStudio.Image;
using ArisStudio.Sound;
using ArisStudio.Spr;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ArisStudio
{
    public class MainControl : MonoBehaviour
    {
        public DebugConsole debugConsole;

        [Header("InputField")] public InputField loadTxtInputField;
        public InputField setWebPathInputField;

        [Header("TextArea")] public TextArea textArea;

        [Header("SelectButton")] public SelectButton selectButton;

        private SprFactory sprFactory;
        private ImageFactory imageFactory;
        private SoundFactory soundFactory;

        private bool isPlaying, isTyping, isSelecting;

        private bool isAuto;
        private float autoTimer;
        private float autoSeconds = 2.3f;

        private bool isWaiting;
        private float waitTimer;
        private float waitSeconds;

        private string textDataPath;
        private string[] textsData;
        private int textsLength;
        private int runLineNumber;

        // List
        Dictionary<string, int> targetList = new Dictionary<string, int>();

        private void Start()
        {
            sprFactory = GetComponent<SprFactory>();
            imageFactory = GetComponent<ImageFactory>();
            soundFactory = GetComponent<SoundFactory>();

            SetLocalDataPath();
        }

        private void Update()
        {
            if (isWaiting)
            {
                autoTimer = 0;
                isPlaying = false;

                waitTimer += Time.deltaTime;
                if (waitTimer < waitSeconds) return;

                isWaiting = false;
                waitTimer = 0;
                isPlaying = true;
                return;
            }

            if (isTyping)
            {
                autoTimer = 0;
                isPlaying = false;
                return;
            }

            if (isSelecting)
            {
                autoTimer = 0;
                isPlaying = false;
                return;
            }

            if (isAuto)
            {
                autoTimer += Time.deltaTime;
                if (autoTimer > autoSeconds)
                {
                    autoTimer = 0;
                    isPlaying = true;
                }
            }

            if (!isPlaying) return;

            if (runLineNumber < textsLength)
            {
                RunText(textsData[runLineNumber].Trim());
            }
        }

        #region SetPath

        public void SetLocalDataPath()
        {
            var rootPath = Directory.GetParent(Application.dataPath).ToString();
            var localDataPath = Path.Combine(rootPath, "Data");
            sprFactory.SetSprDataPath(localDataPath);
            imageFactory.SetImageDataPath(localDataPath);
            soundFactory.SetSoundDataPath(localDataPath);
            textDataPath = Path.Combine(rootPath, "0Txt");

            debugConsole.PrintLog("Set Local Data Path");
        }

        public void SetWebPath()
        {
            var url = setWebPathInputField.text;
            sprFactory.SetSprDataPath(url);
            imageFactory.SetImageDataPath(url);
            soundFactory.SetSoundDataPath(url);

            debugConsole.PrintLog($"Set Web Path: <color=lime>{url}</color>");
        }

        #endregion

        # region Set PlayState

        public void SetAuto()
        {
            isAuto = true;
            debugConsole.PrintLog("Auto");
        }

        public void SetPlay()
        {
            if (isTyping)
            {
                textArea.PlayAllText();
                return;
            }

            isPlaying = true;
            debugConsole.PrintLog("Play Once");
        }

        public void SetTyping(bool b)
        {
            isTyping = b;
        }

        public void SetSelect(string tName)
        {
            runLineNumber = targetList[tName];
            isSelecting = false;
            debugConsole.PrintLog($"Select: <color=lime>{tName}</color>");
        }

        # endregion

        // ReSharper disable Unity.PerformanceAnalysis
        public void RunText(string text)
        {
            try
            {
                ParseTextIntoCommand(text);
            }
            catch (Exception e)
            {
                debugConsole.PrintLog(e.Message);
                Debug.LogException(e);
            }
            finally
            {
                runLineNumber++;
            }
        }

        public void LoadTextData()
        {
            StartCoroutine(SetTextData(Path.Combine(textDataPath, loadTxtInputField.text + ".txt")));
        }

        private IEnumerator SetTextData(string textPath)
        {
            var www = UnityWebRequest.Get(textPath);
            yield return www.SendWebRequest();
            textsData = www.downloadHandler.text.Split('\n');

            textsLength = textsData.Length;
            PreLoad(textsData);

            debugConsole.PrintLog($"Load Text Data: <color=lime>{textPath}</color>");
        }

        private void Initialize()
        {
            textArea.gameObject.SetActive(false);
            sprFactory.Initialize();
            imageFactory.Initialize();
            soundFactory.Initialize();
            targetList.Clear();
            debugConsole.PrintLog("<color=orange>Initialize</color>");
        }

        private void PreLoad(string[] texts)
        {
            Initialize();

            for (var lineIndex = 0; lineIndex < texts.Length; lineIndex++)
            {
                var text = texts[lineIndex].Trim();
                if (text.StartsWith("target"))
                {
                    targetList.Add(text.Replace("target ", ""), lineIndex);
                }
                else if (text.StartsWith("load"))
                {
                    var textSplit = text.Split(' ');
                    switch (textSplit[1])
                    {
                        case "spr":
                        {
                            sprFactory.CreateSprGameObjectWithDef(textSplit[2], textSplit[3]);
                            break;
                        }
                        case "sprC":
                        {
                            sprFactory.CreateSprGameObjectWithComm(textSplit[2], textSplit[3]);
                            break;
                        }
                        case "bg":
                        {
                            imageFactory.LoadBackground(textSplit[2], textSplit[3]);
                            break;
                        }
                        case "cover":
                        {
                            imageFactory.LoadCover(textSplit[2], textSplit[3]);
                            break;
                        }
                        case "bgm":
                        {
                            soundFactory.LoadBgm(textSplit[2], textSplit[3]);
                            break;
                        }
                        case "se":
                        {
                            soundFactory.LoadSoundEffect(textSplit[2], textSplit[3]);
                            break;
                        }
                        case "end":
                        {
                            runLineNumber = lineIndex;
                            isPlaying = false;
                            break;
                        }
                    }
                }
            }

            foreach (var t in targetList)
            {
                debugConsole.PrintLog($"Target: <color=lime>{t.Key}</color> Line: <color=lime>{t.Value}</color>");
            }
        }

        private void ParseTextIntoCommand(string text)
        {
            if (text.StartsWith("="))
            {
                isPlaying = false;
            }
            else if (text != string.Empty || text.StartsWith("//"))
            {
                var l = text.Split(' ');
                string[] tt;
                switch (l[0])
                {
                    case "wait":
                        waitSeconds = float.Parse(l[1]);
                        isWaiting = true;
                        debugConsole.PrintLog($"Wait: <color=lime>{waitSeconds} s</color>");
                        break;

                    case "auto":
                        autoSeconds = float.Parse(l[1]);
                        debugConsole.PrintLog($"Auto Seconds: <color=lime>{autoSeconds} s</color>");
                        break;

                    case "jump":
                        runLineNumber = targetList[l[1]];
                        debugConsole.PrintLog($"Jump: <color=lime>{l[1]}</color>");
                        break;


                    // Text
                    case "txt":
                        isPlaying = false;
                        tt = text.Split('\'');
                        textArea.SetText(tt[1], tt[3], tt[5]);
                        break;

                    case "t":
                        isPlaying = false;
                        tt = text.Split('\'');
                        textArea.SetText(tt[1], tt[3], tt[5]);
                        break;

                    case "text":
                        textArea.TextCommand(text);
                        break;


                    case "button":
                        isPlaying = false;
                        isSelecting = true;
                        selectButton.SelectCommand(text);
                        break;


                    // Spr
                    case "spr":
                        sprFactory.SprCommand(text);
                        break;

                    // Image
                    case "bg":
                        imageFactory.ImageCommand(text);
                        break;
                    case "cover":
                        imageFactory.ImageCommand(text);
                        break;

                    // Sound
                    case "bgm":
                        soundFactory.SoundCommand(text);
                        break;
                    case "se":
                        soundFactory.SoundCommand(text);
                        break;
                }
            }
        }
    }
}