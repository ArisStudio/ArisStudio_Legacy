using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour, IPointerClickHandler
{
    public Button playBtn;
    public Text txt;

    int lineN=0;
    void Start()
    {
        
    }

    public void SetButton(string t,string n)
    {
        txt.text = t;
        lineN=int.Parse(n);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        playBtn.GetComponent<Play>().SetLine(lineN);
    }
}
