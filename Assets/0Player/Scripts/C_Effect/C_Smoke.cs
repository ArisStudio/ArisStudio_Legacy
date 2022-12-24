using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Smoke : MonoBehaviour
{
    public GameObject smokeF;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
