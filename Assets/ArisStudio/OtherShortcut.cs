using UnityEngine;

namespace ArisStudio
{
    public class OtherShortcut : MonoBehaviour
    {
        public GameObject settingArea, debugConsole;

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
            {
                settingArea.SetActive(!settingArea.activeSelf);
            }

            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
            {
                debugConsole.SetActive(!debugConsole.activeSelf);
            }

            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }
    }
}