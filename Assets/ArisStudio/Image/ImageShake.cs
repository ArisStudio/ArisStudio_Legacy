using UnityEngine;

namespace ArisStudio.Image
{
    public class ImageShake : MonoBehaviour
    {
        //Shake
        private float oXPosition, oYPosition;
        private float shakeTimeX, shakeTimeY;
        private float shakeXa, shakeYa;
        private float shakeXh, shakeYh;
        private float shakeXt, shakeYt;
        private bool xShaking, yShaking;

        private void Update()
        {
            if (xShaking)
            {
                shakeTimeX += Time.deltaTime;
                if (shakeTimeX * shakeXa > shakeXt)
                {
                    transform.localPosition = new Vector3(oXPosition, 0, 0);
                    shakeXa = 0;
                    shakeXh = 0;
                    shakeTimeX = 0;
                    xShaking = false;
                }
                else
                {
                    transform.localPosition = new Vector3(Mathf.Sin(shakeTimeX * Mathf.PI * shakeXa) * shakeXh + oXPosition, 0, 0);
                }
            }

            if (yShaking)
            {
                shakeTimeY += Time.deltaTime;
                if (shakeTimeY * shakeYa > shakeYt)
                {
                    transform.localPosition = new Vector3(0, oYPosition, 0);
                    shakeYa = 0;
                    shakeYh = 0;
                    shakeTimeY = 0;
                    yShaking = false;
                }
                else
                {
                    transform.localPosition = new Vector3(0, Mathf.Sin(shakeTimeY * Mathf.PI * shakeYa) * shakeYh + oYPosition, 0);
                }
            }
        }

        public void ShakeX(float xa, float xh, float xt)
        {
            shakeXa = xa;
            shakeXh = xh;
            shakeXt = xt;
            oXPosition = transform.localPosition.x;
            xShaking = true;
        }

        public void ShakeY(float ya, float yh, float yt)
        {
            shakeYa = ya;
            shakeYh = yh;
            shakeYt = yt;
            oYPosition = transform.localPosition.y;
            yShaking = true;
        }
    }
}