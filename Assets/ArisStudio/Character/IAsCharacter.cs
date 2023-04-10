namespace ArisStudio.Character
{
    public interface IAsCharacter
    {
        void Show();

        void Hide();

        void Appear();

        void Disappear();

        void Highlight(float hl);
        void Highlight(float hl, float time);

        void Position(float x, float y);

        void Move();

        void Z(float z);

        void Shake();

        void Scale(float scale);

        void Close();

        void Back();

        void State();

        void Emotion();

        void Animation();
    }
}
