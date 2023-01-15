using UnityEngine;

namespace ArisStudio.Spr
{
    public class OldSprEmotion : MonoBehaviour
    {
        private GameObject emo;

        public void InitEmoticon()
        {
            emo = transform.Find("Emotion").gameObject;
            emo.transform.position = new Vector3(-3.5f, 16, -1);
        }

        public void PlayEmoticon(string emoticon)
        {
            emo.GetComponent<OldEmoticonScript>().EmoPlay(emoticon);
        }
    }
}