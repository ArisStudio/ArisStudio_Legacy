using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace ArisStudio.AsGameObject.Character
{
    public class AsCharacter : MonoBehaviour, IAsCharacterState, IAsCharacterMovement
    {
        private Transform asCharacterBaseTf;

        private SkeletonAnimation sa;
        private MaterialPropertyBlock mpb;
        private MeshRenderer md;

        private string materialType;
        private float highlightValue;

        public bool IsCommunication { get; private set; }

        private static readonly int HighlightMat = Shader.PropertyToID("_Highlight");


        public static AsCharacter GetAsCharacter(GameObject go)
        {
            var asChar = go.GetComponent<AsCharacter>();
            if (asChar == null)
            {
                asChar = go.AddComponent<AsCharacter>();
            }

            return asChar;
        }

        public void AsCharacterInit(bool isCommunication)
        {
            asCharacterBaseTf = transform.parent.gameObject.transform;
            IsCommunication = isCommunication;
            StateInit();
            MovementInit();
        }

        // Character State

        private void StateInit()
        {
            sa = GetComponent<SkeletonAnimation>();
            md = GetComponent<MeshRenderer>();
            mpb = new MaterialPropertyBlock();
            Highlight(0);
            Disappear();
        }

        public void Show()
        {
            highlightValue = 0;
            Appear();
            Highlight(1, 0.5f);
        }

        public void Hide()
        {
            DOVirtual.Float(highlightValue, 0, 0.5f, value =>
            {
                mpb.SetFloat(HighlightMat, value);
                md.SetPropertyBlock(mpb);
            }).onComplete += Disappear;
            highlightValue = 0;
        }

        public void Appear()
        {
            gameObject.SetActive(true);
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
        }

        public void Highlight(float hl)
        {
            Highlight(hl, 0);
        }

        public void Highlight(float hl, float time)
        {
            DOVirtual.Float(highlightValue, hl, time, value =>
            {
                mpb.SetFloat(HighlightMat, value);
                md.SetPropertyBlock(mpb);
            });
            highlightValue = hl;
        }

        public void Fade(float alpha, float time)
        {
            throw new System.NotImplementedException();
        }

        public void State(string stateName, int trackIndex, bool loop)
        {
            sa.AnimationState.SetAnimation(trackIndex, stateName, loop);
        }

        public void Skin(string skinName)
        {
            sa.skeleton.SetSkin(skinName);
        }

        public void Emotion(string emotionName)
        {
            throw new System.NotImplementedException();
        }

        public void Animation(string animationName)
        {
            throw new System.NotImplementedException();
        }


        // Character Movement

        private void MovementInit()
        {
            asCharacterBaseTf.localPosition = Vector3.zero;
            asCharacterBaseTf.localScale = Vector3.one;
        }

        public void X(float x)
        {
            asCharacterBaseTf.localPosition += Vector3.right * x;
        }

        public void Y(float y)
        {
            asCharacterBaseTf.localPosition += Vector3.up * y;
        }

        public void Z(float z)
        {
            asCharacterBaseTf.localPosition += Vector3.forward * z;
        }

        public void Position(float x, float y)
        {
            asCharacterBaseTf.localPosition = new Vector3(x, y, asCharacterBaseTf.localPosition.z);
        }

        public void MoveX(float x, float time)
        {
            asCharacterBaseTf.DOMoveX(x, time);
        }

        public void MoveY(float y, float time)
        {
            asCharacterBaseTf.DOMoveY(y, time);
        }

        public void MovePosition(float x, float y, float time)
        {
            asCharacterBaseTf.DOMove(new Vector3(x, y, asCharacterBaseTf.localPosition.z), time);
        }

        public void Shake(float strength, float time)
        {
            asCharacterBaseTf.DOShakePosition(time, strength);
        }

        public void ShakeX(float strength, float time)
        {
            asCharacterBaseTf.DOShakePosition(time, Vector3.right * strength);
        }

        public void ShakeY(float strength, float time)
        {
            asCharacterBaseTf.DOShakePosition(time, Vector3.down * strength);
        }

        public void Scale(float scale, float time)
        {
            asCharacterBaseTf.DOScale(new Vector3(scale, scale, 1), time);
        }

        public void Close()
        {
            asCharacterBaseTf.localScale = new Vector3(1.5f, 1.5f, 1);
            asCharacterBaseTf.localPosition += Vector3.down * 8.5f;
        }

        public void Back()
        {
            asCharacterBaseTf.localScale = Vector3.one;
            asCharacterBaseTf.localPosition += Vector3.up * 8.5f;
        }
    }
}
