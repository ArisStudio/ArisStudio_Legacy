using UnityEngine;
using UnityEngine.UI;

public class C_Banner : MonoBehaviour
{
    public Text bannerText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBannerText(string text)
    {
        bannerText.text = text;
        gameObject.SetActive(true);
    }
}
