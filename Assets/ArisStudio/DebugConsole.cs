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

            if (!Input.GetKey(KeyCode.LeftControl) || !Input.GetKeyDown(KeyCode.Return)) return;

            consoleInputField.ActivateInputField();
            RunCommand();
        }

        public void PrintLog(string debugMessage)
        {
            debugText.text += $"{debugMessage}\n";
        }

        public void RunCommand()
        {
            var sTmp = consoleInputField.text.Trim();
            mainControl.PreLoad(sTmp);
            mainControl.RunText(sTmp);
            PrintLog($"> <b>{sTmp}</b>");
            consoleInputField.text = string.Empty;
        }
    }
}