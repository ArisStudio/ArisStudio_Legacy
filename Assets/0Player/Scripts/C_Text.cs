using UnityEngine;
using UnityEngine.UI;
using RichText;

public class C_Text : MonoBehaviour
{
    public GameObject control;
    public Text nameTxt, groupTxt, contentTxt;

    string tmpContent;

    int currentPos = 0;
    float timer;
    bool typing;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(typing)
        {
            timer += Time.deltaTime;
            if (timer >= 0.02f)
            {
                timer = 0;
                currentPos++;
                contentTxt.text = tmpContent.RichTextSubString( currentPos);

                if (currentPos >= tmpContent.RichTextLength())
                {
                    PlayTxtAll();
                }
            }
        }
    }

    public void SetTxt(string name, string group, string content)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        timer = 0;
        nameTxt.text = name;
        groupTxt.text = group;
        contentTxt.text = "";
        tmpContent = content;
        typing = true;
    }

    public void SetTxtSize(string size)
    {
        contentTxt.text = "";
        if (size == "big")
        {
            contentTxt.fontSize = 48;
        }
        else if (size == "small")
        {
            contentTxt.fontSize = 24;
        }else if (size == "medium")
        {
            contentTxt.fontSize = 32;
        }
    }

    public void PlayTxtAll()
    {
        typing = false;
        currentPos = 0;
        contentTxt.text = tmpContent;
        control.GetComponent<C_Control>().SetTxtTyping(false);
    }

    public bool GetTypingState()
    {
        return typing;
    }
}
