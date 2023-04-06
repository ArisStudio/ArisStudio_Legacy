using UnityEngine;

public class C_BgShake : MonoBehaviour
{
    //Shake
    float oXPosition, oYPosition;
    float shakeTimeX = 0;
    float shakeTimeY = 0;
    float shakeXA, shakeYA;
    float shakeXH, shakeYH;
    float shakeXT, shakeYT;
    bool xShaking, yShaking;

    // Update is called once per frame
    void Update()
    {
        if (xShaking)
        {
            shakeTimeX += Time.deltaTime;
            if (shakeTimeX * shakeXA > shakeXT)
            {
                transform.localPosition = new Vector3(oXPosition, 0, 0);
                shakeXA = 0;
                shakeXH = 0;
                shakeTimeX = 0;
                xShaking = false;
            }
            else
            {
                transform.localPosition =
                    new Vector3(Mathf.Sin(shakeTimeX * Mathf.PI * shakeXA) * shakeXH + oXPosition, 0, 0);
            }
        }

        if (yShaking)
        {
            shakeTimeY += Time.deltaTime;
            if (shakeTimeY * shakeYA > shakeYT)
            {
                transform.localPosition = new Vector3(0, oYPosition, 0);
                shakeYA = 0;
                shakeYH = 0;
                shakeTimeY = 0;
                yShaking = false;
            }
            else
            {
                transform.localPosition =
                    new Vector3(0, Mathf.Sin(shakeTimeY * Mathf.PI * shakeYA) * shakeYH + oYPosition, 0);
            }
        }
    }

    public void ShakeX(string xa, string xh, string xt)
    {
        shakeXA = float.Parse(xa);
        shakeXH = float.Parse(xh);
        shakeXT = float.Parse(xt);
        oXPosition = transform.localPosition.x;
        xShaking = true;
    }

    public void ShakeY(string ya, string yh, string yt)
    {
        shakeYA = float.Parse(ya);
        shakeYH = float.Parse(yh);
        shakeYT = float.Parse(yt);
        oYPosition = transform.localPosition.y;
        yShaking = true;
    }
}