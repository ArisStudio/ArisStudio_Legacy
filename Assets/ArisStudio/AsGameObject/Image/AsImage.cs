using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.AsGameObject.Image
{
    public class AsImage : MonoBehaviour, IAsImageState, IAsImageMovement
    {
        private RawImage rawImage;

        private const float ShowHideDuration = 0.5f;

        public static AsImage GetAsImage(GameObject go)
        {
            var asImage = go.GetComponent<AsImage>();
            if (asImage == null) asImage = go.AddComponent<AsImage>();

            return asImage;
        }

        public void AsImageInit(Texture2D tex, int imageTypeIndex)
        {
            rawImage = GetComponent<RawImage>();
            rawImage.texture = tex;

            // 0:bg, 1:mg, 2:fg
            rawImage.rectTransform.sizeDelta = imageTypeIndex switch
            {
                2 => new Vector2(tex.width, tex.height),
                1 => new Vector2(tex.width / 1.55f, tex.height / 1.55f),
                _ => new Vector2(tex.width * 1.6f, tex.height * 1.6f)
            };
            if (imageTypeIndex == 1) Y(112);

            StateInit();
            MovementInit();
        }

        // Image State

        private void StateInit()
        {
            Disappear();
        }

        public void Show()
        {
            rawImage.color = new Color(1, 1, 1, 0);
            Appear();
            Fade(1, ShowHideDuration);
        }

        public void Hide()
        {
            rawImage.DOFade(0, ShowHideDuration).onComplete += Disappear;
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
            rawImage.DOFade(alpha, time);
        }

        // Image Movement

        private void MovementInit()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
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

        public void Shake(float strength, float time, int vibrato)
        {
            transform.DOShakePosition(time, strength, vibrato);
        }

        public void ShakeX(float strength, float time, int vibrato)
        {
            transform.DOShakePosition(time, Vector3.right * strength, vibrato);
        }

        public void ShakeY(float strength, float time, int vibrato)
        {
            transform.DOShakePosition(time, Vector3.down * strength, vibrato);
        }

        public void Scale(float scale, float time)
        {
            transform.DOScale(new Vector3(scale, scale, 1), time);
        }
    }
}
