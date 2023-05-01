using ArisStudio.Spr;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace ArisStudio.AsGameObject.Character
{
    public class AsCharacter : MonoBehaviour, IAsCharacterState, IAsCharacterMovement
    {
        private Transform asCharacterBaseTf;

        private SkeletonAnimation skeletonAnimation;
        private MaterialPropertyBlock materialPropertyBlock;
        private MeshRenderer meshRenderer;
        private Animator charAnimator, emoAnimator;

        private OldSprEmotion oldSprEmotion;

        private string materialType;
        private float highlightValue;

        public bool IsCommunication { get; private set; }

        private const float SHDuration = 0.5f;

        private static readonly int HighlightMat = Shader.PropertyToID("_Highlight");


        public static AsCharacter GetAsCharacter(GameObject go)
        {
            var asChar = go.GetComponent<AsCharacter>();
            if (asChar == null) asChar = go.AddComponent<AsCharacter>();

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
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            meshRenderer = GetComponent<MeshRenderer>();
            materialPropertyBlock = new MaterialPropertyBlock();
            Highlight(0, 0);

            oldSprEmotion = GetComponent<OldSprEmotion>();

            // charAnimator = GetComponent<Animator>();
            // emoAnimator = transform.GetChild(0).GetComponent<Animator>();
            Disappear();
        }

        public void Show()
        {
            highlightValue = 0;
            Appear();
            Highlight(1, SHDuration);
        }

        public void Hide()
        {
            DOVirtual.Float(highlightValue, 0, SHDuration, value =>
            {
                materialPropertyBlock.SetFloat(HighlightMat, value);
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
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

        public void Highlight(float hl, float time)
        {
            DOVirtual.Float(highlightValue, hl, time, value =>
            {
                materialPropertyBlock.SetFloat(HighlightMat, value);
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            });
            highlightValue = hl;
        }

        public void Fade(float alpha, float time)
        {
            throw new System.NotImplementedException();
        }

        public void State(string stateName, int trackIndex, bool loop)
        {
            skeletonAnimation.AnimationState.SetAnimation(trackIndex, stateName, loop);
        }

        public void Skin(string skinName)
        {
            skeletonAnimation.skeleton.SetSkin(skinName);
        }

        public void Emotion(string emotionName)
        {
            oldSprEmotion.PlayEmoticon(emotionName);
        }

        public void Animation(string animationName)
        {
            charAnimator.Play(animationName);
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

        public void Shake(float strength, float time, int vibrato)
        {
            asCharacterBaseTf.DOShakePosition(time, strength, vibrato);
        }

        public void ShakeX(float strength, float time, int vibrato)
        {
            asCharacterBaseTf.DOShakePosition(time, Vector3.right * strength, vibrato);
        }

        public void ShakeY(float strength, float time, int vibrato)
        {
            asCharacterBaseTf.DOShakePosition(time, Vector3.down * strength, vibrato);
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
