using UnityEngine;

namespace ArisStudio.Spr
{
    public class OldSprEmotion : MonoBehaviour
    {
        private GameObject emo;

        private void Start()
        {
            emo = transform.Find("Emotion").gameObject;
        }

        public void PlayEmoticon(string emoticon)
        {
            emo.GetComponent<OldEmoticonScript>().EmoPlay(emoticon);
        }
    }
}