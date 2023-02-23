using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    [AddComponentMenu("Aris Studio/UI/Animations/Main Menu Animation")]
    public class MainMenuAnimation : MonoBehaviour
    {
        [SerializeField] CanvasGroup m_MainMenuCanvasGroup;
        [Range(0.1f, 2f), SerializeField] float m_AnimationDuration = 0.2f;
        [SerializeField] Ease m_AnimationEase = Ease.Linear;

        void Start()
        {
            CloseMainMenu();
        }

        public void OpenMainMenu()
        {
            m_MainMenuCanvasGroup.gameObject.SetActive(true);
            m_MainMenuCanvasGroup.interactable = true;
            m_MainMenuCanvasGroup.blocksRaycasts = true;
            m_MainMenuCanvasGroup.DOFade(1, m_AnimationDuration).SetEase(m_AnimationEase);
        }

        public void CloseMainMenu()
        {
            m_MainMenuCanvasGroup.interactable = false;
            m_MainMenuCanvasGroup.blocksRaycasts = false;
            m_MainMenuCanvasGroup.DOFade(0, m_AnimationDuration).SetEase(m_AnimationEase).OnComplete(() =>
            {
                m_MainMenuCanvasGroup.gameObject.SetActive(false);
            });
        }
    }
}
