using UnityEngine;

namespace ArisStudio
{
    public class SettingMenu : MonoBehaviour
    {
        public GameObject settingArea;

        public void ChangeMenuState()
        {
            settingArea.SetActive(!settingArea.activeSelf);
        }
    }
}