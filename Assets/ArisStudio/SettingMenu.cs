using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArisStudio
{
    public class SettingMenu : MonoBehaviour
    {
        public GameObject settingArea;

        public void ChangeMenuState()
        {
            settingArea.SetActive(!settingArea.activeSelf);
        }

        public void Change2Preview()
        {
            Screen.SetResolution(512, 256, false);
            SceneManager.LoadScene("Preview");
        }
    }
}