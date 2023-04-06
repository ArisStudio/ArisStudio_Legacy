using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.Sound
{
    public class SoundEffect : MonoBehaviour
    {
        public Slider vSlider;
        public Text vText;

        private AudioSource se;

        private bool down;
        private float downTime;
        private const float ChangeDownTime = 1;
        private float ov;

        private void Start()
        {
            se = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!down) return;
            downTime += Time.deltaTime;

            if (downTime >= ChangeDownTime || downTime / ChangeDownTime >= 0.95f)
            {
                downTime = 0;
                se.volume = 0;
                down = false;
            }
            else
            {
                se.volume = downTime / ChangeDownTime * ov;
            }
        }

        public void SetSoundEffect(AudioClip ac)
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

        public void Loop()
        {
            se.loop = true;
        }

        public void Once()
        {
            se.loop = false;
        }

        public void SetVolume(float v)
        {
            vSlider.value = v * 10;
            vText.text = $"{v:F1} v";
            se.volume = v;
        }

        public void ChangeVolume()
        {
            var vTmp = vSlider.value / 10;
            vText.text = $"{vTmp:F1} v";
            se.volume = vTmp;
        }

        public void Down()
        {
            ov = se.volume;
            down = true;
        }
    }
}