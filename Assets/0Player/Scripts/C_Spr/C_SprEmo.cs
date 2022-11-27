using UnityEngine;

public class C_SprEmo : MonoBehaviour
{
    public GameObject emo;

    void Start()
    {
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
