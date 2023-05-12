#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ArisStudio.Utils.CustomEditors
{
    [CustomEditor(typeof(ScreenshotUtility))]
    public class ScreenshotUtilityEditor : Editor
    {
        ScreenshotUtility screenshotUtility => (ScreenshotUtility)target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(5);

            if (GUILayout.Button("Take Screenshot"))
            {
                screenshotUtility.TakeScreenshot();
            }
        }
    }
}
#endif // UNITY_EDITOR
