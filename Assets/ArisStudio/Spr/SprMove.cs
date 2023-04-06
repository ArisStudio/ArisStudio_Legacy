using Unity.Mathematics;
using UnityEngine;

namespace ArisStudio.Spr
{
    public class SprMove : MonoBehaviour
    {
        public GameObject sprBase;

        //Move
        private float moveX, moveY, distanceX;
        private float moveXSpeed, moveYSpeed;
        private bool xMoving, yMoving;

        //Shake
        private float oXPosition, oYPosition;
        private float shakeTimeX;
        private float shakeTimeY;
        private float shakeXa, shakeYa;
        private float shakeXh, shakeYh;
        private float shakeXt, shakeYt;
        private bool xShaking, yShaking;

        private void Update()
        {
            if (xMoving)
            {
                if (math.abs(moveX - sprBase.transform.localPosition.x) <= 0.1f)
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = new Vector3(moveX, sbLp.y, sbLp.z);
                    sprBase.transform.localPosition = sbLp;
                    xMoving = false;
                }
                else
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = Vector3.MoveTowards(sbLp, new Vector3(moveX, sbLp.y, sbLp.z),
                        moveXSpeed * Mathf.Sin(Time.deltaTime * Mathf.PI * math.abs(moveX - sbLp.x) / distanceX));
                    sprBase.transform.localPosition = sbLp;
                }
            }

            if (yMoving)
            {
                if (math.abs(moveY - sprBase.transform.localPosition.y) <= 0.1f)
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = new Vector3(sbLp.x, moveY, sbLp.z);
                    sprBase.transform.localPosition = sbLp;
                    yMoving = false;
                }
                else
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = Vector3.MoveTowards(sbLp, new Vector3(sbLp.x, moveY, sbLp.z), moveYSpeed * Time.deltaTime);
                    sprBase.transform.localPosition = sbLp;
                }
            }

            if (xShaking)
            {
                shakeTimeX += Time.deltaTime;
                if (shakeTimeX * shakeXa > shakeXt)
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = new Vector3(oXPosition, sbLp.y, sbLp.z);
                    sprBase.transform.localPosition = sbLp;
                    shakeXa = 0;
                    shakeXh = 0;
                    shakeTimeX = 0;
                    xShaking = false;
                }
                else
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = new Vector3(Mathf.Sin(shakeTimeX * Mathf.PI * shakeXa) * shakeXh + oXPosition, sbLp.y, sbLp.z);
                    sprBase.transform.localPosition = sbLp;
                }
            }

            if (yShaking)
            {
                shakeTimeY += Time.deltaTime;
                if (shakeTimeY * shakeYa > shakeYt)
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = new Vector3(sbLp.x, oYPosition, sbLp.z);
                    sprBase.transform.localPosition = sbLp;
                    shakeYa = 0;
                    shakeYh = 0;
                    shakeTimeY = 0;
                    yShaking = false;
                }
                else
                {
                    var sbLp = sprBase.transform.localPosition;
                    sbLp = new Vector3(sbLp.x, Mathf.Sin(shakeTimeY * Mathf.PI * shakeYa) * shakeYh + oYPosition, sbLp.z);
                    sprBase.transform.localPosition = sbLp;
                }
            }
        }

        public void SetX(float x)
        {
            var sbLp = sprBase.transform.localPosition;
            sbLp = new Vector3(x, sbLp.y, sbLp.z);
            sprBase.transform.localPosition = sbLp;
        }

        public void SetY(float y)
        {
            var sbLp = sprBase.transform.localPosition;
            sbLp = new Vector3(sbLp.x, y, sbLp.z);
            sprBase.transform.localPosition = sbLp;
        }

        public void SetZ(float z)
        {
            var sbLp = sprBase.transform.localPosition;
            sbLp = new Vector3(sbLp.x, sbLp.y, -z);
            sprBase.transform.localPosition = sbLp;
        }

        public void Move2X(float x, float speed)
        {
            moveX = x;
            distanceX = math.abs(moveX - sprBase.transform.localPosition.x);
            moveXSpeed = speed;
            xMoving = true;
        }

        public void Move2Y(float y, float speed)
        {
            moveY = y;
            moveYSpeed = speed;
            yMoving = true;
        }

        public void Close()
        {
            sprBase.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            sprBase.transform.localPosition = new Vector3(sprBase.transform.localPosition.x, sprBase.transform.localPosition.y - 8.5f, 0);
        }

        public void Back()
        {
            sprBase.transform.localScale = Vector3.one;
            sprBase.transform.localPosition = new Vector3(sprBase.transform.localPosition.x, sprBase.transform.localPosition.y + 8.5f, 0);
        }

        public void ShakeX(float xa, float xh, float xt)
        {
            shakeXa = xa;
            shakeXh = xh * 0.2f;
            shakeXt = xt;
            oXPosition = sprBase.transform.localPosition.x;
            xShaking = true;
        }

        public void ShakeY(float ya, float yh, float yt)
        {
            shakeYa = ya;
            shakeYh = yh * 0.4f;
            shakeYt = yt;
            oYPosition = sprBase.transform.localPosition.y;
            yShaking = true;
        }
    }
}