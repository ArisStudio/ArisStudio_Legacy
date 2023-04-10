using System;
using System.Collections;
using System.IO;
using ArisStudio.Core;
using ArisStudio.Spr;
using ArisStudio.Utils;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Character
{
    public class AsCharacter : AsCharacterBehavior
    {
        [Header("Material")] [SerializeField] Material defaultMaterial, communicationMaterial;

        [Space] [SerializeField] GameObject sprBaseGo;

        private const float SprScale = 0.0136f;

        // DebugConsole debugConsole;
        private SettingsManager settingsManager;

        private void Awake()
        {
            settingsManager = SettingsManager.Instance;
        }

        public AsCharacter(string nameId, string sprName, bool isCommunication) :
            this(nameId, 1, "Idle_01", sprName, new[] { $"{sprName}.png" }, isCommunication)
        {
        }

        public AsCharacter(string nameId, float scale, string idle, string sprName, string[] imgList, bool isCommunication)
        {
            StartCoroutine(CreateSpineGameObject(nameId, scale, idle, sprName, imgList, isCommunication));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CreateSpineGameObject(string nameId, float scale, string idle, string sprName, string[] imgList,
            bool isCommunication)
        {
            var sprBaseClone = Instantiate(sprBaseGo);
            sprBaseClone.name = nameId;
            var sprGo = sprBaseClone.transform.Find("SprObject").gameObject;

            var sprPath = Path.Combine(settingsManager.currentSprPath, sprName);
            var atlasPath = $"{sprPath}.atlas";
            var skelPath = $"{sprPath}.skel";

            # region Load Atlas

            string atlasTxt;
            using (var uwr = UnityWebRequest.Get(atlasPath))
            {
                yield return uwr.SendWebRequest();
                atlasTxt = uwr.downloadHandler.text;
            }

            var atlasTextAsset = new TextAsset(atlasTxt);

            #endregion

            # region Load Textures

            var textures = new Texture2D[imgList.Length];
            for (var i = 0; i < imgList.Length; i++)
            {
                byte[] imageData;
                var imgPath = Path.Combine(settingsManager.currentSprPath, imgList[i].Trim());
                using (var uwr = UnityWebRequest.Get(imgPath))
                {
                    yield return uwr.SendWebRequest();
                    imageData = uwr.downloadHandler.data;
                }

                var texture = new Texture2D(1, 1);
                texture.LoadImage(imageData);
                texture.name = Path.GetFileNameWithoutExtension(imgPath);
                textures[i] = texture;
            }

            IsCommunication = isCommunication;

            var sprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures,
                isCommunication ? communicationMaterial : defaultMaterial, true);

            #endregion

            # region Load Skeleton

            var attachmentLoader = new AtlasAttachmentLoader(sprAtlasAsset.GetAtlas());
            var binary = new SkeletonBinary(attachmentLoader);
            binary.Scale *= SprScale * scale;

            byte[] skelData;
            using (var uwr = UnityWebRequest.Get(skelPath))
            {
                yield return uwr.SendWebRequest();
                skelData = uwr.downloadHandler.data;
            }

            Stream skelStream = new MemoryStream(skelData);

            var skeletonData = binary.ReadSkeletonData(skelStream);
            skeletonData.Name = Path.GetFileNameWithoutExtension(skelPath);
            var stateData = new AnimationStateData(skeletonData);
            var sprSkeletonDataAsset = SpineHelper.CreateRuntimeInstance(skeletonData, stateData);

            # endregion

            var skeletonAnimation = SkeletonAnimation.AddToGameObject(sprGo, sprSkeletonDataAsset);

            skeletonAnimation.Initialize(false);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();

            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);

            sprGo.GetComponent<SprState>().SprInit();
            sprGo.GetComponent<OldSprEmotion>().InitEmoticon();

            DebugConsole.Instance.PrintLoadLog("spr", sprName, nameId);

            sprGo.SetActive(false);
        }
    }
}
