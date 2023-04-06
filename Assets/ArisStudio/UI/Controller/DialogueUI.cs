using UnityEngine;
using TMPro;

namespace ArisStudio.UI
{
    [AddComponentMenu("Aris Studio/UI/Controller/Dialogue UI")]
    public class DialogueUI : MonoBehaviour
    {
        [Header("Dialogue UI")]
        [SerializeField] public CanvasGroup m_DialoguePanelBackground;
        [SerializeField] TMP_Text m_CharacterNameText;
        [SerializeField] TMP_Text m_CharacterGroupText;
        [SerializeField] TMP_Text m_DialogueText;
        [Space]
        [SerializeField] DialogueIndicatorAnimation m_DialogueIndicator;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            m_CharacterNameText.text = string.Empty;
            m_CharacterGroupText.text = string.Empty;
            m_DialogueText.text = string.Empty;
        }

        public void SetText()
        {

        }
    }
}
