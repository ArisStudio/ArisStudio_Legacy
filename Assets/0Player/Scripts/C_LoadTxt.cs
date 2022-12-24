using UnityEngine;
using UnityEngine.UI;

public class C_LoadTxt : MonoBehaviour
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

    public void LoadTxt()
    {
        StartCoroutine(control.GetComponent<C_Control>().LoadTxt(input.text));
        input.text = string.Empty;
    }
}
