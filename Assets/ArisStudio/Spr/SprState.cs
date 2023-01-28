using System.Linq;
using Spine.Unity;
using UnityEngine;

namespace ArisStudio.Spr
{
    public class SprState : MonoBehaviour
    {
        private SkeletonAnimation sa;
        private MaterialPropertyBlock mpb;
        MeshRenderer md;

        //EyeClose
        private bool isEyeClose;
        private float closeTimer;
        private string eyeCloseName;
        private const float CloseInterval = 5;

        //Show,Hide
        private float showTimer, hideTimer;
        private const float ChangeShowTime = 0.4f;
        private const float ChangeHideTime = 0.4f;

        private bool showing, hiding;

        public bool IsComm { get; private set; }

        private static readonly int FillPhase = Shader.PropertyToID("_FillPhase");
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        // Update is called once per frame
        private void Update()
        {
            if (isEyeClose)
            {
                closeTimer += Time.deltaTime;
                if (closeTimer >= CloseInterval)
                {
                    sa.AnimationState.AddAnimation(1, eyeCloseName, false, 0);
                    closeTimer = 0;
                }
            }

            if (showing)
            {
                showTimer += Time.deltaTime;
                if (showTimer >= ChangeShowTime || showTimer / ChangeShowTime >= 0.95f)
                {
                    showTimer = 0;
                    if (IsComm)
                    {
                        md.material.SetColor(Color1, new Color(1, 1, 1, 1));
                    }
                    else
                    {
                        mpb.SetFloat(FillPhase, 0);
                        md.SetPropertyBlock(mpb);
                    }

                    showing = false;
                }
                else
                {
                    if (IsComm)
                    {
                        md.material.SetColor(Color1, new Color(1, 1, 1, showTimer / ChangeShowTime));
                    }
                    else
                    {
                        mpb.SetFloat(FillPhase, 1 - showTimer / ChangeShowTime);
                        md.SetPropertyBlock(mpb);
                    }
                }
            }

            else if (hiding)
            {
                hideTimer += Time.deltaTime;
                if (hideTimer >= ChangeHideTime || hideTimer / ChangeHideTime >= 0.95f)
                {
                    hideTimer = 0;
                    if (IsComm)
                    {
                        md.material.SetColor(Color1, new Color(1, 1, 1, 0));
                    }
                    else
                    {
                        mpb.SetFloat(FillPhase, 1);
                        md.SetPropertyBlock(mpb);
                    }

                    hiding = false;
                    sa.gameObject.SetActive(false);
                }
                else
                {
                    if (IsComm)
                    {
                        md.material.SetColor(Color1, new Color(1, 1, 1, 1 - hideTimer / ChangeHideTime));
                    }
                    else
                    {
                        mpb.SetFloat(FillPhase, hideTimer / ChangeHideTime);
                        md.SetPropertyBlock(mpb);
                    }
                }
            }
        }

        public void Init()
        {
            sa = GetComponent<SkeletonAnimation>();
            md = GetComponent<MeshRenderer>();
            IsComm = md.material.name == "Comm (Instance)";
        }

        public void Show()
        {
            mpb = new MaterialPropertyBlock();
            showing = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            mpb = new MaterialPropertyBlock();
            hiding = true;
        }

        public void HighLight(float f)
        {
            if (IsComm)
            {
                md.material.SetColor(Color1, new Color(1, 1, 1, f));
            }
            else
            {
                mpb = new MaterialPropertyBlock();
                mpb.SetFloat(FillPhase, 1 - f);
                md.SetPropertyBlock(mpb);
            }
        }

        public void SetState(string stateName)
        {
            if (stateName.EndsWith("01"))
            {
                foreach (var a in sa.skeleton.Data.Animations.Where(a => a.Name.EndsWith("lose_" + stateName)))
                {
                    eyeCloseName = a.Name;
                    isEyeClose = true;
                }
            }
            else
            {
                isEyeClose = false;
                closeTimer = 0;
            }

            sa.AnimationState.SetAnimation(1, stateName, true);
        }
    }
}