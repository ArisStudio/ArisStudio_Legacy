using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    public class Label : MonoBehaviour
    {
        public Text labelText;
        private bool showing;
        private float t;
        private const float ShowTime = 2f;

        private void Update()
        {
            if (!showing) return;
            t += Time.deltaTime;

            if (t < ShowTime) return;

            t = 0;
            showing = false;
            gameObject.SetActive(false);
        }

        public void SetLabelText(string text)
        {
            t = 0;
            // labelText.text = text.Split('\'')[1];
            labelText.text = text;
            showing = true;
            gameObject.SetActive(true);
        }
    }
}
