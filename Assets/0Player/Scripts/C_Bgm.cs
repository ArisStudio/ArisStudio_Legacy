using UnityEngine;

public class C_Bgm : MonoBehaviour
{
    public AudioSource bgm;

    bool down;
    float downTime = 0;
    float changeDownTime = 0.5f;
    float ov;

    void Start()
    {
    }

    void Update()
    {
        if (down)
        {
            downTime += Time.deltaTime;
            if (downTime >= changeDownTime || downTime / changeDownTime >= 0.95f)
            {
                downTime = 0;
                bgm.volume = 0;
                down = false;
            }
            else
            {
                bgm.volume = downTime / changeDownTime * ov;
            }
        }
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

    public void V(string v)
    {
        bgm.volume=float.Parse(v);
    }

    public void Down()
    {
        ov = bgm.volume;
        down=true;
    }
}
