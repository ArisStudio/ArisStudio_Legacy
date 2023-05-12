#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ArisStudio.Utils.CustomEditors
{
    [CustomEditor(typeof(TestParser))]
    public class TestParserEditor : Editor
    {
        TestParser testParser => (TestParser)target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(5);

            if (GUILayout.Button("Test Out!"))
            {
                testParser.Test();
            }
        }
    }
}
#endif // UNITY_EDITOR
