using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio
{
    public class DebugConsole : MonoBehaviour
    {
        public MainControl mainControl;

        public Text debugText, fpsText;
        public InputField consoleInputField;

        private void Update()
        {
            var fpsNow = 1 / Time.deltaTime;
            fpsText.text = $"{fpsNow:F2} FPS";
        }

        public void PrintLog(string debugMessage)
        {
            debugText.text += $"{debugMessage}\n";
        }

        public void RunCommand()
        {
            consoleInputField.ActivateInputField();
            var sTmp = consoleInputField.text.Trim();
            if (sTmp == string.Empty) return;

            mainControl.PreLoad(sTmp);
            mainControl.RunText(sTmp);
            PrintLog($"> <color=cyan><b>{sTmp}</b></color>");
            consoleInputField.text = string.Empty;
        }
    }
}