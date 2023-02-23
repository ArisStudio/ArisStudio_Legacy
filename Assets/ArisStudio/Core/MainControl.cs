using System;
using System.Collections;
using System.Collections.Generic;
using ArisStudio.ScreenEffect;
using ArisStudio.Sound;
using ArisStudio.Spr;
using ArisStudio.UI;
using ArisStudio.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Core
{
    /// <summary>
    /// The core class that behave like a bridge to connect with other class functionality.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Main Control")]
    public class MainControl : MonoBehaviourSingleton<MainControl>
    {
        [Header("User Interface")]
        [SerializeField] public DebugConsole m_DebugConsole;
        [SerializeField] public DialogueManager m_DialogueManager;
        [SerializeField] public SettingsManager m_SettingsManager;
        [Space]
        [SerializeField] protected TextArea textArea;
        [Space]
        [SerializeField] protected SelectButton selectButton;
        [Space]
        [SerializeField] protected Label label;
        [Space]
        [SerializeField] protected Banner banner;


        // [SerializeField] protected InputField loadTxtInputField;
        // [SerializeField] protected InputField setWebPathInputField;
        // [Space]

        private SprFactory sprFactory;
        private ImageFactory imageFactory;
        private SoundFactory soundFactory;
        private ScreenEffectFactory screenEffectFactory;
        private End end;

        private bool isPlaying, isTyping, isSelecting, isBanner;

        private bool isAuto;
        private float autoTimer;
        private float autoSeconds = 2.3f;

        private bool isWaiting;
        private float waitTimer;
        private float waitSeconds;

        // private string textDataPath;
        private string[] textsData;
        private int textsLength;
        private int runLineNumber;

        // List
        Dictionary<string, int> targetList = new Dictionary<string, int>();

        new void Awake()
        {
            // this.Persistent = true;
            base.Awake();

            // m_DebugConsole = DebugConsole.Instance;
            // m_SettingsManager = SettingsManager.Instance;

            sprFactory = FindObjectOfType<SprFactory>();
            imageFactory = FindObjectOfType<ImageFactory>();
            soundFactory = FindObjectOfType<SoundFactory>();
            screenEffectFactory = FindObjectOfType<ScreenEffectFactory>();

            end = FindObjectOfType<End>();
        }

        void Update()
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

            if (isBanner)
            {
                banner.CloseBanner();
                isBanner = false;
                return;
            }

            if (runLineNumber < textsLength)
            {
                RunText(textsData[runLineNumber].Trim());
            }
        }

        void NewInitialize()
        {
            m_DialogueManager.Initialize();
        }

        public void PlayStory()
        {
            NewInitialize();
        }

        #region SetPath
        // public void SetLocalDataPath()
        // {
// #if UNITY_ANDROID
//             string rootPath = $"file:///{Application.persistentDataPath}";
// #elif UNITY_STANDALONE_OSX
//             string rootPath = Directory.GetParent($"file://{Application.dataPath}").ToString();
// #else
//             string rootPath = Directory.GetParent(Application.dataPath).ToString();
// #endif
            // string localDataPath = Path.Combine(rootPath, "Data");
            // sprFactory.SetSprDataPath(m_SettingsManager.currentLocalDataPath);
            // imageFactory.SetImageDataPath(m_SettingsManager.currentLocalDataPath);
            // soundFactory.SetSoundDataPath(m_SettingsManager.currentLocalDataPath);
            // textDataPath = Path.Combine(rootPath, "0Txt");

            // m_DebugConsole.PrintLog("Set Local Data Path");
        // }

        // public void SetWebPath()
        // {
        //     string url = setWebPathInputField.text;
        //     sprFactory.SetSprDataPath(url);
        //     imageFactory.SetImageDataPath(url);
        //     soundFactory.SetSoundDataPath(url);

        //     m_DebugConsole.PrintLog($"Set Web Path: <#00ff00>{url}</color>");
        // }

        #endregion

        # region Set PlayState

        public void SetAuto(bool b)
        {
            isAuto = b;
            m_DebugConsole.PrintLog($"Auto {(isAuto ? "On" : "Off")}");
        }

        public void SetPlay()
        {
            if (isTyping)
            {
                textArea.PlayAllText();
                return;
            }

            isPlaying = true;
            m_DebugConsole.PrintLog("Play Once");
        }

        public void SetTyping(bool b)
        {
            isTyping = b;
        }

        public void SetSelect(string tName)
        {
            runLineNumber = targetList[tName];
            isSelecting = false;
            m_DebugConsole.PrintLog($"Select choice: <#00ff00>{tName}</color>");
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
                m_DebugConsole.PrintLog(e.Message);
                Debug.LogException(e);
            }
            finally
            {
                runLineNumber++;
            }
        }

        public void LoadTextData()
        {
            // StartCoroutine(SetTextData(Path.Combine(textDataPath, $"{loadTxtInputField.text}.txt")));
            StartCoroutine(SetTextData(m_SettingsManager.currentStoryFilePath));
        }

        private void LoadTextData(string txtName)
        {
            // StartCoroutine(SetTextData(Path.Combine(textDataPath, $"{txtName}.txt")));
            StartCoroutine(SetTextData(m_SettingsManager.currentStoryFilePath));
        }

        private IEnumerator SetTextData(string textPath)
        {
            UnityWebRequest www = UnityWebRequest.Get(textPath);
            yield return www.SendWebRequest();
            textsData = www.downloadHandler.text.Split('\n');

            textsLength = textsData.Length;
            PreLoad(textsData);

            m_DebugConsole.PrintLog($"Load Story Data: <#00ff00>{textPath}</color>");
        }

        private void Initialize()
        {
            textArea.gameObject.SetActive(false);
            sprFactory.Initialize();
            imageFactory.Initialize();
            soundFactory.Initialize();
            screenEffectFactory.Initialize();
            end.Clear();
            selectButton.Initialize();

            targetList.Clear();

            label.gameObject.SetActive(false);
            banner.CloseBanner();

            autoTimer = 0;
            runLineNumber = 0;

            m_DebugConsole.PrintLog("\n<#ffa500>Initialize</color>");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void PreLoad(string text)
        {
            try
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
                    case "custom":
                    {
                        var customImgList = text.Split('[')[1].Split(']')[0].Split(',');
                        sprFactory.CreateCustomGameObjectWithDef(textSplit[2], float.Parse(textSplit[3]), textSplit[4], textSplit[5],
                            customImgList);
                        break;
                    }
                    case "customC":
                    {
                        var customImgList = text.Split('[')[1].Split(']')[0].Split(',');
                        sprFactory.CreateCustomGameObjectWithComm(textSplit[2], float.Parse(textSplit[3]), textSplit[4], textSplit[5],
                            customImgList);
                        break;
                    }
                    case "char":
                    {
                        var charImgList = text.Split('[')[1].Split(']')[0].Split(',');
                        sprFactory.CreateCharGameObjectWithDef(textSplit[2], float.Parse(textSplit[3]), textSplit[4], charImgList);
                        break;
                    }
                    case "charC":
                    {
                        var charImgList = text.Split('[')[1].Split(']')[0].Split(',');
                        sprFactory.CreateCharGameObjectWithComm(textSplit[2], float.Parse(textSplit[3]), textSplit[4], charImgList);
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
                }
            }
            catch (Exception e)
            {
                m_DebugConsole.PrintLog(e.Message);
                Debug.LogException(e);
            }
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
                else if (text.StartsWith("load end"))
                {
                    runLineNumber = lineIndex;
                    isPlaying = false;
                    m_DebugConsole.PrintLog($"PreLoad End at {runLineNumber}");
                }
                else if (text.StartsWith("load"))
                {
                    PreLoad(text);
                }
            }

            foreach (var t in targetList)
            {
                m_DebugConsole.PrintLog($"Target: <#00ff00>{t.Key}</color> Line: <#00ff00>{t.Value}</color>");
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
                    // End
                    case "end":
                        end.EndCommand(text);
                        isPlaying = false;
                        break;

                    case "ChangeTxt":
                        isPlaying = false;
                        autoTimer = 0;
                        LoadTextData(l[1]);
                        break;

                    case "wait":
                        waitSeconds = float.Parse(l[1]);
                        isWaiting = true;
                        m_DebugConsole.PrintLog($"Wait: <#00ff00>{waitSeconds} s</color>");
                        break;

                    case "auto":
                        autoSeconds = float.Parse(l[1]);
                        m_DebugConsole.PrintLog($"Auto Seconds: <#00ff00>{autoSeconds} s</color>");
                        break;

                    case "jump":
                        runLineNumber = targetList[l[1]];
                        m_DebugConsole.PrintLog($"Jump: <#00ff00>{l[1]}</color>");
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

                    case "tc":
                        tt = text.Split('\'');
                        textArea.SetText(tt[1], tt[3], tt[5]);
                        break;

                    case "th":
                        sprFactory.TextWithHl(l[1]);
                        isPlaying = false;
                        tt = text.Split('\'');
                        textArea.SetText(tt[1], tt[3], tt[5]);
                        break;

                    case "text":
                        textArea.TextCommand(text);
                        break;

                    // Button
                    case "button":
                        isPlaying = false;
                        isSelecting = true;
                        selectButton.SelectCommand(text);
                        break;
                    case "buttonS":
                        isPlaying = false;
                        isSelecting = true;
                        selectButton.SelectCommandWithSelect(int.Parse(l[1]), text);
                        break;

                    // Label
                    case "label":
                        label.SetLabelText(l[1]);
                        break;
                    // Banner
                    case "banner":
                        banner.SetBanner1Text(text);
                        isPlaying = false;
                        isBanner = true;
                        break;
                    case "banner2":
                        banner.SetBanner2Text(text);
                        isPlaying = false;
                        isBanner = true;
                        break;

                    // ScreenEffect
                    case "screen":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;
                    case "curtain":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;
                    case "speedline":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;
                    case "smoke":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;
                    case "dust":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;
                    case "snow":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;
                    case "rain":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;

                    // Spr
                    case "spr":
                        sprFactory.SprCommand(text);
                        break;
                    case "s":
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

                    default:
                        sprFactory.SprCommand($"s {text}");
                        break;
                }
            }
        }
    }
}
