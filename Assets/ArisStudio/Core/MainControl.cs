using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArisStudio.AsGameObject.Audio;
using ArisStudio.AsGameObject.Character;
using ArisStudio.ScreenEffect;
using ArisStudio.Sound;
using ArisStudio.Spr;
using ArisStudio.UI;
using ArisStudio.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Core
{
    /// <summary>
    /// The core class that behave like a bridge to connect with other class functionality.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Main Control")]
    public class MainControl : Singleton<MainControl>
    {
        [Header("User Interface")] [SerializeField]
        TextArea textArea;

        [Space] [SerializeField] SelectButton selectButton;

        [Space] [SerializeField] Label label;

        [Space] [SerializeField] Banner banner;

        private AsCharacterManager asCharacterManager;
        private AsAudioManager asAudioManager;


        SprFactory sprFactory;
        ImageFactory imageFactory;
        SoundFactory soundFactory;
        ScreenEffectFactory screenEffectFactory;
        End end;

        bool isPlaying,
            isTyping,
            isSelecting,
            isBanner;

        bool isAuto;
        float autoTimer;
        float autoSeconds = 2.3f;

        bool isWaiting;
        float waitTimer;
        float waitSeconds;

        // string textDataPath;
        string[] textsData;
        List<string> commands = new List<string>();
        int textsLength;
        int runLineNumber;

        // List
        private readonly Dictionary<string, int> targetList = new Dictionary<string, int>();
        Dictionary<string, string> nameIdList = new Dictionary<string, string>();

        void Awake()
        {
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
                if (waitTimer < waitSeconds)
                    return;

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

            if (!isPlaying)
                return;

            if (isBanner)
            {
                banner.CloseBanner();
                isBanner = false;
                return;
            }

            if (runLineNumber < textsLength)
                RunText(commands[runLineNumber]);
            // RunText(textsData[runLineNumber].Trim());
        }

        # region Set PlayState

        public void SetAuto(bool b)
        {
            isAuto = b;
            DebugConsole.Instance.PrintLog($"Auto {(isAuto ? "On" : "Off")}");
        }

        public void SetPlay()
        {
            if (isTyping)
            {
                textArea.PlayAllText();
                return;
            }

            isPlaying = true;
            DebugConsole.Instance.PrintLog("Play Once");
        }

        public void SetTyping(bool b)
        {
            isTyping = b;
        }

        public void SetSelect(string tName)
        {
            runLineNumber = targetList[tName];
            isSelecting = false;
            SetPlay();
            DebugConsole.Instance.PrintLog($"Select choice: <#00ff00>{tName}</color>");
        }

        # endregion

        // ReSharper disable Unity.PerformanceAnalysis
        public void RunText(string text)
        {
            try
            {
                ComandManager(text);
            }
            catch (Exception e)
            {
                DebugConsole.Instance.PrintLog(e.Message);
                Debug.LogException(e);
            }
            finally
            {
                runLineNumber++;
            }
        }

        /// <summary>
        /// Start a story.
        /// </summary>
        public void RunStory()
        {
            StartCoroutine(SetTextData(SettingsManager.Instance.currentStoryFilePath));
        }

        public void LoadTextData()
        {
            // StartCoroutine(SetTextData(Path.Combine(textDataPath, $"{loadTxtInputField.text}.txt")));
            StartCoroutine(SetTextData(SettingsManager.Instance.currentStoryFilePath));
        }

        void LoadTextData(string txtName)
        {
            // StartCoroutine(SetTextData(Path.Combine(textDataPath, $"{txtName}.txt")));
            StartCoroutine(SetTextData(SettingsManager.Instance.currentStoryFilePath));
        }

        IEnumerator SetTextData(string textPath)
        {
            UnityWebRequest www = UnityWebRequest.Get(textPath);
            yield return www.SendWebRequest();
            // textsData = www.downloadHandler.text.Split('\n');
            commands = www.downloadHandler.text.Split('\n').ToList();
            commands.RemoveAll(string.IsNullOrEmpty);

            // textsLength = textsData.Length;
            textsLength = commands.Count;
            // PreLoad(textsData);
            PreLoadCommand(commands.ToArray());

            DebugConsole.Instance.PrintLog($"Load Story Data: <#00ff00>{textPath}</color>");
        }

        void Initialize()
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

            DebugConsole.Instance.PrintLog("\n<#ffa500>Initialize</color>");
        }


        string DebugArrayList(string[] message)
        {
            // string result = "";
            // foreach (string text in message)
            //     result += $"{text} ".Trim();

            return string.Join(" ", new List<string>(message).ConvertAll(i => i.ToString()).ToArray());
        }

        string[] ParseCommand(string textCommand)
        {
            /*
            * Split command with space delimiter then return as array.
            *
            * You can write a word without using single or double quote.
            * Example: txt Arona Hello
            *
            * But, you must use either single or double quote if
            * you write a sentence that have 'Space' character.
            * Example: txt Arona 'Hello, Sensei.'
            *
            * You can write a single or double quote in a word or
            * sentence by escaping the character: txt Arona 'It\'s yummy.'
            * or use double quote as delimiter: txt Arona "It's yummy."
            * and vice versa.
            *
            * Source: txt 'Arona' "Arona" "I'm, Arona." 'Say, "Hello"' 'You\'re'
            * Result: ["txt", "'Arona'", "\"Arona\"", "\"I'm, Arona.\"", "'Say, "Hello"'", "'You\'re'"]
            */
            const string pattern = @"('[^'\\]*(?:\\.[^'\\]*)*')|(\""[^""\\]*(?:\\.[^""\\]*)*"")|(\S+)";
            String[] textSplit = Regex
                .Matches(textCommand, pattern)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();

            /*
            * Normalize splitted text by removing either single or
            * double quote at the start and end of the word.
            * Also Unescape escaped character.
            *
            * Using string.Trim() resulting in unexpected result, so
            * I reconstruct it using string.Substring().
            *
            * Result: ["txt", "Arona", "Arona", "I'm, Arona.", "Say, "Hello"", "You're"]
            */
            var finalCommand = new List<string>();

            foreach (String str in textSplit)
            {
                String cmd = str;

                if ((str.StartsWith("'") && str.EndsWith("'")) || (str.StartsWith("\"") && str.EndsWith("\"")))
                {
                    cmd = str.Substring(1, str.Length - 2);
                    cmd = Regex.Unescape(cmd);
                }

                finalCommand.Add(cmd);
            }

            return finalCommand.ToArray();
        }

        public void PreLoadCommand(string text)
        {
            var command = ParseCommand(text);

            switch (command[1])
            {
                case "spr":
                    asCharacterManager.AsCharacter_LoadCommand(command, false);
                    nameIdList.Add("char", command[3]);
                    break;
                case "sprc":
                case "spr_c":
                    asCharacterManager.AsCharacter_LoadCommand(command, true);
                    nameIdList.Add("char", command[3]);
                    break;

                case "bg":
                case "si":
                    imageFactory.Image_LoadCommand(command);
                    break;

                case "bgm":
                    asAudioManager.AsAudio_LoadCommand(command, "bgm");
                    nameIdList.Add("bgm", command[3]);
                    break;
                case "sfx":
                    asAudioManager.AsAudio_LoadCommand(command, "sfx");
                    nameIdList.Add("sfx", command[3]);
                    break;
            }
        }

        private void PreLoadCommand(string[] texts)
        {
            Initialize();

            for (int lineIndex = 0; lineIndex < texts.Length; lineIndex++)
            {
                string text = texts[lineIndex];

                if (text.StartsWith("target")) targetList.Add(text.Replace("target", "").Trim(), lineIndex);

                else if (text.StartsWith("load")) PreLoadCommand(text);
            }
        }

        private void ShowAllTargets()
        {
            foreach (KeyValuePair<string, int> target in targetList)
            {
                DebugConsole.Instance.PrintLog($"Target: <#00ff00>{target.Key}</color> at line <#00ff00>{target.Value}</color>");
            }
        }


        public void ComandManager(string text)
        {
            while (true)
            {
                if (text == string.Empty || text.StartsWith("//")) return;

                if (text.StartsWith("="))
                {
                    isPlaying = false;
                    return;
                }

                var command = ParseCommand(text);

                switch (command[0])
                {
                    // special commands
                    case "wait":
                        break;
                    case "targets":
                        ShowAllTargets();
                        break;
                    case "jump":
                        runLineNumber = targetList[command[1]];
                        DebugConsole.Instance.PrintLog($"Jump to: <#00ff00>{command[1]}</color>");
                        break;
                    case "auto":
                        autoSeconds = float.Parse(command[1]);
                        DebugConsole.Instance.PrintLog($"Auto: <#00ff00>{autoSeconds} s</color>");
                        break;
                    case "switch":
                        break;

                    // select commands
                    case "select":
                        selectButton.SelectCommand(command);
                        break;

                    // text commands
                    case "t":
                    case "text":
                    case "txt":
                        break;

                    case "th":
                        break;

                    case "label":
                        break;

                    case "banner":
                        break;

                    // image commands
                    case "bg":
                    case "si":
                        imageFactory.ImageCommand(command);
                        break;

                    // audio commands
                    case "bgm":
                    case "sfx":
                        asAudioManager.AsAudioCommand(command);
                        break;

                    // scene commands
                    case "scene":
                        break;

                    // character commands
                    case "spr":
                    case "char":
                        asCharacterManager.AsCharacterCommand(command);
                        break;

                    default:
                        text = $"char {text}";
                        continue;
                }

                break;
            }
        }

        void CommandFactory_BK(string text)
        {
            if (text.StartsWith("="))
                isPlaying = false;
            else if (text != string.Empty || text.StartsWith("//"))
            {
                string[] command = ParseCommand(text);

                switch (command[0])
                {
                    // End
                    case "end":
                        end.EndCommand(text);
                        isPlaying = false;
                        break;
                    case "ChangeTxt":
                        isPlaying = false;
                        autoTimer = 0;
                        LoadTextData(command[1]);
                        break;
                    case "wait":
                        waitSeconds = float.Parse(command[1]);
                        isWaiting = true;
                        DebugConsole.Instance.PrintLog($"Wait: <#00ff00>{waitSeconds} s</color>");
                        break;
                    case "auto":
                        autoSeconds = float.Parse(command[1]);
                        DebugConsole.Instance.PrintLog(
                            $"Auto Seconds: <#00ff00>{autoSeconds} s</color>"
                        );
                        break;
                    case "jump":
                        runLineNumber = targetList[command[1]];
                        DebugConsole.Instance.PrintLog($"Jump: <#00ff00>{command[1]}</color>");
                        break;

                    // Text
                    case "txt":
                    case "t":
                        isPlaying = false;
                        textArea.SetText(command[1], command[2], command[3]);
                        break;
                    case "tc":
                        textArea.SetText(command[1], command[2], command[3]);
                        break;
                    case "th":
                        sprFactory.TextWithHl(command[1]);
                        isPlaying = false;
                        textArea.SetText(command[1], command[2], command[3]);
                        break;
                    case "text":
                        textArea.TextCommand(command);
                        break;

                    // Button
                    case "button":
                        isPlaying = false;
                        isSelecting = true;
                        selectButton.SelectCommand(command);
                        break;
                    case "buttonS":
                        isPlaying = false;
                        isSelecting = true;
                        selectButton.SelectCommandWithSelect(int.Parse(command[1]), command);
                        break;

                    // Label
                    case "label":
                        label.SetLabelText(command[1]);
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
                    case "curtain":
                        screenEffectFactory.ScreenEffectCommand(text);
                        break;

                    // Spr
                    case "spr":
                        sprFactory.SprCommand(text);
                        break;

                    // Image
                    case "bg":
                    case "si":
                        imageFactory.ImageCommand(text);
                        break;

                    // Sound
                    case "bgm":
                    case "se":
                        soundFactory.SoundCommand(text);
                        break;

                    default:
                        sprFactory.SprCommand($"spr {text}");
                        break;
                }
            }
        }
    }
}
