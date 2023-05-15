using DG.Tweening;
using UnityEngine;

namespace ArisStudio.UI
{
    [AddComponentMenu("Aris Studio/UI/Animations/Settings Menu Animation")]
    public class SettingsMenuAnimation : MonoBehaviour
    {
        [Header("Animation Components")]
        [SerializeField] CanvasGroup m_MainMenuRoot;
        [SerializeField] CanvasGroup m_MainMenuContent;
        [SerializeField] CanvasGroup m_MainMenuOverlay;
        [SerializeField] RectTransform m_MainMenuOverlayBackground;
        [SerializeField] CanvasGroup m_MainMenuBrand;
        float brandWaitTime;
        Vector2 overlayBackgroundSize;

        void Start()
        {
            overlayBackgroundSize = m_MainMenuOverlayBackground.sizeDelta;
            CloseMainMenu();
        }

        public void OpenMainMenu()
        {
            m_MainMenuRoot.gameObject.SetActive(true);
            m_MainMenuRoot.interactable = true;
            m_MainMenuRoot.blocksRaycasts = true;
            m_MainMenuRoot.DOFade(1, 0.2f);

            m_MainMenuOverlay.gameObject.SetActive(true);
            m_MainMenuOverlay.alpha = 1;

            m_MainMenuOverlayBackground.sizeDelta = overlayBackgroundSize;

            m_MainMenuContent.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBounce);
            m_MainMenuContent.DOFade(1, 0.2f).OnComplete(() => {
                m_MainMenuBrand.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBounce);
                m_MainMenuBrand.DOFade(1, 0.2f);
                DOTween
                .To(() => brandWaitTime, x => brandWaitTime = x, 1, 0.8f)
                .OnComplete(() =>
                {
                    m_MainMenuBrand.DOFade(0, 0.2f);
                    m_MainMenuOverlayBackground.DOSizeDelta(new Vector2(m_MainMenuOverlayBackground.sizeDelta.x, 0), 0.2f).OnComplete(() => {
                        m_MainMenuOverlay.DOFade(0, 0.2f).OnComplete(() => {
                            m_MainMenuOverlay.gameObject.SetActive(false);
                        });
                    });
                });
            });
        }

        public void CloseMainMenu()
        {
            m_MainMenuRoot.interactable = false;
            m_MainMenuRoot.blocksRaycasts = false;
            m_MainMenuBrand.transform.localScale = Vector3.one / 2;
            m_MainMenuOverlay.gameObject.SetActive(false);

            DOTween.Kill(m_MainMenuContent);
            DOTween.Kill(m_MainMenuBrand);
            DOTween.Kill(m_MainMenuOverlayBackground);
            DOTween.Kill(m_MainMenuOverlay);

            m_MainMenuRoot.DOFade(0, 0.2f).OnComplete(() =>
            {
                m_MainMenuContent.transform.localScale = Vector3.zero;
                m_MainMenuRoot.gameObject.SetActive(false);
            });
        }
    }
}
