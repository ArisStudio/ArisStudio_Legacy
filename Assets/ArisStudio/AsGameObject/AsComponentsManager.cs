using ArisStudio.Core;
using ArisStudio.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

using UnityImage = UnityEngine.UI.Image;

namespace ArisStudio.AsGameObject
{
    [AddComponentMenu("Aris Studio/AsGameObject/Components Manager")]
    public class AsComponentsManager : Singleton<AsComponentsManager>
    {
        [Header("Menu")]
        [SerializeField]
        private CanvasGroup m_MainMenu;

        [Header("Label")]
        [SerializeField]
        private CanvasGroup m_Label;

        [SerializeField]
        private TMP_Text m_LabelText;
        private float labelTime;

        [Header("Banner")]
        [SerializeField]
        private CanvasGroup m_Banner;

        [SerializeField]
        private UnityImage m_BannerFrame;

        [SerializeField]
        private CanvasGroup m_BannerContent,
            m_BannerTexts,
            m_BannerLine;

        [SerializeField]
        private TMP_Text m_BannerText,
            m_BannerText1,
            m_BannerText2;
        private float bannerTime,
            bannerInitialScale = 0.5f;

        public void AsComponentsInit()
        {
            m_Label.gameObject.SetActive(false);
            m_LabelText.text = "";

            m_Banner.gameObject.SetActive(false);
            m_BannerText.text = "";
            m_BannerText1.text = "";
            m_BannerText2.text = "";
        }

        public void SetLabel(string text)
        {
            m_Label.gameObject.SetActive(true);
            m_LabelText.text = text;
            m_Label.DOFade(1, 1);
            DOTween
                .To(() => labelTime, x => labelTime = x, 1, 3)
                .OnComplete(() =>
                {
                    m_Label.DOFade(0, 1).OnComplete(() => m_Label.gameObject.SetActive(false));
                });
        }

        public void BannerCommand(string[] command)
        {
            if (command[1] == "hide")
            {
                HideBanner();
                return;
            }

            MainManager.Instance.IsPlaying = false;

            m_MainMenu.gameObject.SetActive(false);
            m_Banner.alpha = 1;
            m_BannerFrame.DOFade(1, 0.5f);
            m_BannerContent.transform.localScale = new Vector3(1, bannerInitialScale, 1);
            m_BannerContent.DOFade(1, 0.5f);
            m_BannerContent.transform
                .DOScaleY(1, 0.5f)
                .OnComplete(() =>
                {
                    m_BannerTexts.DOFade(1, 1);
                });

            if (command.Length == 3)
            {
                m_BannerText.gameObject.SetActive(false);
                m_BannerLine.gameObject.SetActive(true);
                m_BannerText1.gameObject.SetActive(true);
                m_BannerText2.gameObject.SetActive(true);
                m_BannerText1.text = command[1];
                m_BannerText2.text = command[2];
            }
            else
            {
                m_BannerText.gameObject.SetActive(true);
                m_BannerText.text = command[1];
                m_BannerText1.gameObject.SetActive(false);
                m_BannerText2.gameObject.SetActive(false);
                m_BannerLine.gameObject.SetActive(false);
            }

            m_Banner.gameObject.SetActive(true);
        }

        public void HideBanner()
        {
            Tween bannerTween = m_Banner
                .DOFade(0, 1);

            MainManager.Instance.IsPlaying = false;

            // Check if 'banner hide' command executed from AsCommandList or Debug Console
            if (
                MainManager.Instance.AsCommandList[MainManager.Instance.RunLineIndex].Contains(
                    "banner hide"
                )
            )
            {
                // If yes, automatically progress the story
                bannerTween.OnComplete(() =>
                {
                    MainManager.Instance.IsPlaying = true;
                    m_Banner.gameObject.SetActive(false);
                    m_MainMenu.gameObject.SetActive(true);
                });
            }
            else
            {
                bannerTween.OnComplete(() => {
                    m_Banner.gameObject.SetActive(false);
                    m_MainMenu.gameObject.SetActive(true);
                });
            }
        }
    }
}
