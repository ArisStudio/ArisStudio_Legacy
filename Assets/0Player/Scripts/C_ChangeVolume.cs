using UnityEngine;
using UnityEngine.UI;

public class C_ChangeVolume : MonoBehaviour
{
    public AudioSource sound;
    public Slider slider;
    public Text v;

    public void setVolume()
    {
        float tmpV = slider.value / 10;
        sound.volume = tmpV;
        v.text = tmpV.ToString();
    }
}