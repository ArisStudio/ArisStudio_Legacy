using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio
{
    public class DebugConsole : MonoBehaviour
    {
        public Text debugText;

        public InputField consoleInputField;

        private void Start()
        {
        }

        public void PrintLog(string debugMessage)
        {
            debugText.text += $"{debugMessage}\n";
        }
    }
}