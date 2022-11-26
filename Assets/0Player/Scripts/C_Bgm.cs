using UnityEngine;

public class C_Bgm : MonoBehaviour
{
    AudioSource bgm;

    void Start()
    {
        bgm = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void SetBgm(AudioClip ac)
    {
        bgm.clip = ac;
        bgm.loop = true;
        bgm.Play();
    }

    public void Play()
    {
        bgm.Play();
    }

    public void Pause()
    {
        bgm.Pause();
    }

    public void Stop()
    {
        bgm.Stop();
    }
}
