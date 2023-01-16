using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ArisStudio.Image
{
    public class ImageFactory : MonoBehaviour
    {
        public DebugConsole debugConsole;

        [Header("Background")] public RawImage backgroundImage;
        public ImageShake bgShake;

        [Header("Cover")] public RawImage coverImage;

        private string backgroundDataPath, coverDataPath;

        private float bgShowTimer;
        private const float BgChangeShowTime = 0.5f;
        private bool bgShowing, bgHiding;


        Dictionary<string, Texture2D> backgroundList = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> coverList = new Dictionary<string, Texture2D>();

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

        public void SetImageDataPath(string rootPath)
        {
            var imageDataPath = Path.Combine(rootPath, "Image");
            backgroundDataPath = Path.Combine(imageDataPath, "Background");
            coverDataPath = Path.Combine(imageDataPath, "Cover");
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
                    var imagePath = Path.Combine(backgroundDataPath, imageName);
                    var www = UnityWebRequest.Get(imagePath);
                    yield return www.SendWebRequest();
                    imageData = www.downloadHandler.data;
                    texture.LoadImage(imageData);
                    backgroundList.Add(nameId, texture);
                    break;
                }
                case "Cover":
                {
                    var imagePath = Path.Combine(coverDataPath, imageName);
                    var www = UnityWebRequest.Get(imagePath);
                    yield return www.SendWebRequest();
                    imageData = www.downloadHandler.data;
                    texture.LoadImage(imageData);
                    coverList.Add(nameId, texture);
                    break;
                }
            }

            debugConsole.PrintLog($"Loaded {imageType}: <color=lime>{imageName}</color>");
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
                            var bgTmp = backgroundList[l[2]];
                            backgroundImage.rectTransform.sizeDelta = new Vector2(bgTmp.width, bgTmp.height) * 1.6f;
                            backgroundImage.texture = bgTmp;
                            backgroundImage.color = new Color(1, 1, 1, 0);
                            bgShowing = true;
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