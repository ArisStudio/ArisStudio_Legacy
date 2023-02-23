using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    [AddComponentMenu("Aris Studio/UI/Animations/Debug Console Animation")]
    public class DebugConsoleAnimation : MonoBehaviour
    {
        [SerializeField] Toggle m_ToggleDebugConsole;
        [SerializeField] CanvasGroup m_DebugConsoleCanvasGroup;
        [SerializeField, Range(0.1f, 2f)] float m_AnimationDuration = 0.2f;
        [SerializeField] Ease m_AnimationEase = Ease.Linear;

        void Start()
        {
            ToggleDebugConsole();
        }

        public void ToggleDebugConsole()
        {
            if (m_ToggleDebugConsole.isOn)
                OpenDebugConsole();
            else
                CloseDebugConsole();
        }

        public void OpenDebugConsole()
        {
            m_ToggleDebugConsole.isOn = true;
            m_DebugConsoleCanvasGroup.gameObject.SetActive(true);
            m_DebugConsoleCanvasGroup.interactable = true;
            m_DebugConsoleCanvasGroup.blocksRaycasts = true;
            m_DebugConsoleCanvasGroup.DOFade(1, m_AnimationDuration).SetEase(m_AnimationEase);
        }

        public void CloseDebugConsole()
        {
            m_ToggleDebugConsole.isOn = false;
            m_DebugConsoleCanvasGroup.interactable = false;
            m_DebugConsoleCanvasGroup.blocksRaycasts = false;
            m_DebugConsoleCanvasGroup.DOFade(0, m_AnimationDuration).SetEase(m_AnimationEase).OnComplete(() =>
            {
                m_DebugConsoleCanvasGroup.gameObject.SetActive(true);
            });
        }
    }
}
