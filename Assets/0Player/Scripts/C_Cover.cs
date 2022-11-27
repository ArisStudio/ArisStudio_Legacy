using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Cover : MonoBehaviour
{
    public RawImage coverGo;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCover(Texture2D cover)
    {
        coverGo.texture = cover;
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
