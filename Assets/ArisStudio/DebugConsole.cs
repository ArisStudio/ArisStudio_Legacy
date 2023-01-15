using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio
{
    public class DebugConsole : MonoBehaviour
    {
        public MainControl mainControl;

        public Text debugText;
        public InputField consoleInputField;

        private void Update()
        {
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
            mainControl.RunText(sTmp);
            PrintLog($"> <b>{sTmp}</b>");
            consoleInputField.text = string.Empty;
        }
    }
}