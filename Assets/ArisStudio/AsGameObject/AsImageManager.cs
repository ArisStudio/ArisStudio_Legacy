using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArisStudio.AsGameObject.Image;
using ArisStudio.Core;
using ArisStudio.Utils;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ArisStudio.AsGameObject
{
    public class AsImageManager : MonoBehaviour
    {
        [SerializeField] private GameObject foregroundListBase;
        [SerializeField] private GameObject midgroundListBase;
        [SerializeField] private GameObject backgroundListBase;

        private SettingsManager settingsManager;
        private DebugConsole debugConsole;

        private readonly Dictionary<string, AsImage> imageList = new Dictionary<string, AsImage>();

        private const float BehaviourDuration = 0.5f;

        private void Awake()
        {
            settingsManager = SettingsManager.Instance;
            debugConsole = DebugConsole.Instance;
        }

        public void AsImageInit()
        {
            foreach (var i in imageList) Destroy(i.Value.gameObject);
            imageList.Clear();

            debugConsole.PrintLog("AsImageInit");
        }

        # region Load Image

        public void LoadAsImage(string[] asImageLoadCommand, string imageType)
        {
            StartCoroutine(CreateAsImage(asImageLoadCommand[2], asImageLoadCommand[3], imageType));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CreateAsImage(string nameId, string imageName, string imageType)
        {
            GameObject imageListBase;
            string imagePath;

            switch (imageType)
            {
                case "fg":
                case "foreground":
                    imageListBase = foregroundListBase;
                    imagePath = settingsManager.currentForegroundPath;
                    break;
                case "md":
                case "midground":
                    imageListBase = midgroundListBase;
                    imagePath = settingsManager.currentMidgroundPath;
                    break;
                default:
                    imageListBase = backgroundListBase;
                    imagePath = settingsManager.currentBackgroundPath;
                    break;
            }

            var imageGo = new GameObject(nameId).AddComponent<RawImage>();
            imageGo.transform.SetParent(imageListBase.transform);

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(Path.Combine(imagePath, imageName));
            yield return www.SendWebRequest();

            var asImage = AsImage.GetAsImage(imageGo.gameObject);
            asImage.AsImageInit(DownloadHandlerTexture.GetContent(www));
            imageList.Add(nameId, asImage);

            debugConsole.PrintLoadLog(imageType, nameId, imageName);
        }

        # endregion

        public void AsImageCommand(string[] asImageCommand)
        {
            switch (asImageCommand[2])
            {
                // Image State
                case "show":
                    imageList[asImageCommand[1]].Show();
                    break;
                case "hide":
                    imageList[asImageCommand[1]].Hide();
                    break;
                case "showD": // Legacy
                case "appear":
                    imageList[asImageCommand[1]].Appear();
                    break;
                case "hideD": // Legacy
                case "disappear":
                    imageList[asImageCommand[1]].Disappear();
                    break;

                case "hl":
                case "highlight":
                    // TODO: image highlight
                    break;

                case "fade":
                    imageList[asImageCommand[1]].Fade(
                        float.Parse(asImageCommand[3]),
                        ArrayHelper.IsIndexInRange(asImageCommand, 4) ? float.Parse(asImageCommand[4]) : 0
                    );
                    break;

                // Image Movement
                case "x":
                    imageList[asImageCommand[1]].X(float.Parse(asImageCommand[3]));
                    break;
                case "y":
                    imageList[asImageCommand[1]].Y(float.Parse(asImageCommand[3]));
                    break;
                case "z":
                    imageList[asImageCommand[1]].Z(float.Parse(asImageCommand[3]));
                    break;
                case "p":
                case "pos":
                case "position":
                    imageList[asImageCommand[1]].Position(float.Parse(asImageCommand[3]), float.Parse(asImageCommand[4]));
                    break;

                case "moveX": // legacy
                case "xm":
                case "move":
                case "move_x":
                    imageList[asImageCommand[1]].MoveX(
                        float.Parse(asImageCommand[3]),
                        ArrayHelper.IsIndexInRange(asImageCommand, 4) ? float.Parse(asImageCommand[4]) : BehaviourDuration
                    );
                    break;
                case "moveY": // legacy
                case "ym":
                case "move_y":
                    imageList[asImageCommand[1]].MoveY(
                        float.Parse(asImageCommand[3]),
                        ArrayHelper.IsIndexInRange(asImageCommand, 4) ? float.Parse(asImageCommand[4]) : BehaviourDuration
                    );
                    break;
                case "pm":
                case "move_pos":
                case "move_position":
                    imageList[asImageCommand[1]].MovePosition(
                        float.Parse(asImageCommand[3]),
                        float.Parse(asImageCommand[4]),
                        ArrayHelper.IsIndexInRange(asImageCommand, 5) ? float.Parse(asImageCommand[5]) : BehaviourDuration
                    );
                    break;

                case "shakeX": // legacy
                case "xs":
                case "shake_x":
                    imageList[asImageCommand[1]].ShakeX(
                        float.Parse(asImageCommand[3]),
                        ArrayHelper.IsIndexInRange(asImageCommand, 4) ? float.Parse(asImageCommand[4]) : BehaviourDuration
                    );
                    break;
                case "shakeY": // legacy
                case "ys":
                case "shake_y":
                    imageList[asImageCommand[1]].ShakeY(
                        float.Parse(asImageCommand[3]),
                        ArrayHelper.IsIndexInRange(asImageCommand, 4) ? float.Parse(asImageCommand[4]) : BehaviourDuration
                    );
                    break;

                case "scale":
                    imageList[asImageCommand[1]].Scale(
                        float.Parse(asImageCommand[3]),
                        ArrayHelper.IsIndexInRange(asImageCommand, 4) ? float.Parse(asImageCommand[4]) : BehaviourDuration
                    );
                    break;
            }
        }
    }
}
