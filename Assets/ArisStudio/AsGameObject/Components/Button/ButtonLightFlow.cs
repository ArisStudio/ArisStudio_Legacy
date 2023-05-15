using System;
using UnityEngine;
using UnityImage = UnityEngine.UI.Image;

namespace ArisStudio.AsGameObject.Components
{
    [AddComponentMenu("Aris Studio/AsGameObject/Components/Button Light Flow")]
    public class ButtonLightFlow : MonoBehaviour
    {
        [SerializeField] float m_Angle;
        UnityImage image;

        private void Awake()
        {
            image = GetComponent<UnityImage>();
        }

        private void Update()
        {
            if (m_Angle > 360) m_Angle = 0;

            m_Angle += Time.deltaTime * 100;
            image.material.SetFloat("_StartAngle", m_Angle);
        }
    }
}
