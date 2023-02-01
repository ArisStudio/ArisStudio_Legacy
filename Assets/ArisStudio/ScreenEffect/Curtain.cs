using UnityEngine;

namespace ArisStudio.ScreenEffect
{
    public class Curtain : MonoBehaviour
    {
        private UnityEngine.UI.Image curtain;

        private float showTime;
        private const float ChangeShowTime = 0.5f;
        private bool showing;

        private float hideTime;
        private const float ChangeHideTime = 0.5f;
        private bool hiding;

        private void Start()
        {
            curtain = GetComponent<UnityEngine.UI.Image>();
        }

        private void Update()
        {
            if (showing)
            {
                showTime += Time.deltaTime;
                if (showTime >= ChangeShowTime || showTime / ChangeShowTime >= 0.95f)
                {
                    showTime = 0;
                    curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 1);
                    showing = false;
                }
                else
                {
                    curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, showTime / ChangeShowTime);
                }
            }
            else if (hiding)
            {
                hideTime += Time.deltaTime;
                if (hideTime >= ChangeHideTime || hideTime / ChangeHideTime >= 0.95f)
                {
                    hideTime = 0;
                    curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 0);
                    hiding = false;
                    gameObject.SetActive(false);
                }
                else
                {
                    curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 1 - (hideTime / ChangeHideTime));
                }
            }
        }

        public void Show()
        {
            curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 0);
            showing = true;
            gameObject.SetActive(true);
        }

        public void ShowD()
        {
            curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 1);
            gameObject.SetActive(true);
        }

        public void Alpha(float alpha)
        {
            curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, alpha);
        }

        public void Hide()
        {
            hiding = true;
        }

        public void HideD()
        {
            gameObject.SetActive(false);
        }

        public void Black()
        {
            curtain.color = Color.black;
        }

        public void White()
        {
            curtain.color = Color.white;
        }

        public void Red()
        {
            curtain.color = Color.red;
        }

        public void SetColor(string color)
        {
            Debug.Log(color);
            curtain.color = ColorUtility.TryParseHtmlString(color, out var c) ? c : curtain.color;
        }
    }
}