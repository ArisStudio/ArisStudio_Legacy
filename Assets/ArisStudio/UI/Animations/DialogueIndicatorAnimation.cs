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

        void Awake()
        {
            TryGetComponent<Image>(out Image image);
        }

        void StartAnimate()
        {
            image.DOFade(1, 0.1f);

            transform.DOMoveY(transform.position.y - m_YEndPosition, m_MoveDuration, false).SetEase(m_MoveEase).SetLoops(-1, LoopType.Yoyo);
        }

        void StopAnimate()
        {
            image.DOFade(0, 0.1f);

            DOTween.Complete(image);
            DOTween.Complete(transform);
        }
    }
}
