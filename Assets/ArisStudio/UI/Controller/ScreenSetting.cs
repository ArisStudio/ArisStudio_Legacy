using ArisStudio.Core;
using TMPro;
using UnityEngine;

namespace ArisStudio.UI
{
    public class ScreenSetting : MonoBehaviour
    {
        [SerializeField]
        TMP_Dropdown m_ResolutionDropdown,
            m_FPSDropdown;

        // public void ChangeFullScreen()
        // {
        //     Screen.fullScreen = !Screen.fullScreen;
        //     MainControl.Instance.m_DebugConsole.PrintLog($"FullScreen = {Screen.fullScreen}");
        // }

        // public void ChangeResolution()
        // {
        //     string resolution = m_ResolutionDropdown.options[
        //         m_ResolutionDropdown.value
        //     ].text.Trim();
        //     string[] r = resolution.Split('x');

        //     Screen.SetResolution(int.Parse(r[0]), int.Parse(r[1]), Screen.fullScreen);
        //     MainControl.Instance.m_DebugConsole.PrintLog($"Resolution = {resolution}");
        // }

        // public void ChangeFPS()
        // {
        //     string fps = m_FPSDropdown.options[m_FPSDropdown.value].text.Trim();

        //     if (fps == "Unlimited")
        //         Application.targetFrameRate = -1;
        //     else
        //         Application.targetFrameRate = int.Parse(fps);

        //     MainControl.Instance.m_DebugConsole.PrintLog($"FPS = {fps}");
        // }

        // public void ChangeVSync()
        // {
        //     QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
        //     MainControl.Instance.m_DebugConsole.PrintLog(
        //         $"VSync = {QualitySettings.vSyncCount != 0}"
        //     );
        // }
    }
}
