namespace ArisStudio.AsGameObject.Image
{
    public interface IAsImageState
    {
        void Show();

        void Hide();

        void Appear();

        void Disappear();

        void Highlight(float hl, float time);

        void Fade(float alpha, float time);
    }
}
