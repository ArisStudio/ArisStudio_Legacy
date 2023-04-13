using DG.Tweening;
using UnityEngine;

namespace ArisStudio.AsGameObject.Audio
{
    public class AsAudio : MonoBehaviour, IAsAudio
    {
        private AudioSource audioSource;

        public static AsAudio GetAsAudio(GameObject go)
        {
            var asAudio = go.GetComponent<AsAudio>();
            if (asAudio == null)
            {
                asAudio = go.AddComponent<AsAudio>();
            }

            return asAudio;
        }

        public void AsAudioInit(AudioClip ac, bool loop)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = ac;
            audioSource.loop = loop;
            audioSource.Stop();
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void Pause()
        {
            audioSource.Pause();
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void Fade(float volume, float time)
        {
            audioSource.DOFade(volume, time);
        }

        public void Loop()
        {
            audioSource.loop = true;
        }

        public void Once()
        {
            audioSource.loop = false;
        }
    }
}
