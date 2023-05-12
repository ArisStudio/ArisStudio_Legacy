#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.Core;
using UnityEditor;
using UnityEngine;

namespace ArisStudio.Utils
{
    /// <summary>
    /// Testing out command parser.
    /// </summary>
    [AddComponentMenu("Aris Studio/Utility/Test Parser")]
    public class TestParser : MonoBehaviour
    {
        [SerializeField] TextAsset m_SourceFile;
        TextAsset resultFile;

        private List<string> rawCommands = new List<string>();

        /// <summary>
        /// Run test parser.
        /// </summary>
        public void Test()
        {
            string sourceFilePath = AssetDatabase.GetAssetPath(m_SourceFile);
            string resultFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath), $"{m_SourceFile.name}_result.txt");

            if (string.IsNullOrEmpty(sourceFilePath))
            {
                Debug.LogError("Source File is empty! There's no use to test the parser. Returning...");
                return;
            }

            try
            {
                rawCommands = File.ReadAllLines(sourceFilePath).ToList();
                rawCommands.RemoveAll(string.IsNullOrEmpty);

                if (string.IsNullOrEmpty(resultFilePath))
                {
                    resultFile = new TextAsset();
                    AssetDatabase.CreateAsset(resultFile, resultFilePath);
                    EditorUtility.SetDirty(resultFile);
                }

                using (StreamWriter writer = new StreamWriter(resultFilePath, false))
                {
                    foreach (string line in rawCommands)
                    {
                        writer.WriteLine(string.Join(" | ", AsCommand.Parse(line)));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed writing to file: {e.Message}");
            }
            finally
            {
                rawCommands.Clear();
                AssetDatabase.Refresh();
                resultFile = (TextAsset)AssetDatabase.LoadAssetAtPath(resultFilePath, typeof(TextAsset));
                EditorGUIUtility.PingObject(resultFile);
            }
        }
    }
}
#endif // UNITY_EDITOR
