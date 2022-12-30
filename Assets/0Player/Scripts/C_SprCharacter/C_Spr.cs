using Spine.Unity;
using UnityEngine;

public class C_Spr : MonoBehaviour
{
    Shader def, comm;

    SkeletonAnimation sa;
    MaterialPropertyBlock mpb;
    MeshRenderer md;

    //EyeClose
    bool isEyeClose = false;
    float closeTimer = 0;
    float closeInterval = 5;
    string eyeCloseName;

    //Show
    float showTime = 0;
    float changeShowTime = 0.4f;

    bool showing, hiding;

    // Update is called once per frame
    void Update()
    {
        if (isEyeClose)
        {
            closeTimer += Time.deltaTime;
            if (closeTimer >= closeInterval)
            {
                sa.AnimationState.AddAnimation(1, eyeCloseName, false, 0);
                closeTimer = 0;
            }
        }

        if (showing)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || 1 - showTime / changeShowTime <= 0.05f)
            {
                showTime = 0;
                mpb.SetFloat("_FillPhase", 0);
                md.SetPropertyBlock(mpb);
                showing = false;
            }
            else
            {
                mpb.SetFloat("_FillPhase", 1 - showTime / changeShowTime);
                md.SetPropertyBlock(mpb);
            }
        }

        else if (hiding)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || showTime / changeShowTime >= 0.95f)
            {
                showTime = 0;
                mpb.SetFloat("_FillPhase", 1);
                md.SetPropertyBlock(mpb);
                hiding = false;
                sa.gameObject.SetActive(false);
            }
            else
            {
                mpb.SetFloat("_FillPhase", showTime / changeShowTime);
                md.SetPropertyBlock(mpb);
            }
        }
    }

    public void Init()
    {
        sa = GetComponent<SkeletonAnimation>();
        md = GetComponent<MeshRenderer>();

        def = Shader.Find("SFill");
        comm = Shader.Find("Comm");
    }

    public void SetState(string s)
    {
        if (s.EndsWith("01"))
        {
            foreach (Spine.Animation a in sa.skeleton.Data.Animations)
            {
                if (a.Name.EndsWith("lose_" + s))
                {
                    eyeCloseName = a.Name;
                    isEyeClose = true;
                }
            }
        }
        else
        {
            isEyeClose = false;
            closeTimer = 0;
        }

        sa.AnimationState.SetAnimation(1, s, true);
    }

    public void Show()
    {
        mpb = new MaterialPropertyBlock();
        showing = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        mpb = new MaterialPropertyBlock();
        hiding = true;
    }

    public void HighLight(string f)
    {
        mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_FillPhase", 1 - float.Parse(f));
        md.SetPropertyBlock(mpb);
    }

    public void ShowC()
    {
        Comm();
        gameObject.SetActive(true);
    }

    public void HideC()
    {
        Def();
        gameObject.SetActive(false);
    }

    public void Def()
    {
        md.material.shader = def;
    }

    public void Comm()
    {
        md.material.shader = comm;
    }
}