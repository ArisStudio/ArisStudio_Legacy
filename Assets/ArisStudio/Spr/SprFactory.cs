using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.Core;
using ArisStudio.Utils;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Spr
{
    public class SprFactory : MonoBehaviour
    {
        public Material defMaterial, commMaterial;

        GameObject sprBaseGo;

        // string sprDataPath, charDataPath;
        const float SprScale = 0.0136f;


        Dictionary<string, GameObject> sprList = new Dictionary<string, GameObject>();
        // Dictionary<string, GameObject> charList = new Dictionary<string, GameObject>();

        List<string> showList = new List<string>();

        // DebugConsole debugConsole;
        SettingsManager settingsManager;

        private void Awake()
        {
            settingsManager = SettingsManager.Instance;
        }

        public void Start()
        {
            // debugConsole = DebugConsole.Instance;
            sprBaseGo = GameObject.Find("SprBase").gameObject;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Initialize()
        {
            foreach (KeyValuePair<string, GameObject> sprName in sprList)
                Destroy(GameObject.Find(sprName.Key));

            sprList.Clear();
        }


        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator LoadAndCreateCharacterGameObject(string nameId, float scale, string charFolder, string[] charImgList,
            Material stateMaterial)
        {
            Dictionary<string, Sprite> charSpriteDic = new Dictionary<string, Sprite>();
            string charPath = Path.Combine(settingsManager.currentCharacterPath, charFolder);

            GameObject charBaseClone = Instantiate(sprBaseGo);
            charBaseClone.name = nameId;
            GameObject charGo = charBaseClone.transform.Find("SprObject").gameObject;
            charGo.transform.localScale = new Vector3(scale, scale, 1);

            SpriteRenderer charSprite = charGo.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            foreach (string i in charImgList)
            {
                string sImg = i.Trim();
                if (sImg == string.Empty) continue;

                byte[] imageData;
                using (UnityWebRequest uwr = UnityWebRequest.Get($"{charPath}/{sImg}"))
                {
                    yield return uwr.SendWebRequest();
                    imageData = uwr.downloadHandler.data;
                }

                Texture2D texture = new Texture2D(1, 1);
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

            sprList.Add(nameId, charGo);
            charGo.SetActive(false);
        }


        #region Load Spr

        public void Spr_LoadCommand(string[] sprLoadCommand)
        {
            switch (sprLoadCommand[1])
            {
                case "spr":
                    switch (sprLoadCommand.Length)
                    {
                        // load spr hihumi hihumi_spr
                        case 4:
                            LoadSpr(sprLoadCommand[2], sprLoadCommand[3], false);
                            break;

                        // load spr h 1 Idle_01 someone_spr someone_spr.png, someone_spr2.png
                        case 7:
                            LoadSpr(sprLoadCommand[2], float.Parse(sprLoadCommand[3]), sprLoadCommand[4], sprLoadCommand[5],
                                sprLoadCommand[6].Split(','), false);
                            break;
                    }

                    break;

                case "spr_c":
                    switch (sprLoadCommand.Length)
                    {
                        case 4:
                            LoadSpr(sprLoadCommand[2], sprLoadCommand[3], true);
                            break;
                        case 7:
                            LoadSpr(sprLoadCommand[2], float.Parse(sprLoadCommand[3]), sprLoadCommand[4], sprLoadCommand[5],
                                sprLoadCommand[6].Split(','), true);
                            break;
                    }

                    break;
            }
        }

        private void LoadSpr(string nameId, string sprName, bool comm)
        {
            LoadSpr(nameId, 1, "Idle_01", sprName, new[] { $"{sprName}.png" }, comm);
        }

        private void LoadSpr(string nameId, float scale, string idle, string sprName, string[] imgList, bool comm)
        {
            StartCoroutine(CreateSpineGameObject(nameId, scale, idle, sprName, imgList, comm ? commMaterial : defMaterial));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CreateSpineGameObject(string nameId, float scale, string idle, string sprName, string[] imgList,
            Material stateMaterial)
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

            var sprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, stateMaterial, true);

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

            sprList.Add(nameId, sprGo);

            DebugConsole.Instance.PrintLoadLog("spr", sprName, nameId);

            sprGo.SetActive(false);
        }

        #endregion

        public void TextWithHl(string nameId)
        {
            foreach (string s in showList)
            {
                SprState sprState = sprList[s].GetComponent<SprState>();

                if (sprState.IsComm) continue;

                if (s == nameId)
                    sprState.HighLight(1);
                else
                    sprState.HighLight(0.5f);
            }
        }


        public void SprCommand(string sprCommand)
        {
            var l = sprCommand.Split(' ');

            switch (l[2])
            {
                // SprState
                case "show":
                {
                    sprList[l[1]].GetComponent<SprState>().Show();
                    showList.Add(l[1]);
                    break;
                }
                case "hide":
                {
                    sprList[l[1]].GetComponent<SprState>().Hide();
                    showList.Remove(l[1]);
                    break;
                }
                case "showD":
                {
                    sprList[l[1]].gameObject.SetActive(true);
                    showList.Add(l[1]);
                    break;
                }
                case "hideD":
                {
                    sprList[l[1]].gameObject.SetActive(false);
                    showList.Remove(l[1]);
                    break;
                }
                case "hl":
                {
                    sprList[l[1]].GetComponent<SprState>().HighLight(float.Parse(l[3]));
                    break;
                }
                case "state":
                {
                    sprList[l[1]].GetComponent<SprState>().SetState(l[3]);
                    break;
                }

                // SprEmotion
                case "emo":
                {
                    sprList[l[1]].GetComponent<OldSprEmotion>().PlayEmoticon(l[3]);
                    break;
                }
                case "emoInit":
                {
                    sprList[l[1]].GetComponent<OldSprEmotion>().InitEmoticon();
                    break;
                }

                // SprAnimation
                case "empty":
                {
                    sprList[l[1]].GetComponent<SprAnimation>().Empty();
                    break;
                }
                case "up":
                {
                    sprList[l[1]].GetComponent<SprAnimation>().Up();
                    break;
                }
                case "down":
                {
                    sprList[l[1]].GetComponent<SprAnimation>().Down();
                    break;
                }

                // SprMove
                case "x":
                {
                    sprList[l[1]].GetComponent<SprMove>().SetX(float.Parse(l[3]));
                    break;
                }
                case "y":
                {
                    sprList[l[1]].GetComponent<SprMove>().SetY(float.Parse(l[3]));
                    break;
                }
                case "z":
                {
                    sprList[l[1]].GetComponent<SprMove>().SetZ(float.Parse(l[3]));
                    break;
                }
                case "move":
                {
                    sprList[l[1]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveX":
                {
                    sprList[l[1]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveY":
                {
                    sprList[l[1]].GetComponent<SprMove>().Move2Y(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "close":
                {
                    sprList[l[1]].GetComponent<SprMove>().Close();
                    break;
                }
                case "back":
                {
                    sprList[l[1]].GetComponent<SprMove>().Back();
                    break;
                }
                case "shakeX":
                {
                    sprList[l[1]].GetComponent<SprMove>().ShakeX(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
                case "shakeY":
                {
                    sprList[l[1]].GetComponent<SprMove>().ShakeY(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
            }
        }
    }
}
