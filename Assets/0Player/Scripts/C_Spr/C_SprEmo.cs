using UnityEngine;

public class C_SprEmo : MonoBehaviour
{
    GameObject emo;

    void Start()
    {
        emo = transform.Find("Emotion").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEmoticon(string emoticon)
    {
        emo.GetComponent<C_Emoticon>().EmoPlay(emoticon);
    }
}
