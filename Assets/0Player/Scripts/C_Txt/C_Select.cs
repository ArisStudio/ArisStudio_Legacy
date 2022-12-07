using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class C_Select : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public int i;
    public GameObject selectbuttonGo;

    void Start()
    {
        
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectbuttonGo.GetComponent<C_SelectButton>().Select(i);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
