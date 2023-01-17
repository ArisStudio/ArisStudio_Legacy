using UnityEngine;

namespace ArisStudio
{
    public class OtherShortcut : MonoBehaviour
    {
        public GameObject settingArea, autoButtonDefault, autoButtonSelect;
        public DebugConsole debugConsole;
        public MainControl mainControl;

        private void Update()
        {
            if (debugConsole.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    debugConsole.RunCommand();
                }

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
                {
                    debugConsole.gameObject.SetActive(false);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                settingArea.SetActive(!settingArea.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                debugConsole.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (autoButtonDefault.activeSelf)
                {
                    mainControl.SetAuto(true);
                    autoButtonDefault.SetActive(false);
                    autoButtonSelect.SetActive(true);
                    return;
                }

                if (autoButtonSelect.activeSelf)
                {
                    mainControl.SetAuto(false);
                    autoButtonDefault.SetActive(true);
                    autoButtonSelect.SetActive(false);
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                mainControl.LoadTextData();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mainControl.SetPlay();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }
    }
}