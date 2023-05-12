using ScreenshotCompanionCore;
using TMPro;
using UnityEngine;

namespace ArisStudio.Utils
{
    /// <summary>
    /// Small utility to take screenshot.
    /// </summary>
    [AddComponentMenu("Aris Studio/Utility/Screenshot")]
    [RequireComponent(typeof(ScreenshotCompanion))]
    public class ScreenshotUtility : MonoBehaviour
    {
        [Tooltip("Resolution Setting will be used at runtime to capture the whole screen according to the choosed resolution at the setting menu. Meanwhile, if you\'re in the Editor, you can just change the resolution (Size Multiplier) at Screenshot Companion")]
        [SerializeField] TMP_Dropdown m_ResolutionSetting;

        float multiplierSize = 1f;
        ScreenshotCompanion screenshot => GetComponent<ScreenshotCompanion>();

        void Start()
        {
            // Change capture method to Render Texture at runtime if you forgot to switch back to the initial capture method at Editor.
            screenshot.settings.captureMethod = ScreenshotCompanion.CaptureMethod.RenderTexture;
        }

        /// <summary>
        /// Set multiplier size according to resolution setting.
        /// NOTE: Add this to OnValueChanged in Resolution Setting Dropdown.
        /// </summary>
        public void ChangeMultiplierSize()
        {
            if (m_ResolutionSetting == null)
            {
                Debug.LogError("<b>Resolution Dropdown Setting</b> isn't assigned!");
                return;
            }

            switch (m_ResolutionSetting.value)
            {
                case 0: // 1280x720
                    multiplierSize = 1f;
                    break;
                case 1: // 1920x1080
                    multiplierSize = 1.5f;
                    break;
                case 2: // 2560x1440
                    multiplierSize = 2f;
                    break;
                case 3: // 3840x2160
                    multiplierSize = 3f;
                    break;
            }

            screenshot.settings.renderSizeMultiplier = multiplierSize;
        }

        /// <summary>
        /// Take screenshot.
        /// </summary>
        public void TakeScreenshot()
        {
            screenshot.CaptureRenderTexture(Camera.main, 0);
        }
    }
}
