using UnityEngine;

public class C_Smoke : MonoBehaviour
{
    public GameObject smokeF;

    public void Show()
    {
        gameObject.SetActive(true);
        smokeF.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        smokeF.SetActive(false);
    }
}