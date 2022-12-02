using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Smoke : MonoBehaviour
{
    public GameObject s3;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        s3.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        s3.SetActive(false);
    }
}
