using UnityEngine;

public class Flow : MonoBehaviour
{
    public float speed = 80;
    public int split = 3;
    public bool moveRight = true;

    float w;
    float targetX;

    void Start()
    {
        w = gameObject.GetComponent<RectTransform>().rect.width / split;
        if (!moveRight)
        {
            w = -w;
        }

        targetX = transform.localPosition.x + w;
    }

    // Update is called once per frame
    void Update()
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