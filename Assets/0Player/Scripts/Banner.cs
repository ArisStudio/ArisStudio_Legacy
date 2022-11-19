using UnityEngine;
using UnityEngine.UI;

public class Banner : MonoBehaviour
{
    public Text bannerTxt;

    public void SetBannerTxt(string text)
    {
        bannerTxt.text = text;
        gameObject.SetActive(true);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
