using System;
using ArisStudio.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArisStudio.Core
{
    /// <summary>
    /// Class responsible for in-game Debug Console.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Debug Console [Singleton]")]
    public class DebugConsole : Singleton<DebugConsole>
    {
        [Header("Debug Window")] [SerializeField]
        public TMP_Text m_DebugText;

        [SerializeField] TMP_InputField m_ConsoleInputField;

        [Header("FPS Counter")] [SerializeField]
        TMP_Text m_FPSText;

        [SerializeField, Range(1, 10)] int m_UpdateRate = 4;
        int frameCount;

        float deltaTime,
            fps;

        void Awake()
        {
        }

        void Update()
        {
            DisplayFPS();
        }

        /// <summary>
        /// Display current FPS.
        /// </summary>
        void DisplayFPS()
        {
            deltaTime += Time.unscaledDeltaTime;
            frameCount++;

            if (deltaTime > 1f / m_UpdateRate)
            {
                // Calculate FPS
                fps = frameCount / deltaTime;

                // Update fps text
                m_FPSText.text = $"{Mathf.RoundToInt(fps)} FPS";

                // Reset variables
                deltaTime = 0f;
                frameCount = 0;
            }
        }


        public void PrintLoadLog(string type, string fileName, string nameId)
        {
            PrintLog($"Load <#ff0080>{type}/color>: <#8000ff>{fileName}</color> as <#00ff00>{nameId}</color>");
        }

        /// <summary>
        /// Print message to Debug window.
        /// </summary>
        /// <param name="debugMessage"></param>
        public void PrintLog(string debugMessage)
        {
            DateTime timeNow = DateTime.Now;
            string timePattern = @"HH:mm:ss";

            m_DebugText.text += $"[{timeNow.ToString(timePattern)}] {debugMessage}\n";
        }

        /// <summary>
        /// Execute console command.
        /// </summary>
        public void RunCommand()
        {
            m_ConsoleInputField.ActivateInputField();
            string command = m_ConsoleInputField.text.Trim();
            if (command == string.Empty)
                return;

            MainControl.Instance.PreLoad(command);
            MainControl.Instance.RunText(command);
            PrintLog($"> <#00ffff><b>{command}</b></color>");
            m_ConsoleInputField.text = string.Empty;
        }
    }
}
