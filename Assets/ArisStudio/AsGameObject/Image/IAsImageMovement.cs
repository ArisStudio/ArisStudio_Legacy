namespace ArisStudio.AsGameObject.Image
{
    public interface IAsImageMovement
    {
        void X(float x);

        void Y(float y);

        void Z(float z);

        void Position(float x, float y);

        void MoveX(float x, float time);

        void MoveY(float y, float time);

        void MovePosition(float x, float y, float time);

        void Shake(float strength, float time, int vibrato);

        void ShakeX(float strength, float time, int vibrato);

        void ShakeY(float strength, float time, int vibrato);

        void Scale(float scale, float time);
    }
}
