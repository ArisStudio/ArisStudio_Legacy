using UnityEngine;

public class C_Emoticon : MonoBehaviour
{
    GameObject gm;
    Animator anim;
    bool playing;
    string animName;

    void Start()
    {
        
    }

    public void EmoPlay(string emoticon)
    {
        if (gm != null && gm.activeSelf)
        {
            gm.SetActive(false);
        }
        animName = emoticon;
        gm = transform.Find(emoticon).gameObject;
        anim = gm.GetComponent<Animator>();
        gm.SetActive(true);
        playing = true;
    }

    void Update()
    {
        if (playing)
        {
            AnimatorStateInfo animTmp = anim.GetCurrentAnimatorStateInfo(0);
            if (animTmp.normalizedTime > 0.99f && animTmp.IsName(animName))
            {
                gm.SetActive(false);
                playing = false;
            }
        }
    }
}
