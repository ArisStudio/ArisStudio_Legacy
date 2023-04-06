using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArisStudio.Image;
using ArisStudio.ScreenEffect;
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

        [Header("Label")] public Label label;

        [Header("Banner")] public Banner banner;

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
            screenEffectFactory = GetComponent<ScreenEffectFactory>();

            end = GetComponent<End>();

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

        #region SetPath

        public void SetLocalDataPath()
        {
#if UNITY_ANDROID
            var rootPath = $"file:///{Application.persistentDataPath}";
#elif UNITY_STANDALONE_OSX
            var rootPath = Directory.GetParent($"file://{Application.dataPath}").ToString();
#else
            var rootPath = Directory.GetParent(Application.dataPath).ToString();
#endif
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

        public void SetAuto(bool b)
        {
            isAuto = b;
            debugConsole.PrintLog($"Auto {(isAuto ? "On" : "Off")}");
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
            StartCoroutine(SetTextData(Path.Combine(textDataPath, $"{loadTxtInputField.text}.txt")));
        }

        private void LoadTextData(string txtName)
        {
            StartCoroutine(SetTextData(Path.Combine(textDataPath, $"{txtName}.txt")));
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
            screenEffectFactory.Initialize();
            end.Clear();
            selectButton.Initialize();

            targetList.Clear();

            label.gameObject.SetActive(false);
            banner.CloseBanner();

            autoTimer = 0;
            runLineNumber = 0;

            debugConsole.PrintLog("\n<color=orange>Initialize</color>");
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
                debugConsole.PrintLog(e.Message);
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
                    debugConsole.PrintLog($"PreLoad End at {runLineNumber}");
                }
                else if (text.StartsWith("load"))
                {
                    PreLoad(text);
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