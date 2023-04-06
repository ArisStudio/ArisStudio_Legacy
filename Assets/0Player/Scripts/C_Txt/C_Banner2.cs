using UnityEngine;
using UnityEngine.UI;

public class C_Banner2 : MonoBehaviour
{
    public Text t1, t2;

    public void SetBanner2Text(string s1, string s2)
    {
        t1.text = s1;
        t2.text = s2;
        gameObject.SetActive(true);
    }
}