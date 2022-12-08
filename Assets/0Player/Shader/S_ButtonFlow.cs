using UnityEngine;
using UnityEngine.UI;

public class S_ButtonFlow : MonoBehaviour
{
    public Image i;

    float timer = 0;
    float s = 2.5f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer / s >= 1)
        {
            i.material.SetFloat("_FlowPos",0);
            timer= 0;
        }
        else
        {
            i.material.SetFloat("_FlowPos", timer/s);
        }
    }
}
