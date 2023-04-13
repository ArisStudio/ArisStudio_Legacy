namespace ArisStudio.AsGameObject.Audio
{
    public interface IAsAudio
    {
        void Play();

        void Pause();

        void Stop();

        void SetVolume(float volume);

        void Fade(float volume, float time);

        void Loop();

        void Once();
    }
}
