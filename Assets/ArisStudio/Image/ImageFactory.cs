using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArisStudio.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    public class ImageFactory : MonoBehaviour
    {
        [Header("Background")] public RawImage backgroundImage;
        public ImageShake bgShake;

        [Header("Cover")] public RawImage coverImage;

        // private string backgroundDataPath, coverDataPath;

        private float bgShowTimer;
        private const float BgChangeShowTime = 0.5f;
        private bool bgShowing, bgHiding;


        Dictionary<string, Texture2D> backgroundList = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> coverList = new Dictionary<string, Texture2D>();

        // DebugConsole debugConsole;

        void Awake()
        {
        }

        void Start()
        {
            // debugConsole = DebugConsole.Instance;
        }

        private void Update()
        {
            if (bgShowing)
            {
                bgShowTimer += Time.deltaTime;
                if (bgShowTimer >= BgChangeShowTime || bgShowTimer / BgChangeShowTime >= 0.95f)
                {
                    bgShowTimer = 0;
                    backgroundImage.color = new Color(1, 1, 1, 1);
                    bgShowing = false;
                }
                else
                {
                    backgroundImage.color = new Color(1, 1, 1, bgShowTimer / BgChangeShowTime);
                }
            }

            else if (bgHiding)
            {
                bgShowTimer += Time.deltaTime;
                if (bgShowTimer >= BgChangeShowTime || bgShowTimer / BgChangeShowTime >= 0.95f)
                {
                    bgShowTimer = 0;
                    backgroundImage.color = new Color(1, 1, 1, 0);
                    bgHiding = false;
                }
                else
                {
                    backgroundImage.color = new Color(1, 1, 1, 1 - bgShowTimer / BgChangeShowTime);
                }
            }
        }

        public void Initialize()
        {
            backgroundImage.color = Color.black;
            backgroundList.Clear();

            coverImage.gameObject.SetActive(false);
            coverList.Clear();
        }

        #region Load Image

        public void LoadBackground(string nameId, string bgName)
        {
            StartCoroutine(LoadImage(nameId, bgName, "Background"));
        }

        public void LoadCover(string nameId, string coverName)
        {
            StartCoroutine(LoadImage(nameId, coverName, "Cover"));
        }


        private IEnumerator LoadImage(string nameId, string imageName, string imageType)
        {
            byte[] imageData;
            var texture = new Texture2D(1, 1);

            switch (imageType)
            {
                case "Background":
                {
                    string imagePath = Path.Combine(SettingsManager.Instance.currentBackgroundPath, imageName);
                    UnityWebRequest www = UnityWebRequest.Get(imagePath);
                    yield return www.SendWebRequest();
                    imageData = www.downloadHandler.data;
                    texture.LoadImage(imageData);
                    backgroundList.Add(nameId, texture);
                    break;
                }
                case "Cover":
                {
                    var imagePath = Path.Combine(SettingsManager.Instance.currentCoverPath, imageName);
                    var www = UnityWebRequest.Get(imagePath);
                    yield return www.SendWebRequest();
                    imageData = www.downloadHandler.data;
                    texture.LoadImage(imageData);
                    coverList.Add(nameId, texture);
                    break;
                }
            }

            DebugConsole.Instance.PrintLog($"Load {imageType}: <#00ff00>{imageName}</color>");
        }

        #endregion

        public void ImageCommand(string imageCommand)
        {
            var l = imageCommand.Split(' ');
            switch (l[0])
            {
                case "bg":
                    switch (l[1])
                    {
                        case "set":
                            var bgSetTmp = backgroundList[l[2]];
                            backgroundImage.rectTransform.sizeDelta = new Vector2(bgSetTmp.width, bgSetTmp.height) * 1.6f;
                            backgroundImage.texture = bgSetTmp;
                            backgroundImage.color = new Color(1, 1, 1, 0);
                            bgShowing = true;
                            break;

                        case "change":
                            var bgChangeTmp = backgroundList[l[2]];
                            backgroundImage.rectTransform.sizeDelta = new Vector2(bgChangeTmp.width, bgChangeTmp.height) * 1.6f;
                            backgroundImage.texture = bgChangeTmp;
                            break;

                        case "show":
                            backgroundImage.color = new Color(1, 1, 1, 0);
                            bgShowing = true;
                            break;
                        case "hide":
                            bgHiding = true;
                            break;

                        case "showD":
                            backgroundImage.color = new Color(1, 1, 1, 1);
                            backgroundImage.gameObject.SetActive(true);
                            break;
                        case "hideD":
                            backgroundImage.gameObject.SetActive(false);
                            break;

                        case "shakeX":
                            bgShake.ShakeX(float.Parse(l[2]), float.Parse(l[3]), float.Parse(l[4]));
                            break;
                        case "shakeY":
                            bgShake.ShakeY(float.Parse(l[2]), float.Parse(l[3]), float.Parse(l[4]));
                            break;
                    }

                    break;

                case "cover":
                    switch (l[1])
                    {
                        case "set":
                            var coverTmp = coverList[l[2]];
                            coverImage.rectTransform.sizeDelta = new Vector2(coverTmp.width, coverTmp.height) / 1.5f;
                            coverImage.texture = coverTmp;
                            coverImage.gameObject.SetActive(true);
                            break;
                        case "show":
                            coverImage.gameObject.SetActive(true);
                            break;
                        case "hide":
                            coverImage.gameObject.SetActive(false);
                            break;
                    }

                    break;
            }
        }
    }
}
