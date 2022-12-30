using UnityEngine;
using UnityEngine.UI;

public class C_Banner : MonoBehaviour
{
    public Text bannerText;

    public void SetBannerText(string text)
    {
        bannerText.text = text;
        gameObject.SetActive(true);
    }
}