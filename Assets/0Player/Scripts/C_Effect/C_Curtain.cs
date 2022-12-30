using UnityEngine;
using UnityEngine.UI;

public class C_Curtain : MonoBehaviour
{
    public Image iGo;

    float showTime = 0;
    float changeShowTime = 0.5f;
    bool showing;
    
    float hideTime = 0;
    float changeHideTime = 0.5f;
    bool hiding;

    void Update()
    {
        if (showing)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || showTime / changeShowTime >= 0.95f)
            {
                showTime = 0;
                iGo.color = new Color(iGo.color.r, iGo.color.g, iGo.color.b, 1);
                showing = false;
            }
            else
            {
                iGo.color = new Color(iGo.color.r, iGo.color.g, iGo.color.b, showTime / changeShowTime);
            }
        }
        else if(hiding)
        {
            hideTime += Time.deltaTime;
            if (hideTime >= changeHideTime || hideTime / changeHideTime >= 0.95f)
            {
                hideTime = 0;
                iGo.color = new Color(iGo.color.r, iGo.color.g, iGo.color.b, 0);
                hiding = false;
                gameObject.SetActive(false);
            }
            else
            {
                iGo.color = new Color(iGo.color.r, iGo.color.g, iGo.color.b, 1 - (hideTime / changeHideTime));
            }
        }
    }

    public void Show()
    {
        iGo.color = new Color(iGo.color.r, iGo.color.g, iGo.color.b, 0);
        showing = true;
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        hiding = true;
    }

    public void Black()
    {
        iGo.color = Color.black;
    }

    public void White()
    {
        iGo.color = Color.white;
    }
    
    public void Red()
    {
        iGo.color = Color.red;
    }
}