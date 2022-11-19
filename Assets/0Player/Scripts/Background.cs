using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    RawImage ri;
    bool isShow, endShow;
    float showTime = 0;
    float changeShowTime = 0.5f;


    void Start()
    {

    }

    void OnEnable()
    {
        ri = gameObject.GetComponent<RawImage>();
        ri.color = new Color(1, 1, 1, 0);
        isShow = true;
        endShow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow && !endShow)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || showTime / changeShowTime >= 0.95f)
            {
                showTime = 0;
                ri.color = new Color(1, 1, 1, 1);
                endShow = true;
            }
            else
            {
                ri.color = new Color(1, 1, 1, showTime / changeShowTime);
            }
        }
    }
}
