using UnityEngine;
using UnityEngine.UI;

public class C_Label : MonoBehaviour
{
    public Text labelText;
    bool showing;
    float t;

    // Update is called once per frame
    void Update()
    {
        if (showing)
        {
            t += Time.deltaTime;
            if (t >= 2)
            {
                t = 0;
                showing = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void SetLabelText(string text)
    {
        t = 0;
        labelText.text = text;
        showing = true;
        gameObject.SetActive(true);
    }
}