using Spine.Unity;
using Unity.Mathematics;
using UnityEngine;

public class SprState : MonoBehaviour
{
    SkeletonAnimation sa;

    MaterialPropertyBlock mpb;
    MeshRenderer md;

    //Show
    float showTime = 0;
    float changeShowTime = 0.4f;

    //Move
    int movePosition;
    float moveSpeed = 24;

    bool isShow, endShow, isHide, endHide, isMove, endMove, isShakeX, isShakeY;

    //Shake
    Vector3 oXPosition, oYPosition, tmpXPosition, tmpYPosition;
    float shakeTimeX = 0;
    float shakeTimeY = 0;
    float shakeXA, shakeYA;
    float shakeXH, shakeYH;
    float shakeXT, shakeYT;



    void Start()
    {
        sa = GetComponent<SkeletonAnimation>();
        md = GetComponent<MeshRenderer>();
    }

    void OnEnable()
    {
        isShow = true; endShow = false;
        isHide = false;
        mpb = new MaterialPropertyBlock();
    }

    public void Close()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
        transform.localPosition = new Vector3(0, -16, 0);
    }

    public void Back()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0, -9, 0);
    }

    public void SetPosition(string x)
    {
        transform.localPosition = new Vector3(int.Parse(x), transform.localPosition.y, transform.localPosition.z);
    }

    public void Hide()
    {
        isHide = true; endHide = false;
        isShow = false;
        mpb = new MaterialPropertyBlock();
    }

    public void Move(string position)
    {
        isMove = true;
        endMove = false;
        movePosition = int.Parse(position);
    }
    public void PlayEmoticon(string emoticon)
    {
        GameObject emo = transform.Find("Emotion(Clone)").gameObject;
        emo.GetComponent<Emoticon>().EmoPlay(emoticon);
    }

    public void SetState(string s)
    {
        sa.AnimationState.SetAnimation(1, s, true);
    }

    public void HighLight(string f)
    {
        mpb.SetFloat("_FillPhase", float.Parse(f));
        md.SetPropertyBlock(mpb);
    }


    void Update()
    {
        if (isShow && !endShow)
        {
            SprShow();
        }
        else if (isHide && !endHide)
        {
            SprHide();
        }
        else if (isMove && !endMove)
        {
            SprMove();
        }
        else if (isShakeX)
        {
            SprShakeX();
        }
        else if (isShakeY)
        {
            SprShakeY();
        }
    }

    void SprShow()
    {
        showTime += Time.deltaTime;
        if (showTime >= changeShowTime || 1 - showTime / changeShowTime <= 0.05f)
        {
            showTime = 0;
            mpb.SetFloat("_FillPhase", 0);
            md.SetPropertyBlock(mpb);
            endShow = true;
        }
        else
        {
            mpb.SetFloat("_FillPhase", 1 - showTime / changeShowTime);
            md.SetPropertyBlock(mpb);
        }
    }

    void SprHide()
    {
        showTime += Time.deltaTime;
        if (showTime >= changeShowTime || showTime / changeShowTime >= 0.95f)
        {
            showTime = 0;
            mpb.SetFloat("_FillPhase", 1);
            md.SetPropertyBlock(mpb);
            endHide = true;
            sa.gameObject.SetActive(false);
        }
        else
        {
            mpb.SetFloat("_FillPhase", showTime / changeShowTime);
            md.SetPropertyBlock(mpb);
        }
    }

    void SprMove()
    {
        if (math.abs(movePosition - transform.localPosition.x) <= 0.1f)
        {
            transform.localPosition = new Vector3(movePosition, transform.localPosition.y, transform.localPosition.z);
            endMove = true;
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(movePosition, transform.localPosition.y, transform.localPosition.z), moveSpeed * Time.deltaTime);
        }
    }

    public void ShakeX(string xa, string xh, string xt)
    {
        shakeXA = float.Parse(xa); shakeXH = float.Parse(xh); shakeXT = float.Parse(xt);
        oXPosition = transform.position;
        isShakeX = true;
    }

    void SprShakeX()
    {
        shakeTimeX += Time.deltaTime;
        if (shakeTimeX * shakeXA > shakeXT)
        {
            transform.position = oXPosition;
            shakeXA = 0; shakeXH = 0; shakeTimeX = 0;
            isShakeX = false;
        }
        else
        {
            tmpXPosition = oXPosition;
            tmpXPosition.x = Mathf.Sin(shakeTimeX * Mathf.PI * shakeXA) * 0.2f * shakeXH + oXPosition.x;
            transform.position = tmpXPosition;
        }
    }

    public void ShakeY(string ya, string yh, string yt)
    {
        shakeYA = float.Parse(ya); shakeYH = float.Parse(yh); shakeYT = float.Parse(yt);
        oYPosition = transform.position;
        isShakeY = true;
    }

    void SprShakeY()
    {
        shakeTimeY += Time.deltaTime;
        if (shakeTimeY * shakeYA > shakeYT)
        {
            transform.position = oYPosition;
            shakeYA = 0; shakeYH = 0; shakeTimeY = 0;
            isShakeY = false;
        }
        else
        {
            tmpYPosition = oYPosition;
            tmpYPosition.y = Mathf.Sin(shakeTimeY * Mathf.PI * shakeYA) * 0.4f * shakeYH + oYPosition.y;
            transform.position = tmpYPosition;
        }
    }
}
