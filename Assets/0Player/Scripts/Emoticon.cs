using UnityEngine;

public class Emoticon : MonoBehaviour
{
    GameObject gm;
    Animator anim;
    string animName;
    bool isAnimPlay;

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
        isAnimPlay = true;
    }

    void Update()
    {
        if (isAnimPlay)
        {
            AnimatorStateInfo animTmp = anim.GetCurrentAnimatorStateInfo(0);
            if (animTmp.normalizedTime > 0.99f && animTmp.IsName(animName))
            {
                gm.SetActive(false);
                isAnimPlay = false;
            }
        }
    }
}
