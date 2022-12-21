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

    public void InitEmoticon(string scale)
    {
        emo.transform.position = new Vector3(-3.5f, 14, -1);
        emo.transform.localScale = new Vector3(transform.localScale.x / float.Parse(scale), transform.localScale.y / float.Parse(scale), 1);
    }

    public void PlayEmoticon(string emoticon)
    {
        emo.GetComponent<C_Emoticon>().EmoPlay(emoticon);
    }
}
