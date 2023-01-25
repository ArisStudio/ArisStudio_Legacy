using System.Collections;
using System.Collections.Generic;
using System.IO;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Spr
{
    public class SprFactory : MonoBehaviour
    {
        public DebugConsole debugConsole;


        private GameObject sprBaseGo, emoBaseGo;
        private Shader defShader, commShader;

        private string sprDataPath;
        private const float SprScale = 0.0136f;


        Dictionary<string, SkeletonAnimation> sprList = new Dictionary<string, SkeletonAnimation>();

        List<string> showList = new List<string>();

        private void Start()
        {
            defShader = Shader.Find("SFill");
            commShader = Shader.Find("Comm");

            sprBaseGo = GameObject.Find("SprBase").gameObject;
            emoBaseGo = GameObject.Find("EmotionBase").gameObject;
        }

        public void SetSprDataPath(string rootPath)
        {
            sprDataPath = Path.Combine(rootPath, "Spr");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Initialize()
        {
            foreach (var sprName in sprList)
            {
                Destroy(GameObject.Find(sprName.Key));
            }

            sprList.Clear();
        }

        public void CreateSprGameObjectWithDef(string nameId, string sprName)
        {
            StartCoroutine(LoadAndCreateSprGameObject(nameId, sprName, defShader));
        }

        public void CreateSprGameObjectWithComm(string nameId, string sprName)
        {
            StartCoroutine(LoadAndCreateSprGameObject(nameId, sprName, commShader));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadAndCreateSprGameObject(string nameId, string sprName, Shader stateShader)
        {
            var sprBaseClone = Instantiate(sprBaseGo);
            sprBaseClone.name = nameId;
            var sprGo = sprBaseClone.transform.Find("SprObject").gameObject;

            var emoClone = Instantiate(emoBaseGo, sprGo.transform, false);
            emoClone.name = "Emotion";

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
            texture.name = sprName;
            var textures = new Texture2D[1];
            textures[0] = texture;

            var sprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, stateShader, true);

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

            sprGo.GetComponent<SprState>().Init();
            sprGo.GetComponent<OldSprEmotion>().InitEmoticon();

            sprList.Add(nameId, skeletonAnimation);

            debugConsole.PrintLog($"Load Spr: <color=lime>{nameId}</color>");

            sprGo.SetActive(false);
        }

        public void TextWithHl(string nameId)
        {
            foreach (var s in showList)
            {
                if (s == nameId)
                {
                    sprList[s].GetComponent<SprState>().HighLight(1);
                }
                else
                {
                    sprList[s].GetComponent<SprState>().HighLight(0.5f);
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

        // ReSharper disable Unity.PerformanceAnalysis
        private void OldSprCommand(string sprCommand)
        {
            var l = sprCommand.Split(' ');
            switch (l[1])
            {
                // SprState
                case "show":
                {
                    sprList[l[2]].GetComponent<SprState>().Show();
                    showList.Add(l[2]);
                    break;
                }
                case "hide":
                {
                    sprList[l[2]].GetComponent<SprState>().Hide();
                    showList.Remove(l[2]);
                    break;
                }
                case "showD":
                {
                    sprList[l[2]].gameObject.SetActive(true);
                    showList.Add(l[2]);
                    break;
                }
                case "hideD":
                {
                    sprList[l[2]].gameObject.SetActive(false);
                    showList.Remove(l[2]);
                    break;
                }
                case "hl":
                {
                    sprList[l[2]].GetComponent<SprState>().HighLight(float.Parse(l[3]));
                    break;
                }
                case "state":
                {
                    sprList[l[2]].GetComponent<SprState>().SetState(l[3]);
                    break;
                }

                // SprEmotion
                case "emo":
                {
                    sprList[l[2]].GetComponent<OldSprEmotion>().PlayEmoticon(l[3]);
                    break;
                }

                // SprAnimation
                case "empty":
                {
                    sprList[l[2]].GetComponent<SprAnimation>().Empty();
                    break;
                }
                case "up":
                {
                    sprList[l[2]].GetComponent<SprAnimation>().Up();
                    break;
                }
                case "down":
                {
                    sprList[l[2]].GetComponent<SprAnimation>().Down();
                    break;
                }

                // SprMove
                case "x":
                {
                    sprList[l[2]].GetComponent<SprMove>().SetX(float.Parse(l[3]));
                    break;
                }
                case "y":
                {
                    sprList[l[2]].GetComponent<SprMove>().SetY(float.Parse(l[3]));
                    break;
                }
                case "z":
                {
                    sprList[l[2]].GetComponent<SprMove>().SetZ(float.Parse(l[3]));
                    break;
                }
                case "move":
                {
                    sprList[l[2]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveX":
                {
                    sprList[l[2]].GetComponent<SprMove>().Move2X(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "moveY":
                {
                    sprList[l[2]].GetComponent<SprMove>().Move2Y(float.Parse(l[3]), float.Parse(l[4]));
                    break;
                }
                case "close":
                {
                    sprList[l[2]].GetComponent<SprMove>().Close();
                    break;
                }
                case "back":
                {
                    sprList[l[2]].GetComponent<SprMove>().Back();
                    break;
                }
                case "shakeX":
                {
                    sprList[l[2]].GetComponent<SprMove>().ShakeX(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
                case "shakeY":
                {
                    sprList[l[2]].GetComponent<SprMove>().ShakeY(float.Parse(l[3]), float.Parse(l[4]), float.Parse(l[5]));
                    break;
                }
            }
        }
    }
}