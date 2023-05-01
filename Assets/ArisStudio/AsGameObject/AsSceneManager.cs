using ArisStudio.Core;
using ArisStudio.ScreenEffect;
using ArisStudio.Utils;
using UnityEngine;

namespace ArisStudio.AsGameObject
{
    public class AsSceneManager : Singleton<AsSceneManager>
    {
        [SerializeField] private FocusLine focusLine;
        [SerializeField] private GameObject smokeFore, smokeBack, snow, rain, dust;

        public void AsSceneInit()
        {
            focusLine.enabled = false;
            smokeFore.SetActive(false);
            smokeBack.SetActive(false);
            snow.SetActive(false);
            rain.SetActive(false);
            dust.SetActive(false);
        }

        public void AsSceneCommand(string[] asSceneCommand)
        {
            switch (asSceneCommand[1])
            {
                case "focus":
                    focusLine.enabled = asSceneCommand[2] == "show";
                    break;

                case "smoke":
                    smokeFore.SetActive(asSceneCommand[2] == "show");
                    smokeBack.SetActive(asSceneCommand[2] == "show");
                    break;

                case "snow":
                    snow.SetActive(asSceneCommand[2] == "show");
                    break;

                case "rain":
                    rain.SetActive(asSceneCommand[2] == "show");
                    break;

                case "dust":
                    dust.SetActive(asSceneCommand[2] == "show");
                    break;

                default:
                    DebugConsole.Instance.PrintLog("AsSceneCommand: " + asSceneCommand[1] + " is not a valid command.");
                    break;
            }
        }
    }
}
