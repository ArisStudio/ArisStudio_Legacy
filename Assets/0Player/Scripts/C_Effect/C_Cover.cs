using UnityEngine;
using UnityEngine.UI;

public class C_Cover : MonoBehaviour
{
    public RawImage coverGo;

    public void SetCover(Texture2D cover)
    {
        coverGo.rectTransform.sizeDelta = new Vector2(cover.width, cover.height) / 3;
        coverGo.texture = cover;
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}