using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Curtain : MonoBehaviour
{
    public Image iGo;

    float showTime = 0;
    float changeShowTime = 0.5f;
    bool showing;

    void Start()
    {
    }

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
    }

    public void Show()
    {
        iGo.color = new Color(iGo.color.r, iGo.color.g, iGo.color.b, 0);
        showing = true;
        gameObject.SetActive(true);
    }

    public void Black()
    {
        iGo.color = Color.black;
    }

    public void White()
    {
        iGo.color = Color.white;
    }
}
