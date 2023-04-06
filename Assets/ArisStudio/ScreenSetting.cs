using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio
{
    public class ScreenSetting : MonoBehaviour
    {
        public DebugConsole debugConsole;

        public Dropdown resolutionDropdown, fpsDropdown;

        public void ChangeFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            debugConsole.PrintLog($"FullScreen = {Screen.fullScreen}");
        }

        public void ChangeResolution()
        {
            var resolution = resolutionDropdown.options[resolutionDropdown.value].text.Trim();
            var r = resolution.Split('x');
            Screen.SetResolution(int.Parse(r[0]), int.Parse(r[1]), Screen.fullScreen);
            debugConsole.PrintLog($"Resolution = {resolution}");
        }


        public void ChangeFPS()
        {
            var fps = fpsDropdown.options[fpsDropdown.value].text.Trim();
            if (fps == "Unlimited")
            {
                Application.targetFrameRate = -1;
            }
            else
            {
                Application.targetFrameRate = int.Parse(fps);
            }

            debugConsole.PrintLog($"FPS = {fps}");
        }

        public void ChangeVSync()
        {
            QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
            debugConsole.PrintLog($"VSync = {QualitySettings.vSyncCount != 0}");
        }
    }
}