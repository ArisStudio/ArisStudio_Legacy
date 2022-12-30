using UnityEngine;
using UnityEngine.UI;

public class C_Bg : MonoBehaviour
{
    public RawImage bgGo;

    float showTime = 0;
    float changeShowTime = 0.5f;
    bool showing, hiding;

    void Update()
    {
        if (showing)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || showTime / changeShowTime >= 0.95f)
            {
                showTime = 0;
                bgGo.color = new Color(1, 1, 1, 1);
                showing = false;
            }
            else
            {
                bgGo.color = new Color(1, 1, 1, showTime / changeShowTime);
            }
        }

        else if (hiding)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || showTime / changeShowTime >= 0.95f)
            {
                showTime = 0;
                bgGo.color = new Color(1, 1, 1, 0);
                hiding = false;
            }
            else
            {
                bgGo.color = new Color(1, 1, 1, 1 - showTime / changeShowTime);
            }
        }
    }

    public void SetBg(Texture2D bg)
    {
        bgGo.texture = bg;
        Show();
    }

    public void Show()
    {
        bgGo.color = new Color(1, 1, 1, 0);
        showing = true;
    }

    public void Hide()
    {
        hiding = true;
    }

    public void HideD()
    {
        bgGo.color = Color.black;
    }

    public void ShowD()
    {
        bgGo.color = Color.white;
    }
}