using UnityEngine;

namespace ArisStudio.Spr
{
    public class SprAnimation : MonoBehaviour
    {
        private Animator sprAnimator;

        private void Start()
        {
            sprAnimator = GetComponent<Animator>();
        }

        public void Empty()
        {
            sprAnimator.Play("Empty");
        }

        public void Down()
        {
            sprAnimator.Play("SprDown");
        }

        public void Up()
        {
            sprAnimator.Play("SprUp");
        }
    }
}