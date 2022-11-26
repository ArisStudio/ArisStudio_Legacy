using UnityEngine;

public class C_SE : MonoBehaviour
{
    AudioSource se;

    void Start()
    {
        se = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void SetSE(AudioClip ac)
    {
        se.clip = ac;
        se.loop = false;
        se.Play();
    }

    public void Play()
    {
        se.Play();
    }

    public void Pause()
    {
        se.Pause();
    }

    public void Stop()
    {
        se.Stop();
    }

    public void PlayPre(string ac)
    {
        transform.Find(ac).gameObject.GetComponent<AudioSource>().Play();
    }
}
