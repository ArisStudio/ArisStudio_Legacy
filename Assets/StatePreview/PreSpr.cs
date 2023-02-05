using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StatePreview
{
    public class PreSpr : MonoBehaviour
    {
        public Text stateText;

        public GameObject sprBaseGo;
        public Material defMaterial;
        public InputField sprNameInputField;

        private string sprGoName;

        private const float SprScale = 0.015f;
        private string sprDataPath;
        private SkeletonAnimation skeletonAnimation;

        private string cutDataPath;

        private List<Texture2D> cutTextures = new List<Texture2D>();

        private void Start()
        {
            var rootPath = Directory.GetParent(Application.dataPath).ToString();
            var localDataPath = Path.Combine(rootPath, "Data");
            sprDataPath = Path.Combine(localDataPath, "Spr");

            cutDataPath = Path.Combine(localDataPath, "Cut");
            if (Directory.Exists(cutDataPath) == false)
            {
                Directory.CreateDirectory(cutDataPath);
            }
        }

        public void ReturnArisStudio()
        {
            Screen.SetResolution(1280, 720, false);
            SceneManager.LoadScene("ArisStudio");
        }

        public void GenSpr()
        {
            cutTextures.Clear();
            StartCoroutine(GenPng());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator GenPng()
        {
            var anim = skeletonAnimation.Skeleton.Data.Animations;
            foreach (var a in anim)
            {
                stateText.text = a.Name;
                skeletonAnimation.AnimationState.SetAnimation(0, a, false);
                yield return new WaitForEndOfFrame();
                var tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
                tex.ReadPixels(new Rect(Vector2.zero, new Vector2(256, 256)), 0, 0);
                tex.Apply();

                cutTextures.Add(tex);
            }

            // 拼接图片 6列
            var cutTexture = new Texture2D(256 * 6, 256 * (cutTextures.Count / 6 + 1), TextureFormat.RGBA32, false);
            for (var i = 0; i < cutTextures.Count; i++)
            {
                var x = i % 6;
                var y = i / 6;
                var rect = new Rect(x * 256, y * 256, 256, 256);
                cutTexture.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, cutTextures[i].GetPixels());
            }

            cutTexture.Apply();

            var bytes = cutTexture.EncodeToPNG();
            var cutPath = Path.Combine(cutDataPath, $"{sprGoName}.png");
            File.WriteAllBytes(cutPath, bytes);
        }

        public void CreateSprGameObjectWithDef()
        {
            if (!string.IsNullOrEmpty(sprGoName))
            {
                Destroy(GameObject.Find(sprGoName));
            }

            StartCoroutine(LoadAndCreateSprGameObject(sprNameInputField.text, defMaterial));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadAndCreateSprGameObject(string sprName, Material stateMaterial)
        {
            sprGoName = sprName;

            var sprBaseClone = Instantiate(sprBaseGo);
            sprBaseClone.name = sprName;

            # region LoadSpr

            var sprPath = Path.Combine(sprDataPath, sprName);
            var atlasPath = $"{sprPath}.atlas";
            var skelPath = $"{sprPath}.skel";

            string atlasTxt;
            using (var uwr = UnityWebRequest.Get(atlasPath))
            {
                yield return uwr.SendWebRequest();
                atlasTxt = uwr.downloadHandler.text;
            }

            var atlasTextAsset = new TextAsset(atlasTxt);

            byte[] imageData;
            using (var uwr = UnityWebRequest.Get($"{sprPath}.png"))
            {
                yield return uwr.SendWebRequest();
                imageData = uwr.downloadHandler.data;
            }

            var texture = new Texture2D(1, 1);
            texture.LoadImage(imageData);
            texture.name = Path.GetFileNameWithoutExtension($"{sprPath}.png");
            var textures = new Texture2D[1];
            textures[0] = texture;

            var sprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, stateMaterial, true);

            var attachmentLoader = new AtlasAttachmentLoader(sprAtlasAsset.GetAtlas());
            var binary = new SkeletonBinary(attachmentLoader);
            binary.Scale *= SprScale;

            byte[] skelData;
            using (var uwr = UnityWebRequest.Get(skelPath))
            {
                yield return uwr.SendWebRequest();
                skelData = uwr.downloadHandler.data;
            }

            var skeletonData = binary.ReadSkeletonData(sprName, skelData);
            var stateData = new AnimationStateData(skeletonData);
            var sprSkeletonDataAsset = SkeletonDataAsset.CreateSkeletonDataAsset(skeletonData, stateData);

            # endregion

            skeletonAnimation = SkeletonAnimation.AddToGameObject(sprBaseClone, sprSkeletonDataAsset);

            skeletonAnimation.Initialize(false);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();

            skeletonAnimation.gameObject.transform.position = new Vector3(-6, -16, 0);
        }
    }
}