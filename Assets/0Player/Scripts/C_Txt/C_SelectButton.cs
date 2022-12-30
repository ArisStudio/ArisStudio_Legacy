using UnityEngine;
using UnityEngine.UI;

public class C_SelectButton : MonoBehaviour
{
    public Button s1, s2, s3;
    public Text txt1, txt2, txt3;
    public GameObject control;
    public AudioSource bas;

    string t1, t2, t3;
    bool selecting;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Select(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Select(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Select(3);
        }
    }

    public void Select(int n)
    {
        if (selecting)
        {
            bas.Play();

            if (n == 1)
            {
                control.GetComponent<C_Control>().SetSelecting(t1);
            }
            else if (n == 2)
            {
                control.GetComponent<C_Control>().SetSelecting(t2);
            }
            else if (n == 3)
            {
                control.GetComponent<C_Control>().SetSelecting(t3);
            }

            s1.gameObject.SetActive(false);
            s2.gameObject.SetActive(false);
            s3.gameObject.SetActive(false);

            selecting = false;
        }
    }

    public void SetSelectButton(string lt)
    {
        string[] l = lt.Split('\'');
        if (l.Length == 5)
        {
            txt2.text = l[1];
            t2 = l[3];
            s2.gameObject.SetActive(true);
        }
        else if (l.Length == 9)
        {
            txt1.text = l[1];
            txt2.text = l[5];
            t1 = l[3];
            t2 = l[7];
            s1.gameObject.SetActive(true);
            s2.gameObject.SetActive(true);
        }
        else if (l.Length == 13)
        {
            txt1.text = l[1];
            txt2.text = l[5];
            txt3.text = l[9];
            t1 = l[3];
            t2 = l[7];
            t3 = l[11];
            s1.gameObject.SetActive(true);
            s2.gameObject.SetActive(true);
            s3.gameObject.SetActive(true);
        }

        selecting = true;
    }
}