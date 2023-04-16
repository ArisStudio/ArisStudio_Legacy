using DG.Tweening;
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


        public void Highlight(float hl, float time)
        {
            throw new System.NotImplementedException();
        }

        public void Fade(float alpha, float time)
        {
            throw new System.NotImplementedException();
        }

        public void X(float x)
        {
            transform.localPosition += Vector3.right * x;
        }

        public void Y(float y)
        {
            transform.localPosition += Vector3.up * y;
        }

        public void Z(float z)
        {
            transform.localPosition += Vector3.forward * z;
        }

        public void Position(float x, float y)
        {
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        }

        public void MoveX(float x, float time)
        {
            transform.DOMoveX(x, time);
        }

        public void MoveY(float y, float time)
        {
            transform.DOMoveY(y, time);
        }

        public void MovePosition(float x, float y, float time)
        {
            transform.DOMove(new Vector3(x, y, transform.localPosition.z), time);
        }

        public void Shake(float strength, float time)
        {
            transform.DOShakePosition(time, strength);
        }

        public void ShakeX(float strength, float time)
        {
            transform.DOShakePosition(time, Vector3.right * strength);
        }

        public void ShakeY(float strength, float time)
        {
            transform.DOShakePosition(time, Vector3.down * strength);
        }

        public void Scale(float scale, float time)
        {
            transform.DOScale(new Vector3(scale, scale, 1), time);
        }
    }
}
