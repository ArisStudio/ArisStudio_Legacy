using UnityEngine;

namespace ArisStudio.AsGameObject.Image
{
    public class AsImage : MonoBehaviour, IAsImageState, IAsImageMovement
    {
        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Appear()
        {
            gameObject.SetActive(true);
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
        }

        public void Highlight(float hl)
        {
            throw new System.NotImplementedException();
        }

        public void Highlight(float hl, float time)
        {
            throw new System.NotImplementedException();
        }

        public void Fade(float alpha)
        {
            throw new System.NotImplementedException();
        }

        public void Fade(float alpha, float time)
        {
            throw new System.NotImplementedException();
        }

        public void X(float x)
        {
            throw new System.NotImplementedException();
        }

        public void Y(float y)
        {
            throw new System.NotImplementedException();
        }

        public void Z(float z)
        {
            throw new System.NotImplementedException();
        }

        public void Position(float x, float y)
        {
            throw new System.NotImplementedException();
        }

        public void MoveX(float x, float time)
        {
            throw new System.NotImplementedException();
        }

        public void MoveY(float y, float time)
        {
            throw new System.NotImplementedException();
        }

        public void MovePosition(float x, float y, float time)
        {
            throw new System.NotImplementedException();
        }

        public void Shake(float strength, float time)
        {
            throw new System.NotImplementedException();
        }

        public void ShakeX(float strength, float time)
        {
            throw new System.NotImplementedException();
        }

        public void ShakeY(float strength, float time)
        {
            throw new System.NotImplementedException();
        }

        public void Scale(float scale)
        {
            throw new System.NotImplementedException();
        }

        public void Scale(float scale, float time)
        {
            throw new System.NotImplementedException();
        }
    }
}
