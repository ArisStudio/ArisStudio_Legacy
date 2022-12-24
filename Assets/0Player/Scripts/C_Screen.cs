using UnityEngine;

public class C_Screen : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void Set1280x720()
    {
        Screen.SetResolution(1280, 720, Screen.fullScreen);
    }

    public void Set1920x1080()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    public void Set2k()
    {
        Screen.SetResolution(2560, 1440, Screen.fullScreen);
    }

    public void Set4k()
    {
        Screen.SetResolution(3840, 2160, Screen.fullScreen);
    }
}
