using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.Utils;
using SimpleFileBrowser;
using UnityEngine;
using TMPro;

namespace ArisStudio.Core
{
    /// <summary>
    /// Class that responsible for storing all settings variable and utility.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Settings Manager [Singleton]")]
    public class SettingsManager : Singleton<SettingsManager>
    {
        #region Story
        public string currentStoryFilePath { get; private set; }
        #endregion

        #region Display
        public List<string> currentInstalledFonts { get; private set; }
        public string currentScreenResolution { get; private set; }
        public string currentFPSLimit { get; private set; }
        public int currentDefaultFont { get; private set; }
        public bool currentFullScreenState { get; private set; }
        public bool currentVSyncState { get; private set; }
        public float currentTypingInterval { get; set; } // used in interval command
        public float currentDialogueBackgroundPanelOpacity { get; private set; }
        #endregion

        #region Audio
        public float currentBGMVolume { get; set; }
        public float currentSFXVolume { get; set; }
        public float currentVoiceVolume { get; set; }
        #endregion

        #region Data
        public string currentLocalDataPath { get; private set; }
        public string currentRemoteDataPath { get; private set; }
        public string currentSprPath { get; private set; }
        public string currentCharacterPath { get; private set; }
        public string currentAudioPath { get; private set; }
        public string currentBGMPath { get; private set; }
        public string currentSFXPath { get; private set; }
        public string currentVoicePath { get; private set; }
        public string currentImagePath { get; private set; }
        public string currentBackgroundPath { get; private set; }
        public string currentCoverPath { get; private set; }

        const string DEFAULT_DATA_DIRECTORY_NAME = "Data";
        const string DEFAULT_SPR_DIRECTORY_NAME = "Spr";
        const string DEFAULT_AUDIO_DIRECTORY_NAME = "Audio";
        const string DEFAULT_BGM_DIRECTORY_NAME = "BGM";
        const string DEFAULT_SFX_DIRECTORY_NAME = "SFX";
        const string DEFAULT_VOICE_DIRECTORY_NAME = "Voice";
        const string DEFAULT_IMAGE_DIRECTORY_NAME = "Image";
        const string DEFAULT_BACKGROUND_DIRECTORY_NAME = "Background";
        const string DEFAULT_COVER_DIRECTORY_NAME = "Cover";
        #endregion

        void Awake()
        {
            /*
            * Setting default local data path debug message is get
            * printed before clearing any existing message, so we
            * need we clear it before setting the path from here.
            * It's order of execution matter.
            */
            DebugConsole.Instance.m_DebugText.text = string.Empty;
            InitializeDefaultLocalDataPath();
            // InitializeFontsList();
        }

        #region Story Settings
        /// <summary>
        /// Pop-up File Explorer to select Story .txt file to be run.
        /// NOTE: Does we need our own story file extension?
        /// </summary>
        public void GetStoryFile()
        {
            // Set story file filter
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Story File", ".txt"));
            // FileBrowser.SetDefaultFilter(".txt");

            // Show story file browser
            FileBrowser.ShowLoadDialog(
                (paths) =>
                {
                    currentStoryFilePath = paths[0];

                    DebugConsole.Instance.PrintLog(
                        $"Select Story: <#00ff00>{currentStoryFilePath}</color>"
                    );
                },
                () => { },
                FileBrowser.PickMode.Files,
                false,
                GetRootPath(),
                null,
                "Select Story File",
                "Select"
            );
        }
        #endregion

        #region Display Settings
        /// <summary>
        /// Set screen resolution.
        /// The value must be a string that look like this: 1280x720
        /// </summary>
        public void SetScreenResolution(string value)
        {
            currentScreenResolution = value.Trim();
            string[] size = currentScreenResolution.Split('x');

            Screen.SetResolution(int.Parse(size[0]), int.Parse(size[1]), Screen.fullScreen);
            DebugConsole.Instance.PrintLog($"Resolution = {currentScreenResolution}");
        }

        /// <summary>
        /// Set FPS limit.
        /// </summary>
        public void SetFPSLimit(string value)
        {
            currentFPSLimit = value.Trim();

            if (currentFPSLimit == "Unlimited")
                Application.targetFrameRate = -1;
            else
                Application.targetFrameRate = int.Parse(currentFPSLimit);

            DebugConsole.Instance.PrintLog($"FPS Limit = {currentFPSLimit}");
        }

        /// <summary>
        /// Return OS installed font names as a List.
        /// </summary>
        List<string> GetInstalledFontsNames()
        {
            return Font.GetOSInstalledFontNames().ToList();
        }

        void InitializeFontsList()
        {
            currentInstalledFonts = GetInstalledFontsNames();
        }

        /// <summary>
        /// Set default font from selected Font Dropdown list.
        /// </summary>
        public void SetDefaultFont(List<TMP_Text> targetTexts, int fontPathIndex)
        {
            // See: https://forum.unity.com/threads/creating-tmp-font-during-build-runtime.1066712/#post-6888209

            currentDefaultFont = fontPathIndex;
            string[] fontPaths = Font.GetPathsToOSFonts();
            Font selectedFont = new Font(fontPaths[currentDefaultFont]);
            TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(selectedFont);

            foreach (TMP_Text text in targetTexts)
                text.font = fontAsset;
        }

