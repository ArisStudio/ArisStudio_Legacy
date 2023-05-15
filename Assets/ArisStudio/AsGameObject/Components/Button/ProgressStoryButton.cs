using ArisStudio.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityImage = UnityEngine.UI.Image;

namespace ArisStudio.AsGameObject.Components
{
    [AddComponentMenu("Aris Studio/AsGameObject/Components/Progress Story Button")]
    [RequireComponent(typeof(UnityImage), typeof(Button))]
    public class ProgressStoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool pointerEntered { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MainManager.Instance.AsCommandListLength > 0)
                pointerEntered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (MainManager.Instance.AsCommandListLength > 0)
                pointerEntered = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MainManager.Instance.ProgressStory();
        }
    }
}
