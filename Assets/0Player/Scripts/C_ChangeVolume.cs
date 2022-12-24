using UnityEngine;
using UnityEngine.UI;

public class C_ChangeVolume : MonoBehaviour
{
    public AudioSource sound;
    public Slider slider;
    public Text v;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setVolume()
    {
        float tmpV = slider.value / 10;
        sound.volume = tmpV;
        v.text = tmpV.ToString();
    }
}
