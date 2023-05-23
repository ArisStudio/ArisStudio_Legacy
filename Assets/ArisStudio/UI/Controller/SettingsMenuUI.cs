using System;
using System.Collections;
using ArisStudio.Core;
using ArisStudio.Utils;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ArisStudio.UI
{
    [AddComponentMenu("Aris Studio/UI/Controller/Settings Menu")]
    public class SettingsMenuUI : MonoBehaviour
    {
        [Header("Auto")]
        [SerializeField] public Button m_AutoActivate;
        [SerializeField] public Button m_AutoDeactivate;

        [Header("Story")]
        [SerializeField]
        TMP_InputField m_StoryPathInput;

        [Header("Display")]
        [SerializeField]
        TMP_Dropdown m_ScreenResolutionDropdown;

        [SerializeField]
        TMP_Dropdown m_FPSLimitDropdown,
            m_FontListDropdown;

        [SerializeField]
        Toggle m_ToggleFullScreen,
            m_ToggleVSync;

        [SerializeField]
        Slider m_TypingSpeedSlider;

        [SerializeField]
        TMP_Text m_TypingSpeedValue;

        [SerializeField]
        Slider m_DialoguePanelOpacitySlider;

        [SerializeField]
        TMP_Text m_DialoguePanelOpacityValue;

        [Header("Audio")]
        [SerializeField]
        AudioSource m_BGMAudioSource;

        [SerializeField]
        Slider m_BGMVolumeSlider;

        [SerializeField]
        TMP_Text m_BGMVolumeText;

        [SerializeField]
        AudioSource m_SFXAudioSource;

        [SerializeField]
        Slider m_SFXVolumeSlider;

        [SerializeField]
        TMP_Text m_SFXVolumeText;

        [SerializeField]
        AudioSource m_VoiceAudioSource;

        [SerializeField]
        Slider m_VoiceVolumeSlider;

        [SerializeField]
        TMP_Text m_VoiceVolumeText;

        [Header("Data")]
        [SerializeField]
        TMP_InputField m_LocalDataPathInput;

        [SerializeField]
        TMP_InputField m_RemoteDataPathInput;

#if UNITY_ANDROID
        [Header("Hide Settings - Android")]
        [SerializeField]
        RectTransform[] m_HideSettings;
#endif

        SettingsManager settingsManager;

        private void Awake()
        {
            settingsManager = SettingsManager.Instance;
        }

        private void Start()
        {
            InitializeDisplaySettings();
            // InitializeAudioSettings();
            InitializeDataSettings();
            HideSettings();
        }

        /// <summary>
        /// Hide settings on a specific platform.
        /// </summary>
        private void HideSettings()
        {
#if UNITY_ANDROID
            if (m_HideSettings.Length > 0)
            {
                foreach (RectTransform setting in m_HideSettings)
                {
                    if (setting != null)
                        setting.gameObject.SetActive(false);
                }
            }
#endif
        }

        #region Story Settings

        public void SelectStoryFile()
        {
            StartCoroutine(SelectStoryFileRoutine());
        }

        private IEnumerator SelectStoryFileRoutine()
        {
            settingsManager.GetStoryFile();
            yield return new WaitUntil(() => FileBrowser.Success);
            m_StoryPathInput.text = AsHelper.NormalizePath(settingsManager.currentStoryFilePath);
        }

        #endregion

        #region Display Settings

        void InitializeDisplaySettings()
        {
            ChangeScreenResolution();
            ChangeFPSLimit();
            ChangeTypingSpeed();
            ChangeDialogueBackgroundPanelOpacity();
            ToggleFullScreen();
            ToggleVSync();
        }

        public void ChangeScreenResolution()
        {
            settingsManager.SetScreenResolution(
                m_ScreenResolutionDropdown.options[m_ScreenResolutionDropdown.value].text
            );
        }

        public void ChangeFPSLimit()
        {
            settingsManager.SetFPSLimit(m_FPSLimitDropdown.options[m_FPSLimitDropdown.value].text);
        }

        // public void ChangeDefaultFont()
        // {
        //     // For now, just change all TMP_Text font
        //     List<TMP_Text> texts = FindObjectsOfType<TMP_Text>().ToList();
        //     settingsManager.SetDefaultFont(texts, m_FontListDropdown.value);
        // }

        public void ChangeTypingSpeed()
        {
            settingsManager.SetTypingSpeed(m_TypingSpeedSlider.value);
            m_TypingSpeedValue.text =
                $"{Math.Round(settingsManager.currentTypingSpeed, 2).ToString("0.##")}";
        }

        public void ChangeDialogueBackgroundPanelOpacity()
        {
            settingsManager.SetDialogueBackgroundPanelOpacity(m_DialoguePanelOpacitySlider.value);
            // TODO: Actually change background panel opacity
            m_DialoguePanelOpacityValue.text = Math.Round(
                    settingsManager.currentDialogueBackgroundPanelOpacity,
                    2
                )
                .ToString("0.##");
        }

        public void ToggleFullScreen()
        {
            settingsManager.SetFullScreen(m_ToggleFullScreen.isOn);
        }

        public void ToggleVSync()
        {
            settingsManager.SetVSync(m_ToggleVSync.isOn);
        }

        #endregion

        #region Audio Settings

        // void InitializeAudioSettings()
        // {
        //     ChangeBGMVolume();
        //     ChangeSFXVolume();
        //     ChangeVoiceVolume();
        // }
        //
        // public void ChangeBGMVolume()
        // {
        //     settingsManager.currentBGMVolume = m_BGMVolumeSlider.normalizedValue;
        //     settingsManager.SetAudioVolume(m_BGMAudioSource, m_BGMVolumeText, settingsManager.currentBGMVolume);
        // }
        //
        // public void ChangeSFXVolume()
        // {
        //     settingsManager.currentSFXVolume = m_SFXVolumeSlider.normalizedValue;
        //     settingsManager.SetAudioVolume(m_SFXAudioSource, m_SFXVolumeText, settingsManager.currentSFXVolume);
        // }
        //
        // public void ChangeVoiceVolume()
        // {
        //     settingsManager.currentVoiceVolume = m_VoiceVolumeSlider.normalizedValue;
        //     settingsManager.SetAudioVolume(m_VoiceAudioSource, m_VoiceVolumeText, settingsManager.currentVoiceVolume);
        // }

        #endregion

        #region Data Settings

        private void InitializeDataSettings()
        {
            // Only update the UI, the actual logic is in SettingsManager.
            m_LocalDataPathInput.text = AsHelper.NormalizePath(settingsManager.currentLocalDataPath);
        }

        public void SelectLocalDataPath()
        {
            StartCoroutine(SelectLocalDataPathRoutine());
        }

        private IEnumerator SelectLocalDataPathRoutine()
        {
            settingsManager.SetLocalDataPath();
            yield return new WaitUntil(() => FileBrowser.Success);
            m_LocalDataPathInput.text = AsHelper.NormalizePath(settingsManager.currentLocalDataPath);
        }

        #endregion
    }
}
