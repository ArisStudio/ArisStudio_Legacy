using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.Core
{
    /// <summary>
    /// Class that responsible for all settings.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Settings Manager")]
    public class SettingsManager : MonoBehaviour
    {
        [Header("Story")]
        [SerializeField] TMP_InputField m_StoryPathInput;

        public string currentStoryFilePath { get; private set; }

        [Header("Display")]
        [SerializeField] TMP_Dropdown m_ScreenResolutionDropdown;
        [SerializeField] TMP_Dropdown m_FPSLimitDropdown, m_FontListDropdown;
        [SerializeField] Toggle m_ToggleFullScreen, m_ToggleVSync;
        [SerializeField] Slider m_TypingIntervalSlider;
        [SerializeField] TMP_Text m_TypingIntervalValue;
        [SerializeField] Slider m_DialoguePanelOpacitySlider;
        [SerializeField] TMP_Text m_DialoguePanelOpacityValue;

        List<string> installedFonts;
        public float currentTypingInterval { get; set; } = 0.2f;
        float currentDialogueBackgroundPanelOpacity;

        [Header("Audio")]
        [SerializeField] AudioSource m_BGMAudioSource;
        [SerializeField] Slider m_BGMVolumeSlider;
        [SerializeField] TMP_Text m_BGMVolumeText;

        [SerializeField] AudioSource m_SFXAudioSource;
        [SerializeField] Slider m_SFXVolumeSlider;
        [SerializeField] TMP_Text m_SFXVolumeText;

        [SerializeField] AudioSource m_VoiceAudioSource;
        [SerializeField] Slider m_VoiceVolumeSlider;
        [SerializeField] TMP_Text m_VoiceVolumeText;

        public float currentBGMVolume { get; private set; }
        public float currentSFXVolume { get; private set; }
        public float currentVoiceVolume { get; private set; }

        [Header("Data")]
        [SerializeField] TMP_InputField m_LocalDataPathInput;
        [SerializeField] TMP_InputField m_RemoteDataPathInput;

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
        const string DEFAULT_CHARACTER_DIRECTORY_NAME = "Character";
        const string DEFAULT_AUDIO_DIRECTORY_NAME = "Audio";
        const string DEFAULT_BGM_DIRECTORY_NAME = "BGM";
        const string DEFAULT_SFX_DIRECTORY_NAME = "SFX";
        const string DEFAULT_VOICE_DIRECTORY_NAME = "Voice";
        const string DEFAULT_IMAGE_DIRECTORY_NAME = "Image";
        const string DEFAULT_BACKGROUND_DIRECTORY_NAME = "Background";
        const string DEFAULT_COVER_DIRECTORY_NAME = "Cover";

        DebugConsole debugConsole;
        DialogueManager dialogueManager;

        void Awake()
        {
            debugConsole = MainControl.Instance.m_DebugConsole;
            dialogueManager = MainControl.Instance.m_DialogueManager;

            /*
            * Initialize early before Start() method because other class
            * maybe requesting the Data path in Start() method.
            */
            // InitializeDefaultLocalDataPath();
        }

        void Start()
        {
            InitializeDefaultLocalDataPath();
            InitializeDisplaySettings();
            InitializeAudioSettings();
        }

        #region Story Settings
        /// <summary>
        /// Pop-up File Explorer to select Story .txt file to be run.
        /// NOTE: Does we need our own story file extension?
        /// </summary>
        public void LoadStoryFile()
        {
            if (m_StoryPathInput != null)
            {
                // Set story file filter
                FileBrowser.SetFilters(false, new FileBrowser.Filter("Story File", ".txt"));
                // FileBrowser.SetDefaultFilter(".txt");

                // Show story file browser
                FileBrowser.ShowLoadDialog(
                    (paths) =>
                    {
                        currentStoryFilePath = paths[0];
                        m_StoryPathInput.text = currentStoryFilePath;
                        // TextAssetEditor.Instance.Edit();

                        debugConsole.PrintLog(
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
            else
                Debug.LogError(
                    "Story path Input Field is missing in Settings Manager! Please assign it."
                );
        }
        #endregion

        #region Display Settings
        /// <summary>
        /// Initialize default value for all member of Display Settings.
        /// </summary>
        void InitializeDisplaySettings()
        {
            ChangeScreenResolution();
            ChangeFPSLimit();
            ToggleFullScreen();
            ToggleVSync();
            SetTypingInterval();
            PopulateInstalledFonts();
        }

        /// <summary>
        /// Set screen resolution.
        /// </summary>
        public void ChangeScreenResolution()
        {
            if (m_ScreenResolutionDropdown != null)
            {
                string resolution = m_ScreenResolutionDropdown.options[
                    m_ScreenResolutionDropdown.value
                ].text.Trim();
                string[] size = resolution.Split('x');

                Screen.SetResolution(int.Parse(size[0]), int.Parse(size[1]), Screen.fullScreen);
                debugConsole.PrintLog($"Resolution = {resolution}");
            }
            else
                Debug.LogError(
                    "Screen resolution Dropdown is missing in Settings Manager! Please assign it."
                );
        }

        /// <summary>
        /// Set FPS limit.
        /// </summary>
        public void ChangeFPSLimit()
        {
            if (m_FPSLimitDropdown != null)
            {
                string fps = m_FPSLimitDropdown.options[m_FPSLimitDropdown.value].text.Trim();

                if (fps == "Unlimited")
                    Application.targetFrameRate = -1;
                else
                    Application.targetFrameRate = int.Parse(fps);

                debugConsole.PrintLog($"FPS Limit = {fps}");
            }
            else
                Debug.LogError(
                    "FPS limit Dropdown is missing in Settings manager! Please assign it."
                );
        }

        /// <summary>
        /// Toggle Full Screen on/off.
        /// </summary>
        public void ToggleFullScreen()
        {
            if (m_ToggleFullScreen != null)
            {
                // bool value = !(m_ToggleFullScreen.isOn = !m_ToggleFullScreen.isOn);
                bool value = m_ToggleFullScreen.isOn;
#if !UNITY_EDITOR
                Screen.fullScreen = value;
#endif
                debugConsole.PrintLog($"Full Screen = {value}");
            }
            else
                Debug.LogError(
                    "Toggle Full Screen is missing in Settings Manager! Please assign it."
                );
        }

        /// <summary>
        /// Toggle VSync on/off.
        /// </summary>
        public void ToggleVSync()
        {
            if (m_ToggleVSync != null)
            {
                // bool value = !(m_ToggleVSync.isOn = !m_ToggleVSync.isOn);
                bool value = m_ToggleVSync.isOn;
                QualitySettings.vSyncCount = Convert.ToInt32(value);

                debugConsole.PrintLog(
                    $"VSync = {Convert.ToBoolean(QualitySettings.vSyncCount)}"
                );
            }
            else
                Debug.LogError("Toggle VSync is missing in Settings Manager! Please assign it.");
        }

        /// <summary>
        /// Set dialogue typing interval in seconds.
        /// </summary>
        public void SetTypingInterval()
        {
            if (m_TypingIntervalSlider != null)
            {
                currentTypingInterval = m_TypingIntervalSlider.value;
                m_TypingIntervalValue.text = $"{Math.Round(currentTypingInterval, 2).ToString("0.##")} s";
            }
            else
                Debug.LogError(
                    "Dialogue typing interval Slider is missing in Settings Manager! Please assign it."
                );
        }

        /// <summary>
        /// Return OS installed font names as a List.
        /// </summary>
        List<string> GetInstalledFontsNames()
        {
            return Font.GetOSInstalledFontNames().ToList();
        }

        /// <summary>
        /// Add installed font names to Dropdown options.
        /// </summary>
        void PopulateInstalledFonts()
        {
            installedFonts = GetInstalledFontsNames();

            if (m_FontListDropdown != null)
                m_FontListDropdown.AddOptions(installedFonts);
            else
                Debug.LogError(
                    "Font list Dropdown is missing in Settings Manager! Please assign it."
                );
        }

        /// <summary>
        /// Set default font from selected Font Dropdown list.
        /// </summary>
        public void SetDefaultFont()
        {
            // See: https://forum.unity.com/threads/creating-tmp-font-during-build-runtime.1066712/#post-6888209
            string[] fontPaths = Font.GetPathsToOSFonts();
            Font selectedFont = new Font(fontPaths[m_FontListDropdown.value]);
            TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(selectedFont);

            TMP_Text[] texts = FindObjectsOfType<TMP_Text>();
            foreach (TMP_Text text in texts)
                text.font = fontAsset;
        }

        public void SetDialoguePanelOpacity()
        {
            if (m_DialoguePanelOpacitySlider != null)
            {
                currentDialogueBackgroundPanelOpacity = m_DialoguePanelOpacitySlider.value;
                m_DialoguePanelOpacityValue.text = Math.Round(currentDialogueBackgroundPanelOpacity, 2).ToString("0.##");

                if (dialogueManager.m_DialoguePanelBackground != null)
                    dialogueManager.m_DialoguePanelBackground.alpha = currentDialogueBackgroundPanelOpacity;
                else
                    Debug.LogError("Dialogue panel background is missing in Dialogue Manager! Please assign it.");
            }
            else
                Debug.LogError("Dialogue panel opacity Slider is missing in Settings Manager! Please assign it.");
        }
        #endregion

        #region Audio Settings
        /// <summary>
        /// Initialize default value for all member of Audio Settings.
        /// </summary>
        void InitializeAudioSettings()
        {
            ChangeBGMVolume();
            ChangeSFXVolume();
            ChangeVoiceVolume();
        }

        /// <summary>
        /// Change BGM volume.
        /// </summary>
        public void ChangeBGMVolume()
        {
            if (m_BGMAudioSource != null && m_BGMVolumeSlider != null && m_BGMVolumeText != null)
            {
                currentBGMVolume = m_BGMVolumeSlider.normalizedValue;
                m_BGMAudioSource.volume = currentBGMVolume;
                m_BGMVolumeText.text = Math.Round(currentBGMVolume, 2).ToString("0.##");
            }
            else
                Debug.LogError("One or more component of BGM audio is missing in Settings Manager! Please assign it.");
        }

        /// <summary>
        /// Change SFX volume.
        /// </summary>
        public void ChangeSFXVolume()
        {
            if (m_SFXAudioSource != null && m_SFXVolumeSlider != null && m_SFXVolumeText != null)
            {
                currentSFXVolume = m_SFXVolumeSlider.normalizedValue;
                m_SFXAudioSource.volume = currentSFXVolume;
                m_SFXVolumeText.text = Math.Round(currentSFXVolume, 2).ToString("0.##");
            }
            else
                Debug.LogError("One or more component of SFX audio is missing in Settings Manager! Please assign it.");
        }

        /// <summary>
        /// Change Voice volume.
        /// </summary>
        public void ChangeVoiceVolume()
        {
            if (m_VoiceAudioSource != null && m_VoiceVolumeSlider != null && m_SFXVolumeText != null)
            {
                currentVoiceVolume = m_VoiceVolumeSlider.normalizedValue;
                m_VoiceAudioSource.volume = currentVoiceVolume;
                m_VoiceVolumeText.text = Math.Round(currentVoiceVolume, 2).ToString("0.##");
            }
            else
                Debug.LogError("One or more component of Voice audio is missing in Settings Manager! Please assign it.");
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
                debugConsole.PrintLog(
                    $"Set Data path: <#00ff00>{currentLocalDataPath}</color>"
                );
            }
            else
            {
                currentLocalDataPath = GetRootPath();
                debugConsole.PrintLog(
                    $"Default Data path didn\'t exist: <#ff0000>{defaultLocalDataPath}</color>.\nSet Data path to: <#00ff00>{GetRootPath()}</color>.\nIf it\'s not correct path, please fix this by choosing the correct path at Data Settings."
                );
            }

            SetDataContentsPath();

            // Update the Input Field text
            if (m_LocalDataPathInput != null)
                m_LocalDataPathInput.text = currentLocalDataPath;
            else
                Debug.LogError(
                    "Local Data path Input Field is missing in Settings Manager! Please assign it."
                );
        }

        /// <summary>
        /// Pop-up File Explorer to set local Data path.
        /// </summary>
        public void SetLocalDataPath()
        {
            if (m_LocalDataPathInput != null)
            {
                FileBrowser.ShowLoadDialog(
                    (paths) =>
                    {
                        currentLocalDataPath = paths[0];
                        m_LocalDataPathInput.text = currentLocalDataPath;
                        SetDataContentsPath();

                        debugConsole.PrintLog(
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
            else
                Debug.LogError(
                    "Data path Input Field is missing in Settings Manager! Please assign it."
                );
        }

        /// <summary>
        /// Set Data contents (Spr, Audio, Image, etc.) path.
        /// </summary>
        void SetDataContentsPath()
        {
            currentSprPath = Path.Combine(currentLocalDataPath, DEFAULT_SPR_DIRECTORY_NAME);
            currentCharacterPath = Path.Combine(
                currentLocalDataPath,
                DEFAULT_CHARACTER_DIRECTORY_NAME
            );

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
