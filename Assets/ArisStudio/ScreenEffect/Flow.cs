using UnityEngine;

namespace ArisStudio.ScreenEffect
{
    public class Flow : MonoBehaviour
    {
        public float speed = 80;
        public int split = 3;
        public bool moveRight = true;

        private float w;
        private float targetX;

        private void Start()
        {
            w = gameObject.GetComponent<RectTransform>().rect.width / split;
            if (!moveRight)
            {
                w = -w;
            }

            targetX = transform.localPosition.x + w;
        }

        private void Update()
        {
            if (moveRight)
            {
                if (transform.localPosition.x >= targetX)
                {
                    transform.localPosition =
                        new Vector3(targetX - w, transform.localPosition.y, transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                        new Vector3(targetX, transform.localPosition.y, transform.localPosition.z), speed * Time.deltaTime);
                }
            }
            else
            {
                if (transform.localPosition.x <= targetX)
                {
                    transform.localPosition =
                        new Vector3(targetX - w, transform.localPosition.y, transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                        new Vector3(targetX, transform.localPosition.y, transform.localPosition.z), speed * Time.deltaTime);
                }
            }
        }
    }
}