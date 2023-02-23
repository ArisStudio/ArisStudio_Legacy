using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TestParser : MonoBehaviour
{
    public List<string> rawCommands = new List<string>();
    public List<string> finalCommand = new List<string>();
    public Dictionary<string, List<string>> m_Choices = new Dictionary<string, List<string>>();

    void Start()
    {
#if UNITY_ANDROID
        string rootPath = $"file:///{Application.persistentDataPath}";
#elif UNITY_STANDALONE_OSX
        string rootPath = Directory.GetParent($"file://{Application.dataPath}").ToString();
#else
        string rootPath = Directory.GetParent(Application.dataPath).ToString();
#endif
        string filePath = Path.Combine(rootPath, "test.txt");

        try
        {
            // Store every line to list
            rawCommands = File.ReadAllLines(filePath).ToList();
            // Remove all empty lines
            rawCommands.RemoveAll(string.IsNullOrEmpty);

            foreach (string line in rawCommands)
            {
                /*
                * Split line with space delimiter then store to an array.
                * Source: txt 'Arona' "Arona" "I'm, Arona." 'Say, "Hello"' 'You\'re'
                * Result: ["txt", "'Arona'", "\"Arona\"", "\"I'm, Arona.\"", "'Say, "Hello"'", "'You\'re'"]
                */
                string pattern =
                    @"('[^'\\]*(?:\\.[^'\\]*)*')|(\""[^""\\]*(?:\\.[^""\\]*)*"")|(\S+)";
                string[] splittedCommand = Regex
                    .Matches(line, pattern)
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .ToArray();
                /*
                * Normalize result by removing either single or double quote at start and end of the word. Also Unescape escaped character.
                * Result: ["txt", "Arona", "Arona", "I'm, Arona.", "Say, \"Hello\"", "You're"]
                * Using string.Trim() resulting in unexpected result, so reconstruct it using string.Substring().
                */
                foreach (string str in splittedCommand)
                {
                    string cmd = str;

                    if ((str.StartsWith("'") && str.EndsWith("'")) || (str.StartsWith("\"") && str.EndsWith("\"")))
                    {
                        cmd = str.Substring(1, str.Length - 2);
                        cmd = Regex.Unescape(cmd);
                    }

                    finalCommand.Add(cmd);
                }
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
