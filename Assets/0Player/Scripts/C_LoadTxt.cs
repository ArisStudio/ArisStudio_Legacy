using UnityEngine;
using UnityEngine.UI;

public class C_LoadTxt : MonoBehaviour
{
    public InputField input;
    public GameObject control;

    public void LoadTxt()
    {
        StartCoroutine(control.GetComponent<C_Control>().LoadTxt(input.text));
        input.text = string.Empty;
    }
}