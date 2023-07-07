using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio
{
    public class Banner : MonoBehaviour
    {
        public GameObject blur, menu, textArea;

        [Header("Banner1")] public GameObject b1;
        public Text t1;

        [Header("Banner2")] public GameObject b2;
        public Text t21, t22;

        public void SetBanner1Text(string text)
        {
            textArea.SetActive(false);
            menu.SetActive(false);
            blur.SetActive(true);
            t1.text = text.Split('`')[1];
            b1.SetActive(true);
        }

        public void SetBanner2Text(string text)
        {
            textArea.SetActive(false);
            menu.SetActive(false);
            var tt = text.Split('`');
            blur.SetActive(true);
            t21.text = tt[1];
            t22.text = tt[3];
            b2.SetActive(true);
        }

        public void CloseBanner()
        {
            blur.SetActive(false);
            b1.SetActive(false);
            b2.SetActive(false);
            menu.SetActive(true);
        }
    }
}