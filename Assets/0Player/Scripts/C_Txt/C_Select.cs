using UnityEngine;
using UnityEngine.EventSystems;

public class C_Select : MonoBehaviour, IPointerClickHandler
{
    public int i;
    public GameObject selectbuttonGo;

    bool t;
    float tTime = 0;
    float changeTTime = 1.5f;

    void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (t)
        {
            tTime += Time.deltaTime;
            if (tTime > changeTTime)
            {
                tTime = 0;
                t = false;
                gameObject.SetActive(false);
            }
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        selectbuttonGo.GetComponent<C_SelectButton>().Select(i);
    }

    public void Select()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        t = true;
    }
}