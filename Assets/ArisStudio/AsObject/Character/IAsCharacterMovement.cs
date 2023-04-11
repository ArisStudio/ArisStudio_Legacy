namespace ArisStudio.Character
{
    public interface IAsCharacterMovement
    {
        void X(float x);

        void Y(float y);

        void Z(float z);

        void Position(float x, float y);

        void MoveX(float x);

        void MoveX(float x, float time);

        void MoveY(float y);

        void MoveY(float y, float time);

        void MovePosition(float x, float y);

        void MovePosition(float x, float y, float time);

        void ShakeX();

        void ShakeY();

        void Scale(float scale);

        void Close();

        void Back();
    }
}
