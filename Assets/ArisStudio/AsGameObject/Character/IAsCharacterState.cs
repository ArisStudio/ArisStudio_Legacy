namespace ArisStudio.AsGameObject.Character
{
    public interface IAsCharacterState
    {
        void Show();

        void Hide();

        void Appear();

        void Disappear();

        void Highlight(float hl);

        void Highlight(float hl, float time);

        void State(string stateName);

        void Skin(string skinName);

        void Emotion(string emotionName);

        void Animation(string animationName);
    }
}
