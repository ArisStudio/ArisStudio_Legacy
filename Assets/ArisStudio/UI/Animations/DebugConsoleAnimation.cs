using DG.Tweening;
using ArisStudio.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    [AddComponentMenu("Aris Studio/UI/Animations/Debug Console Animation")]
    public class DebugConsoleAnimation : MonoBehaviour
    {
        [SerializeField] RectTransform m_DebugConsoleRect;
        [SerializeField] CanvasGroup m_DebugConsoleCanvasGroup;
        [SerializeField] Button m_ToggleButton;
        [SerializeField] Sprite m_ToggleOpenSprite, m_ToggleCloseSprite;
        [SerializeField, Range(0, 1000f)] float m_WindowMaxHeight = 250f;
        [SerializeField, Range(0.1f, 2f)] float m_AnimationDuration = 0.2f;
        [SerializeField] Ease m_AnimationEase = Ease.Linear;

        bool isOpen = false;

        void Start()
        {
            CloseDebugConsole();
        }

        public void ToggleDebugConsole()
        {
            if (!isOpen)
                OpenDebugConsole();
            else
                CloseDebugConsole();
        }

        void OpenDebugConsole()
        {
            isOpen = true;
            m_DebugConsoleRect.DOSizeDelta(new Vector2(m_DebugConsoleRect.sizeDelta.x, m_WindowMaxHeight), m_AnimationDuration).SetEase(m_AnimationEase).OnComplete(() => {
                m_ToggleButton.GetComponent<Image>().sprite = m_ToggleCloseSprite;
            });
            m_DebugConsoleCanvasGroup.DOFade(1, m_AnimationDuration).SetEase(m_AnimationEase);
        }

        void CloseDebugConsole()
        {
            isOpen = false;
            m_DebugConsoleCanvasGroup.DOFade(0, m_AnimationDuration).SetEase(m_AnimationEase);
            m_DebugConsoleRect.DOSizeDelta(new Vector2(m_DebugConsoleRect.sizeDelta.x, 0), m_AnimationDuration).SetEase(m_AnimationEase).OnComplete(() => {
                m_DebugConsoleCanvasGroup.DOFade(1, 0).SetEase(m_AnimationEase);
                m_ToggleButton.GetComponent<Image>().sprite = m_ToggleOpenSprite;
            });
        }
    }
}
