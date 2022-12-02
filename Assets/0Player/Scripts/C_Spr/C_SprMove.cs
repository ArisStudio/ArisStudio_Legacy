using Unity.Mathematics;
using UnityEngine;

public class C_SprMove : MonoBehaviour
{
    public GameObject sprBase;
    //Move
    int moveX;
    float moveSpeed;
    bool moving;

    //Shake
    float oXPosition, oYPosition;
    float shakeTimeX = 0;
    float shakeTimeY = 0;
    float shakeXA, shakeYA;
    float shakeXH, shakeYH;
    float shakeXT, shakeYT;
    bool xShaking, yShaking;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (math.abs(moveX - sprBase.transform.localPosition.x) <= 0.1f)
            {
                sprBase.transform.localPosition = new Vector3(moveX, sprBase.transform.localPosition.y, sprBase.transform.localPosition.z);
                moving = false;
            }
            else
            {
                sprBase.transform.localPosition = Vector3.MoveTowards(sprBase.transform.localPosition, new Vector3(moveX, sprBase.transform.localPosition.y, sprBase.transform.localPosition.z), moveSpeed * Time.deltaTime);
            }
        }

        if (xShaking)
        {
            shakeTimeX += Time.deltaTime;
            if (shakeTimeX * shakeXA > shakeXT)
            {
                sprBase.transform.localPosition = new Vector3(oXPosition, sprBase.transform.localPosition.y, sprBase.transform.localPosition.z);
                shakeXA = 0; shakeXH = 0; shakeTimeX = 0;
                xShaking = false;
            }
            else
            {
                sprBase.transform.localPosition = new Vector3(Mathf.Sin(shakeTimeX * Mathf.PI * shakeXA) * shakeXH + oXPosition, sprBase.transform.localPosition.y, sprBase.transform.localPosition.z);
            }
        }

        if (yShaking)
        {
            shakeTimeY += Time.deltaTime;
            if (shakeTimeY * shakeYA > shakeYT)
            {
                sprBase.transform.localPosition = new Vector3(sprBase.transform.localPosition.x, oYPosition, sprBase.transform.localPosition.z);
                shakeYA = 0; shakeYH = 0; shakeTimeY = 0;
                yShaking = false;
            }
            else
            {
                sprBase.transform.localPosition = new Vector3(sprBase.transform.localPosition.x, Mathf.Sin(shakeTimeY * Mathf.PI * shakeYA) * shakeYH + oYPosition, sprBase.transform.localPosition.z);
            }
        }
    }

    public void SetX(string x)
    {
        sprBase.transform.localPosition = new Vector3(int.Parse(x), 0, 0);
    }

    public void SetZ(string z)
    {
        sprBase.transform.localPosition = new Vector3(0, 0, -int.Parse(z));
    }

    public void Move(string x, string speed)
    {
        moveX = int.Parse(x);
        moveSpeed = float.Parse(speed);
        moving = true;
    }

    public void Close()
    {
        sprBase.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        sprBase.transform.localPosition = new Vector3(sprBase.transform.localPosition.x, -7, 0);
    }

    public void Back()
    {
        sprBase.transform.localScale = Vector3.one;
        sprBase.transform.localPosition = new Vector3(sprBase.transform.localPosition.x, 0, 0);
    }

    public void ShakeX(string xa, string xh, string xt)
    {
        shakeXA = float.Parse(xa); shakeXH = float.Parse(xh) * 0.2f; shakeXT = float.Parse(xt);
        oXPosition = sprBase.transform.localPosition.x;
        xShaking = true;
    }

    public void ShakeY(string ya, string yh, string yt)
    {
        shakeYA = float.Parse(ya); shakeYH = float.Parse(yh) * 0.4f; shakeYT = float.Parse(yt);
        oYPosition = sprBase.transform.localPosition.y;
        yShaking = true;
    }
}
