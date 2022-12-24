using UnityEngine;
using UnityEngine.EventSystems;

public class C_PlayButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject txtGo;

    GameObject control;
    void Start()
    {
        control = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space))
        {
            Click();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void Click()
    {
        if (txtGo.GetComponent<C_Text>().GetTypingState())
        {
            txtGo.GetComponent<C_Text>().PlayTxtAll();
        }
        else
        {
            control.GetComponent<C_Control>().SetClick(true);
        }
    }
}
