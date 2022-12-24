using UnityEngine;
using UnityEngine.UI;

public class C_SetWebPatn : MonoBehaviour
{
    public InputField input;
    public GameObject control;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWebPathBtn()
    {
        control.GetComponent<C_Control>().SetWebPath(input.text);
        input.text = string.Empty;
    }
}
