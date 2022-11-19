using UnityEngine;
using UnityEngine.UI;

public class Txt : MonoBehaviour
{
    public Text nameTxt, groupTxt, contentTxt;

    string tmpContent;

    bool isActive = false;
    float timer;
    int currentPos = 0;


    void Start()
    {
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
        tmpContent = content;
        isActive = true;
    }

    public bool IsTxtActive() { return isActive; }

    public void PlayTxtAll()
    {
        isActive = false;
        currentPos = 0;
        contentTxt.text = tmpContent;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= 0.02f)
            {
                timer = 0;
                currentPos++;
                contentTxt.text = tmpContent.Substring(0, currentPos);

                if (currentPos >= tmpContent.Length)
                {
                    isActive = false;
                    currentPos = 0;
                    contentTxt.text = tmpContent;
                }
            }
        }
    }
}
