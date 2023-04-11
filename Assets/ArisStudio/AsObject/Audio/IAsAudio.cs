using UnityEngine;

namespace ArisStudio.Audio
{
    public interface IAsAudio
    {
        void SetAudio(AudioClip ac);

        void Play();

        void Pause();

        void Stop();

        void SetVolume(float v);

        void Loop();

        void Once();
    }
}
