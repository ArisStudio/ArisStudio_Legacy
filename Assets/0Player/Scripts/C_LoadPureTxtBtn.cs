using UnityEngine;
using UnityEngine.UI;

public class C_LoadPureTxtBtn : MonoBehaviour
{
    public InputField input;
    public GameObject control;

    public void LoadPureTxtBtn()
    {
        control.GetComponent<C_Control>().LoadPureTxt(input.text);
    }
}