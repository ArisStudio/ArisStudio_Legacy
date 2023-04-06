using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.Sound
{
    public class Bgm : MonoBehaviour
    {
        public Slider vSlider;
        public Text vText;

        private AudioSource bgm;

        private bool down;
        private float downTime;
        private const float ChangeDownTime = 1;
        private float ov;

        private void Start()
        {
            bgm = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!down) return;
            downTime += Time.deltaTime;

            if (downTime >= ChangeDownTime || downTime / ChangeDownTime >= 0.95f)
            {
                downTime = 0;
                bgm.volume = 0;
                down = false;
            }
            else
            {
                bgm.volume = downTime / ChangeDownTime * ov;
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

        public void Loop()
        {
            bgm.loop = true;
        }

        public void Once()
        {
            bgm.loop = false;
        }

        public void SetVolume(float v)
        {
            vSlider.value = v * 10;
            vText.text = $"{v:F1} v";
            bgm.volume = v;
        }

        public void ChangeVolume()
        {
            var vTmp = vSlider.value / 10;
            vText.text = $"{vTmp:F1} v";
            bgm.volume = vTmp;
        }

        public void Down()
        {
            ov = bgm.volume;
            down = true;
        }
    }
}