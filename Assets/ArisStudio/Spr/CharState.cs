using System.Collections.Generic;
using UnityEngine;

namespace ArisStudio.Spr
{
    public class CharState : MonoBehaviour
    {
        private SpriteRenderer sd;
        private SprState sprState;

        private Dictionary<string, Sprite> charState;

        //Show, Hide
        private float showTimer, hideTimer;
        private const float ChangeShowTime = 0.4f;
        private const float ChangeHideTime = 0.4f;

        private bool showing, hiding;

        private bool isComm;

        private static readonly int FillPhase = Shader.PropertyToID("_FillPhase");
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        // Update is called once per frame
        private void Update()
        {
            if (showing)
            {
                showTimer += Time.deltaTime;
                if (showTimer >= ChangeShowTime || showTimer / ChangeShowTime >= 0.95f)
                {
                    showTimer = 0;
                    if (isComm)
                    {
                        sd.material.SetColor(Color1, new Color(1, 1, 1, 1));
                    }
                    else
                    {
                        sd.material.SetFloat(FillPhase, 0);
                    }

                    showing = false;
                }
                else
                {
                    if (isComm)
                    {
                        sd.material.SetColor(Color1, new Color(1, 1, 1, showTimer / ChangeShowTime));
                    }
                    else
                    {
                        sd.material.SetFloat(FillPhase, 1 - showTimer / ChangeShowTime);
                    }
                }
            }

            else if (hiding)
            {
                hideTimer += Time.deltaTime;
                if (hideTimer >= ChangeHideTime || hideTimer / ChangeHideTime >= 0.95f)
                {
                    hideTimer = 0;
                    if (isComm)
                    {
                        sd.material.SetColor(Color1, new Color(1, 1, 1, 0));
                    }
                    else
                    {
                        sd.material.SetFloat(FillPhase, 1);
                    }

                    hiding = false;
                    gameObject.SetActive(false);
                }
                else
                {
                    if (isComm)
                    {
                        sd.material.SetColor(Color1, new Color(1, 1, 1, 1 - hideTimer / ChangeHideTime));
                    }
                    else
                    {
                        sd.material.SetFloat(FillPhase, hideTimer / ChangeHideTime);
                    }
                }
            }
        }

        public void Init(Dictionary<string, Sprite> charSpriteList)
        {
            sprState = GetComponent<SprState>();
            charState = charSpriteList;
            sd = GetComponent<SpriteRenderer>();
            isComm = sd.material.name == "Comm (Instance)";
            sprState.IsComm = isComm;
        }

        public void Show()
        {
            showing = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            hiding = true;
        }

        public void HighLight(float f)
        {
            if (isComm)
            {
                sd.material.SetColor(Color1, new Color(1, 1, 1, f));
            }
            else
            {
                sd.material.SetFloat(FillPhase, 1 - f);
            }
        }

        public void SetState(string stateName)
        {
            sd.sprite = charState[stateName];
        }
    }
}