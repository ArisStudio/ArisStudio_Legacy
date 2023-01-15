using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Image
{
    public class ImageFactory : MonoBehaviour
    {
        public DebugConsole debugConsole;

        private string backgroundDataPath, coverDataPath;


        Dictionary<string, Texture2D> backgroundList = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> coverList = new Dictionary<string, Texture2D>();

        public void SetImageDataPath(string rootPath)
        {
            var imageDataPath = Path.Combine(rootPath, "Image");
            backgroundDataPath = Path.Combine(imageDataPath, "Background");
            coverDataPath = Path.Combine(imageDataPath, "Cover");
        }

        public void Initialize()
        {
            backgroundList.Clear();
            coverList.Clear();
        }


        public void LoadBackground(string nameId, string bgName)
        {
            StartCoroutine(LoadImage(nameId, bgName, "Background"));
        }

        public void LoadCover(string nameId, string coverName)
        {
            StartCoroutine(LoadImage(nameId, coverName, "Cover"));
        }


        IEnumerator LoadImage(string nameId, string imageName, string imageType)
        {
            byte[] imageData;
            Texture2D texture = new Texture2D(1, 1);

            if (imageType == "Background")
            {
                var imagePath = Path.Combine(backgroundDataPath, imageName);
                var www = UnityWebRequest.Get(imagePath);
                yield return www.SendWebRequest();
                imageData = www.downloadHandler.data;
                texture.LoadImage(imageData);
                backgroundList.Add(nameId, texture);
            }

            else if (imageType == "Cover")
            {
                var imagePath = Path.Combine(coverDataPath, imageName);
                var www = UnityWebRequest.Get(imagePath);
                yield return www.SendWebRequest();
                imageData = www.downloadHandler.data;
                texture.LoadImage(imageData);
                coverList.Add(nameId, texture);
            }

            debugConsole.PrintLog($"Loaded {imageType}: <color=lime>{imageName}</color>");
        }

        public void ImageCommand(string imageCommand)
        {
        }
    }
}