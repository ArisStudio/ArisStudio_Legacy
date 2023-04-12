using UnityEngine;

namespace ArisStudio.Audio
{
    public interface IAsAudio
    {
        void SetAudio(AudioClip ac);

        void Play();

        void Pause();

        void Stop();

        void SetVolume(float volume);

        void Fade(float volume, float time);

        void Loop();

        void Once();
    }
}
