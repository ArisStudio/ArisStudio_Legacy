using System;
using DG.Tweening;
using UnityEngine;

namespace ArisStudio.Audio
{
    public class AsBgm : MonoBehaviour, IAsAudio
    {
        private AudioSource bgm;

        private const float FadeDuration = 1f;

        private void Start()
        {
            bgm = GetComponent<AudioSource>();
        }

        public void SetAudio(AudioClip ac)
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

        public void SetVolume(float volume)
        {
            bgm.volume = volume;
        }

        public void Fade(float volume = 0f, float time = FadeDuration)
        {
            bgm.DOFade(volume, time);
        }

        public void Loop()
        {
            bgm.loop = true;
        }

        public void Once()
        {
            bgm.loop = false;
        }
    }
}
