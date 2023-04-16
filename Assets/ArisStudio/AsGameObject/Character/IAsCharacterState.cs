namespace ArisStudio.AsGameObject.Character
{
    public interface IAsCharacterState
    {
        void Show();

        void Hide();

        void Appear();

        void Disappear();

        void Highlight(float hl, float time);

        void Fade(float alpha, float time);

        void State(string stateName, int trackIndex, bool loop);

        void Skin(string skinName);

        void Emotion(string emotionName);

        void Animation(string animationName);
    }
}
