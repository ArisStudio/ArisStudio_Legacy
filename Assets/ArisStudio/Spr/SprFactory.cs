using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Spr
{
    public class SprFactory : MonoBehaviour
    {
        public DebugConsole debugConsole;
        public Material defMaterial, commMaterial;

        private GameObject sprBaseGo;

        private string sprDataPath, charDataPath;
        private const float SprScale = 0.0136f;


        Dictionary<string, GameObject> ssList = new Dictionary<string, GameObject>();
        // Dictionary<string, GameObject> charList = new Dictionary<string, GameObject>();

        List<string> showList = new List<string>();

        private void Start()
        {
            sprBaseGo = GameObject.Find("SprBase").gameObject;
        }

        public void SetSprDataPath(string rootPath)
        {
            sprDataPath = Path.Combine(rootPath, "Spr");
            charDataPath = Path.Combine(rootPath, "Character");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Initialize()
        {
            foreach (var sprName in ssList)
            {
                Destroy(GameObject.Find(sprName.Key));
            }

            ssList.Clear();
        }

        public void CreateSprGameObjectWithDef(string nameId, string sprName)
        {
            StartCoroutine(LoadAndCreateSprGameObject(nameId, sprName, defMaterial));
        }

        public void CreateSprGameObjectWithComm(string nameId, string sprName)
        {
            StartCoroutine(LoadAndCreateSprGameObject(nameId, sprName, commMaterial));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadAndCreateSprGameObject(string nameId, string sprName, Material stateMaterial)
        {
            var sprBaseClone = Instantiate(sprBaseGo);
            sprBaseClone.name = nameId;
            var sprGo = sprBaseClone.transform.Find("SprObject").gameObject;

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

            var skeletonAnimation = SkeletonAnimation.AddToGameObject(sprGo, sprSkeletonDataAsset);

            skeletonAnimation.Initialize(false);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();

            try
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle_01", true);
            }
            catch
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "00", true);
            }

            sprGo.GetComponent<SprState>().SprInit();
            sprGo.GetComponent<OldSprEmotion>().InitEmoticon();

            ssList.Add(nameId, sprGo);

            debugConsole.PrintLog($"Load Spr: <color=lime>{nameId}</color>");

            sprGo.SetActive(false);
        }

        public void CreateCustomGameObjectWithDef(string nameId, float scale, string idle, string customName, string[] imgList)
        {
            StartCoroutine(LoadAndCreateCustomGameObject(nameId, scale, idle, customName, imgList, defMaterial));
        }

        public void CreateCustomGameObjectWithComm(string nameId, float scale, string idle, string customName, string[] imgList)
        {
            StartCoroutine(LoadAndCreateCustomGameObject(nameId, scale, idle, customName, imgList, commMaterial));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadAndCreateCustomGameObject(string nameId, float scale, string idle, string customName, string[] imgList,
            Material stateMaterial)
        {
            var sprBaseClone = Instantiate(sprBaseGo);
            sprBaseClone.name = nameId;
            var sprGo = sprBaseClone.transform.Find("SprObject").gameObject;

            # region LoadSpr

            var sprPath = Path.Combine(sprDataPath, customName);
            var atlasPath = $"{sprPath}.atlas";
            var skelPath = $"{sprPath}.skel";

            string atlasTxt;
            using (var uwr = UnityWebRequest.Get(atlasPath))
            {
                yield return uwr.SendWebRequest();
                atlasTxt = uwr.downloadHandler.text;
            }

            var atlasTextAsset = new TextAsset(atlasTxt);

            var textures = new Texture2D[imgList.Length];
            for (var i = 0; i < imgList.Length; i++)
            {
                byte[] imageData;
                var imgPath = Path.Combine(sprDataPath, imgList[i].Trim());
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

            var sprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, stateMaterial, true);

            var attachmentLoader = new AtlasAttachmentLoader(sprAtlasAsset.GetAtlas());
            var binary = new SkeletonBinary(attachmentLoader);
            binary.Scale *= SprScale * scale;

            byte[] skelData;
            using (var uwr = UnityWebRequest.Get(skelPath))
            {
                yield return uwr.SendWebRequest();
                skelData = uwr.downloadHandler.data;
            }

            var skeletonData = binary.ReadSkeletonData(customName, skelData);
            var stateData = new AnimationStateData(skeletonData);
            var sprSkeletonDataAsset = SkeletonDataAsset.CreateSkeletonDataAsset(skeletonData, stateData);

            # endregion

            var skeletonAnimation = SkeletonAnimation.AddToGameObject(sprGo, sprSkeletonDataAsset);

            skeletonAnimation.Initialize(false);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();


            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);

            sprGo.GetComponent<SprState>().SprInit();
            sprGo.GetComponent<OldSprEmotion>().InitEmoticon();

            ssList.Add(nameId, sprGo);

            debugConsole.PrintLog($"Load Spr: <color=lime>{nameId}</color>");

            sprGo.SetActive(false);
        }

        public void CreateCharGameObjectWithDef(string nameId, float scale, string charFolder, string[] charImgList)
        {
            StartCoroutine(LoadAndCreateCharacterGameObject(nameId, scale, charFolder, charImgList, defMaterial));
        }

        public void CreateCharGameObjectWithComm(string nameId, float scale, string charFolder, string[] charImgList)
        {
            StartCoroutine(LoadAndCreateCharacterGameObject(nameId, scale, charFolder, charImgList, commMaterial));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadAndCreateCharacterGameObject(string nameId, float scale, string charFolder, string[] charImgList,
            Material stateMaterial)
        {
            var charSpriteDic = new Dictionary<string, Sprite>();
            var charPath = Path.Combine(charDataPath, charFolder);

            var charBaseClone = Instantiate(sprBaseGo);
            charBaseClone.name = nameId;
            var charGo = charBaseClone.transform.Find("SprObject").gameObject;
            charGo.transform.localScale = new Vector3(scale, scale, 1);

            var charSprite = charGo.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            foreach (var i in charImgList)
            {
                var sImg = i.Trim();
                if (sImg == string.Empty) continue;

                byte[] imageData;
                using (var uwr = UnityWebRequest.Get($"{charPath}/{sImg}"))
                {
                    yield return uwr.SendWebRequest();
                    imageData = uwr.downloadHandler.data;
                }

                var texture = new Texture2D(1, 1);
                texture.LoadImage(imageData);
                charSpriteDic.Add(
                    sImg.Trim().Split('.')[0],
                    Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.4f)));
            }

            if (charSprite != null)
            {
                charSprite.sprite = charSpriteDic.First().Value;
                charSprite.material = stateMaterial;
            }

            charGo.GetComponent<SprState>().CharInit(charSpriteDic);
            charGo.GetComponent<OldSprEmotion>().InitEmoticon(scale);

            ssList.Add(nameId, charGo);
            charGo.SetActive(false);
        }

        public void TextWithHl(string nameId)
        {
            foreach (var s in showList)
            {
                var sprState = ssList[s].GetComponent<SprState>();
                if (sprState.IsComm) continue;
                if (s == nameId)
                {
                    sprState.HighLight(1);
                }
                else
                {
                    sprState.HighLight(0.5f);
                }
            }
        }

        public void SprCommand(string sprCommand)
        {
            if (sprCommand.StartsWith("spr"))
            {
                OldSprCommand(sprCommand);
            }
            else if (sprCommand.StartsWith("s"))
            {
                SCommand(sprCommand);
            }
        }

        private void SCommand(string sCommand)
        {
            var l = sCommand.Split(' ');

            switch (l[2])
            {
                // SprState
                case "show":
                {
                    ssList[l[1]].GetComponent<SprState>().Show();
                    showList.Add(l[1]);
                    break;
                }
                case "hide":
                {
                    ssList[l[1]].GetComponent<SprState>().Hide();
                    showList.Remove(l[1]);
                    break;
                }
                case "showD":
                {
                    ssList[l[1]].gameObject.SetActive(true);
                    showList.Add(l[1]);
                    break;
                }
                case "hideD":
                {
                    ssList[l[1]].gameObject.SetActive(false);
                    showList.Remove(l[1]);
                    break;
                }
                case "hl":
                {
                    ssList[l[1]].GetComponent<SprState>().HighLight(float.Parse(l[3]));
                    break;
                }
                case "state":
                {
                    ssList[l[1]].GetComponent<SprState>().SetState(l[3]);
                    break;
                }

                // SprEmotion
                case "emo":
                {
                    ssList[l[1]].GetComponent<OldSprEmotion>().PlayEmoticon(l[3]);
                    break;
                }
                case "emoInit":
                {
                    ssList[l[1]].GetComponent<OldSprEmotion>().InitEmoticon();
                    break;
                }

                // SprAnimation
                case "empty":
                {
                    ssList[l[1]].GetComponent<SprAnimation>().Empty();
                    break;
                }
                case "up":
                {
                    ssList[l[1]].GetComponent<SprAnimation>().Up();
                    break;
                }
                case "down":
                {
                    ssList[l[1]].GetComponent<SprAnimation>().Down();
                    break;
                }

                // SprMove
                case "x":
                {
                    ssList[l[1]].GetComponent<SprMove>().SetX(float.Parse(l[3]));
                    break;
                }
                case "y":
                {
                    ssList[l[1]].GetComponent<SprMove>().SetY(float.Parse(l[3]));
                    break;
                }
                case "z":
                {
                    ssList[l[1]].GetComponent<SprMove>().SetZ(float.Parse(l[3]));
                    break;
                }
                case "move":
                {
                    ssList[l[1]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveX":
                {
                    ssList[l[1]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveY":
                {
                    ssList[l[1]].GetComponent<SprMove>().Move2Y(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "close":
                {
                    ssList[l[1]].GetComponent<SprMove>().Close();
                    break;
                }
                case "back":
                {
                    ssList[l[1]].GetComponent<SprMove>().Back();
                    break;
                }
                case "shakeX":
                {
                    ssList[l[1]].GetComponent<SprMove>().ShakeX(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
                case "shakeY":
                {
                    ssList[l[1]].GetComponent<SprMove>().ShakeY(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void OldSprCommand(string sprCommand)
        {
            var l = sprCommand.Split(' ');
            switch (l[1])
            {
                // SprState
                case "show":
                {
                    ssList[l[2]].GetComponent<SprState>().Show();
                    showList.Add(l[2]);
                    break;
                }
                case "hide":
                {
                    ssList[l[2]].GetComponent<SprState>().Hide();
                    showList.Remove(l[2]);
                    break;
                }
                case "showD":
                {
                    ssList[l[2]].gameObject.SetActive(true);
                    showList.Add(l[2]);
                    break;
                }
                case "hideD":
                {
                    ssList[l[2]].gameObject.SetActive(false);
                    showList.Remove(l[2]);
                    break;
                }
                case "hl":
                {
                    ssList[l[2]].GetComponent<SprState>().HighLight(float.Parse(l[3]));
                    break;
                }
                case "state":
                {
                    ssList[l[2]].GetComponent<SprState>().SetState(l[3]);
                    break;
                }

                // SprEmotion
                case "emo":
                {
                    ssList[l[2]].GetComponent<OldSprEmotion>().PlayEmoticon(l[3]);
                    break;
                }

                // SprAnimation
                case "empty":
                {
                    ssList[l[2]].GetComponent<SprAnimation>().Empty();
                    break;
                }
                case "up":
                {
                    ssList[l[2]].GetComponent<SprAnimation>().Up();
                    break;
                }
                case "down":
                {
                    ssList[l[2]].GetComponent<SprAnimation>().Down();
                    break;
                }

                // SprMove
                case "x":
                {
                    ssList[l[2]].GetComponent<SprMove>().SetX(float.Parse(l[3]));
                    break;
                }
                case "y":
                {
                    ssList[l[2]].GetComponent<SprMove>().SetY(float.Parse(l[3]));
                    break;
                }
                case "z":
                {
                    ssList[l[2]].GetComponent<SprMove>().SetZ(float.Parse(l[3]));
                    break;
                }
                case "move":
                {
                    ssList[l[2]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveX":
                {
                    ssList[l[2]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveY":
                {
                    ssList[l[2]].GetComponent<SprMove>().Move2Y(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "close":
                {
                    ssList[l[2]].GetComponent<SprMove>().Close();
                    break;
                }
                case "back":
                {
                    ssList[l[2]].GetComponent<SprMove>().Back();
                    break;
                }
                case "shakeX":
                {
                    ssList[l[2]].GetComponent<SprMove>().ShakeX(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
                case "shakeY":
                {
                    ssList[l[2]].GetComponent<SprMove>().ShakeY(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
            }
        }
    }
}