using System;
using UnityEngine;

namespace ArisStudio.Components.Button
{
    public class ButtonLightFlow : MonoBehaviour
    {
        private UnityEngine.UI.Image image;
        public float angle;

        private void Start()
        {
            image = GetComponent<UnityEngine.UI.Image>();
        }

        private void Update()
        {
            if (angle > 360) angle = 0;

            angle += Time.deltaTime * 100;
            image.material.SetFloat("_StartAngle", angle);
        }
    }
}