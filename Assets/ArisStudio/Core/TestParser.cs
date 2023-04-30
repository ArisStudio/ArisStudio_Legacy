using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ArisStudio.Core
{
    public class TestParser : MonoBehaviour
    {
        public List<string> rawCommands = new List<string>();
        public List<string> finalCommand = new List<string>();
        public Dictionary<string, List<string>> m_Choices = new Dictionary<string, List<string>>();

        private void Start()
        {
#if UNITY_ANDROID
        string rootPath = $"file:///{Application.persistentDataPath}";
#elif UNITY_STANDALONE_OSX
        string rootPath = $"file://{Application.dataPath}";
#else
            string rootPath = Application.dataPath;
#endif
            string filePath = Path.Combine(rootPath, "Test", "test.txt");
            string parserPath = Path.Combine(rootPath, "Test", "test_parser.txt");

            try
            {
                // Store every line to list
                rawCommands = File.ReadAllLines(filePath).ToList();
                File.Delete(parserPath);
                // Remove all empty lines
                rawCommands.RemoveAll(string.IsNullOrEmpty);

                foreach (string line in rawCommands)
                {
                    File.AppendAllText(parserPath, string.Join(" | ", AsCommand.Parse(line)) + "\n");
                    finalCommand.Clear();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
            }
        }
    }
}
