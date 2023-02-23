using System;
using ArisStudio.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArisStudio.Core
{
    /// <summary>
    /// Class responsible for in-game Debug Console.
    /// </summary>
    [AddComponentMenu("Aris Studio/Core/Debug Console")]
    public class DebugConsole : MonoBehaviour
    {
        [Header("Debug Window")]
        [SerializeField] TMP_Text m_DebugText;
        [SerializeField] TMP_InputField m_ConsoleInputField;

        [Header("FPS Counter")]
        [SerializeField] TMP_Text m_FPSText;
        [SerializeField, Range(1, 10)] int m_UpdateRate = 4;
        int frameCount;
        float deltaTime, fps;

        void Awake()
        {
            // Clear existing debug text
            m_DebugText.text = string.Empty;
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
        /// Execute Debug window command.
        /// </summary>
        public void RunCommand()
        {
            m_ConsoleInputField.ActivateInputField();
            string sTmp = m_ConsoleInputField.text.Trim();
            if (sTmp == string.Empty) return;

            MainControl.Instance.PreLoad(sTmp);
            MainControl.Instance.RunText(sTmp);
            PrintLog($"> <#00ffff><b>{sTmp}</b></color>");
            m_ConsoleInputField.text = string.Empty;
        }
    }
}
