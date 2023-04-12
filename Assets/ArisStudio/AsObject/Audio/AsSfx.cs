using DG.Tweening;
using UnityEngine;

namespace ArisStudio.Audio
{
    public class AsSfx : MonoBehaviour, IAsAudio
    {
        private AudioSource sfx;

        private const float FadeDuration = 1f;

        private void Start()
        {
            sfx = GetComponent<AudioSource>();
        }

        public void SetAudio(AudioClip ac)
        {
            sfx.clip = ac;
            sfx.loop = false;
            sfx.Play();
        }

        public void Play()
        {
            sfx.Play();
        }

        public void Pause()
        {
            sfx.Pause();
        }

        public void Stop()
        {
            sfx.Stop();
        }

        public void SetVolume(float volume)
        {
            sfx.volume = volume;
        }

        public void Fade(float volume = 0f, float time = FadeDuration)
        {
            sfx.DOFade(volume, time);
        }

        public void Loop()
        {
            sfx.loop = true;
        }

        public void Once()
        {
            sfx.loop = false;
        }
    }
}
