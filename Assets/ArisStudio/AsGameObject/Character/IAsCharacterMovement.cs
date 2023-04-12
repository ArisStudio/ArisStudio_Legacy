namespace ArisStudio.AsGameObject.Character
{
    public interface IAsCharacterMovement
    {
        void X(float x);

        void Y(float y);

        void Z(float z);

        void Position(float x, float y);

        void MoveX(float x, float time);

        void MoveY(float y, float time);

        void MovePosition(float x, float y, float time);

        void Shake(float strength, float time);

        void ShakeX(float strength, float time);

        void ShakeY(float strength, float time);

        void Scale(float scale);

        void Close();

        void Back();
    }
}
