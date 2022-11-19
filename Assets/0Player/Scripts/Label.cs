using UnityEngine;
using UnityEngine.UI;

public class Label : MonoBehaviour
{
    public Text labelTxt;
    bool lr;
    float t;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetLabelTxt(string text)
    {
        t = 0;
        labelTxt.text = text;
        lr = true;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (lr)
        {
            t += Time.deltaTime;
            if (t >= 2)
            {
                t = 0; lr = false;
                gameObject.SetActive(false);
            }
        }
    }
}
