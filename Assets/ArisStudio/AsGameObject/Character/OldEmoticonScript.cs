using UnityEngine;

namespace ArisStudio.Spr
{
    public class OldEmoticonScript : MonoBehaviour
    {
        private GameObject gm;
        private Animator anim;
        private bool playing;
        private string animName;

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

        private void Update()
        {
            if (!playing) return;
            var animTmp = anim.GetCurrentAnimatorStateInfo(0);

            if (!(animTmp.normalizedTime > 0.99f) || !animTmp.IsName(animName)) return;
            gm.SetActive(false);
            playing = false;
        }
    }
}