        /// <summary>
        /// Set dialogue typing interval in seconds.
        /// </summary>
        public void SetTypingInterval(float value)
        {
            currentTypingInterval = value;
        }

        /// <summary>
        /// Set dialogue panel background opacity.
        /// </summary>
        public void SetDialogueBackgroundPanelOpacity(float value)
        {
            currentDialogueBackgroundPanelOpacity = value;
        }

        /// <summary>
        /// Set Full Screen on/off.
        /// </summary>
        public void SetFullScreen(bool value)
        {
            currentFullScreenState = value;
#if !UNITY_EDITOR
            Screen.fullScreen = currentFullScreenState;
#endif
            DebugConsole.Instance.PrintLog($"Full Screen = {currentFullScreenState}");
        }

        /// <summary>
        /// Set VSync on/off.
        /// </summary>
        public void SetVSync(bool value)
        {
            currentVSyncState = value;
            QualitySettings.vSyncCount = Convert.ToInt32(currentVSyncState);
            DebugConsole.Instance.PrintLog(
                $"VSync = {Convert.ToBoolean(QualitySettings.vSyncCount)}"
            );
        }
        #endregion

        #region Audio Settings
        /// <summary>
        /// Set an audio volume.
        /// </summary>
        public void SetAudioVolume(AudioSource audioSource, TMP_Text valueText, float valueSource)
        {
            audioSource.volume = valueSource;
            valueText.text = Math.Round(valueSource, 2).ToString("0.##");
        }
        #endregion

        #region Data Settings
        /// <summary>
        /// Return current project directory/installed directory.
        /// </summary>
        /// <returns></returns>
        string GetRootPath()
        {
#if UNITY_ANDROID
            string path = $"file:///{Application.persistentDataPath}";
#elif UNITY_STANDALONE_OSX
            string path = Directory.GetParent($"file://{Application.dataPath}").ToString();
#else
            string path = Directory.GetParent(Application.dataPath).ToString();
#endif
            return path;
        }

        /// <summary>
        /// Initialize local Data path.
        /// </summary>
        void InitializeDefaultLocalDataPath()
        {
            // Initialize current Data path from default Data path if exist
            string defaultLocalDataPath = Path.Combine(GetRootPath(), DEFAULT_DATA_DIRECTORY_NAME);

            if (Directory.Exists(defaultLocalDataPath))
            {
                currentLocalDataPath = defaultLocalDataPath;
                DebugConsole.Instance.PrintLog(
                    $"Set Data path: <#00ff00>{currentLocalDataPath}</color>"
                );
            }
            else
            {
                currentLocalDataPath = GetRootPath();
                DebugConsole.Instance.PrintLog(
                    $"Default Data path didn\'t exist: <#ff0000>{defaultLocalDataPath}</color>.\nSet Data path to: <#00ff00>{GetRootPath()}</color>.\nIf it\'s not correct path, please fix this by choosing the correct path at Data Settings."
                );
            }

            SetDataContentsPath();
        }

        /// <summary>
        /// Pop-up File Explorer to set local Data path.
        /// </summary>
        public void SetLocalDataPath()
        {
            FileBrowser.ShowLoadDialog(
                (paths) =>
                {
                    currentLocalDataPath = paths[0];
                    SetDataContentsPath();

                    DebugConsole.Instance.PrintLog(
                        $"Select Data path: <#00ff00>{currentLocalDataPath}</color>"
                    );
                },
                () => { },
                FileBrowser.PickMode.Folders,
                false,
                GetRootPath(),
                null,
                "Select Data Folder",
                "Load"
            );
        }

        /// <summary>
        /// Set Data contents (Spr, Audio, Image, etc.) path.
        /// </summary>
        void SetDataContentsPath()
        {
            currentSprPath = Path.Combine(currentLocalDataPath, DEFAULT_SPR_DIRECTORY_NAME);

            currentAudioPath = Path.Combine(currentLocalDataPath, DEFAULT_AUDIO_DIRECTORY_NAME);
            currentBGMPath = Path.Combine(currentAudioPath, DEFAULT_BGM_DIRECTORY_NAME);
            currentSFXPath = Path.Combine(currentAudioPath, DEFAULT_SFX_DIRECTORY_NAME);
            currentVoicePath = Path.Combine(currentAudioPath, DEFAULT_VOICE_DIRECTORY_NAME);

            currentImagePath = Path.Combine(currentLocalDataPath, DEFAULT_IMAGE_DIRECTORY_NAME);
            currentBackgroundPath = Path.Combine(
                currentImagePath,
                DEFAULT_BACKGROUND_DIRECTORY_NAME
            );
            currentCoverPath = Path.Combine(currentImagePath, DEFAULT_COVER_DIRECTORY_NAME);
        }
        #endregion
    }
}
