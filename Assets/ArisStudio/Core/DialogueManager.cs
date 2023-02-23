using ArisStudio.UI;
using UnityEngine;
//using KoganeUnityLib;
using TMPro;

namespace ArisStudio.Core
{
    /// <summary>
    /// Class that responsible for all about Dialogue related funtionality.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Dialogue Manager")]
    public class DialogueManager : MonoBehaviour
    {
        [Header("Dialogue UI")]
        [SerializeField] public CanvasGroup m_DialoguePanelBackground;
        [SerializeField] TMP_Text m_CharacterNameText;
        [SerializeField] TMP_Text m_CharacterGroupText;
        [SerializeField] TMP_Text m_DialogueText;
        [Space]
        [SerializeField] DialogueIndicatorAnimation m_DialogueIndicator;

        // SprFactory sprFactory;
        // ImageFactory imageFactory;
        // SoundFactory soundFactory;
        // ScreenEffectFactory screenEffectFactory;

        DebugConsole debugConsole;

        void Awake()
        {
            debugConsole = MainControl.Instance.m_DebugConsole;

            // sprFactory = FindObjectOfType<SprFactory>();
            // imageFactory = FindObjectOfType<ImageFactory>();
            // soundFactory = FindObjectOfType<SoundFactory>();
            // screenEffectFactory = FindObjectOfType<ScreenEffectFactory>();
        }

        void Start()
        {
            // sprFactory.Initialize();
            // imageFactory.Initialize();
            // soundFactory.Initialize();
        }

        /// <summary>
        /// Initialize Dialogue related settings before playing the story.
        /// </summary>
        public void Initialize()
        {
            if (m_CharacterNameText != null && m_CharacterGroupText != null && m_DialogueText != null)
            {
                m_CharacterNameText.text = string.Empty;
                m_CharacterGroupText.text = string.Empty;
                m_DialogueText.text = string.Empty;
            }
            else
                Debug.LogError("One or more Dialogue UI is missing in Dialogue Manager! Please assign it.");
        }
    }
}
