using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    [AddComponentMenu("Aris Studio/UI/Animations/Dialogue Indicator Animation")]
    public class DialogueIndicatorAnimation : MonoBehaviour
    {
        [Range(0.1f, 1f), SerializeField] float m_YEndPosition = 0.5f;
        [Range(0.1f, 1f), SerializeField] float m_MoveDuration = 0.8f;
        [SerializeField] Ease m_MoveEase = Ease.InSine;

        Image image;
        RectTransform rectTransform;
        Vector3 originalPosition;

        void Awake()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            originalPosition = rectTransform.localPosition;
        }

        public void StartAnimate()
        {
            image.enabled = true;
            transform.DOMoveY(transform.position.y - m_YEndPosition, m_MoveDuration, false).SetEase(m_MoveEase).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopAnimate()
        {
            image.enabled = false;
            DOTween.Kill(transform);
            rectTransform.localPosition = originalPosition;
        }
    }
}
