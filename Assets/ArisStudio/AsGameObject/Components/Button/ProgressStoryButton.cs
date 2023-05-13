using ArisStudio.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArisStudio.AsGameObject.Components
{
    public class ProgressStoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool pointerEntered { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MainManager.Instance.asCommandListLength > 0)
                pointerEntered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (MainManager.Instance.asCommandListLength > 0)
                pointerEntered = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MainManager.Instance.ProgressStory();
        }
    }
}
