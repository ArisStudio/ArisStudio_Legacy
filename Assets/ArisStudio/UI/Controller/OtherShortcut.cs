using ArisStudio.Core;
using UnityEngine;

namespace ArisStudio.UI
{
    public class OtherShortcut : MonoBehaviour
    {
        public GameObject settingArea,
            autoButtonDefault,
            autoButtonSelect;

        // DebugConsole debugConsole;

        void Awake()
        {
            // debugConsole = FindObjectOfType<DebugConsole>();
        }

        void Start()
        {
            // debugConsole = DebugConsole.Instance;
        }

        void Update()
        {
            if (DebugConsole.Instance.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    DebugConsole.Instance.RunCommand();
                }

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
                {
                    DebugConsole.Instance.gameObject.SetActive(false);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                settingArea.SetActive(!settingArea.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                DebugConsole.Instance.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (autoButtonDefault.activeSelf)
                {
                    MainControl.Instance.SetAuto(true);
                    autoButtonDefault.SetActive(false);
                    autoButtonSelect.SetActive(true);
                    return;
                }

                if (autoButtonSelect.activeSelf)
                {
                    MainControl.Instance.SetAuto(false);
                    autoButtonDefault.SetActive(true);
                    autoButtonSelect.SetActive(false);
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                MainControl.Instance.LoadTextData();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                MainControl.Instance.SetPlay();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }
    }
}
