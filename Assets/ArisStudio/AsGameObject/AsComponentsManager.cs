using ArisStudio.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.AsGameObject
{
    public class AsComponentsManager : Singleton<AsComponentsManager>
    {
        [Header("Label")] [SerializeField] private GameObject labelGo;
        [SerializeField] private Text labelText;
        private float labelTime;

        [Header("Banner")] [SerializeField] private GameObject bannerGo;
        [SerializeField] private Text banner1Text, banner21Text, banner22Text;
        private float bannerTime;

        public void AsComponentsInit()
        {
            labelGo.SetActive(false);
            labelText.text = "";
            bannerGo.SetActive(false);
            banner1Text.text = "";
            banner21Text.text = "";
            banner22Text.text = "";
        }

        public void SetLabel(string text)
        {
            labelText.text = text;
            labelGo.SetActive(true);
            DOTween.To(() => labelTime, x => labelTime = x, 2, 1)
                .OnComplete(() => labelGo.SetActive(false));
        }

        public void SetBanner(string[] command)
        {
            if (command.Length == 3)
            {
                banner21Text.text = command[1];
                banner22Text.text = command[2];
                banner1Text.gameObject.SetActive(false);
                banner21Text.gameObject.SetActive(true);
                banner22Text.gameObject.SetActive(true);
            }
            else
            {
                banner1Text.text = command[1];
                banner21Text.gameObject.SetActive(false);
                banner22Text.gameObject.SetActive(false);
                banner1Text.gameObject.SetActive(true);
            }

            bannerGo.SetActive(true);
        }

        public void HideBanner()
        {
            bannerGo.SetActive(false);
        }
    }
}
