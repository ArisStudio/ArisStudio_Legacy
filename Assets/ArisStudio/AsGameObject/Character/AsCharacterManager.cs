using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArisStudio.Core;
using ArisStudio.Utils;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.AsGameObject.Character
{
    public class AsCharacterManager : MonoBehaviour
    {
        [Header("Material")] [SerializeField] private Material defaultMaterial, communicationMaterial;

        [Space] [SerializeField] private GameObject asCharacterBase;

        private const float SprScale = 0.0136f;

        private readonly Dictionary<string, AsCharacter> characterList = new Dictionary<string, AsCharacter>();
        private readonly List<string> showCharList = new List<string>();

        // DebugConsole debugConsole;
        private SettingsManager settingsManager;

        private void Awake()
        {
            settingsManager = SettingsManager.Instance;
        }

        public void AsCharacterInit()
        {
            foreach (var i in characterList) Destroy(i.Value.gameObject);

            characterList.Clear();

            showCharList.Clear();
        }

        #region Load Character

        // Todo: load with pure image

        public void AsCharacter_LoadCommand(string[] asCharLoadCommand, bool isCommunication)
        {
            // load spr/sprc hihumi hihumi_spr
            if (asCharLoadCommand.Length == 4) LoadSpineCharacter(asCharLoadCommand[2], asCharLoadCommand[3], isCommunication);

            // load spr/sprc h 1 Idle_01 someone_spr someone_spr.png, someone_spr2.png
            else
                LoadSpineCharacter(asCharLoadCommand[2], float.Parse(asCharLoadCommand[3]),
                    asCharLoadCommand[4], asCharLoadCommand[5],
                    asCharLoadCommand[6].Split(','), isCommunication);
        }

        private void LoadSpineCharacter(string nameId, string sprName, bool isCommunication)
        {
            LoadSpineCharacter(nameId, 1, "Idle_01", sprName, new[] { $"{sprName}.png" }, isCommunication);
        }

        private void LoadSpineCharacter(string nameId, float scale, string idle, string sprName, string[] imgList, bool isCommunication)
        {
            StartCoroutine(CreateSpineGameObject(nameId, scale, idle, sprName, imgList, isCommunication));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CreateSpineGameObject(string nameId, float scale, string idle, string sprName, string[] imgList,
            bool isCommunication)
        {
            var sprCharBaseClone = Instantiate(asCharacterBase);
            sprCharBaseClone.name = nameId;
            var sprCharGo = sprCharBaseClone.transform.Find("SprCharacter").gameObject;

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
            var asCharSkeletonDataAsset = SpineHelper.CreateRuntimeInstance(skeletonData, stateData);

            # endregion

            var skeletonAnimation = SkeletonAnimation.AddToGameObject(sprCharGo, asCharSkeletonDataAsset);

            skeletonAnimation.Initialize(false);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);

            AsCharacter asChar = AsCharacter.GetAsCharacterBehavior(sprCharGo);
            asChar.AsCharacterInit(isCommunication);

            characterList.Add(nameId, asChar);

            DebugConsole.Instance.PrintLoadLog($"{(isCommunication ? "spr" : "spr_c")}", sprName, nameId);
        }

        #endregion

        public void TextWithHighlight(string nameId)
        {
            foreach (var i in showCharList)
            {
                AsCharacter asChar = characterList[i];

                if (asChar.IsCommunication) continue;

                if (i == nameId) asChar.Highlight(0.5f);
                else asChar.Highlight(0);
            }
        }

        public void AsCharacterCommand(string[] asCharCommand)
        {
            switch (asCharCommand[2])
            {
                // Character state
                case "show":
                    characterList[asCharCommand[1]].Show();
                    showCharList.Add(asCharCommand[1]);
                    break;
                case "hide":
                    characterList[asCharCommand[1]].Hide();
                    showCharList.Remove(asCharCommand[1]);
                    break;
                case "showD": // legacy
                case "appear":
                    characterList[asCharCommand[1]].Appear();
                    showCharList.Add(asCharCommand[1]);
                    break;
                case "hideD": // legacy
                case "disappear":
                    characterList[asCharCommand[1]].Disappear();
                    showCharList.Remove(asCharCommand[1]);
                    break;

                case "hl":
                case "highlight":
                    characterList[asCharCommand[1]].Highlight(float.Parse(asCharCommand[3]));
                    break;

                case "state":
                    characterList[asCharCommand[1]].State(asCharCommand[3]);
                    break;
                case "skin":
                    characterList[asCharCommand[1]].Skin(asCharCommand[3]);
                    break;
                case "emo":
                case "emotion":
                    characterList[asCharCommand[1]].Emotion(asCharCommand[3]);
                    break;

                case "anim":
                case "animation":
                    characterList[asCharCommand[1]].Animation(asCharCommand[3]);
                    break;

                // Character Movement
                case "x":
                    characterList[asCharCommand[1]].X(float.Parse(asCharCommand[3]));
                    break;
                case "y":
                    characterList[asCharCommand[1]].Y(float.Parse(asCharCommand[3]));
                    break;
                case "z":
                    characterList[asCharCommand[1]].Z(float.Parse(asCharCommand[3]));
                    break;
                case "pos":
                case "position":
                    characterList[asCharCommand[1]].Position(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[4]));
                    break;

                case "moveX": // legacy
                case "xm":
                case "move":
                case "move_x":
                    if (asCharCommand.Length == 4)
                        characterList[asCharCommand[1]].MoveX(float.Parse(asCharCommand[3]));
                    else
                        characterList[asCharCommand[1]].MoveX(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[4]));
                    break;
                case "moveY": // legacy
                case "ym":
                case "move_y":
                    if (asCharCommand.Length == 4)
                        characterList[asCharCommand[1]].MoveY(float.Parse(asCharCommand[3]));
                    else
                        characterList[asCharCommand[1]].MoveY(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[4]));
                    break;
                case "pm":
                case "move_pos":
                case "move_position":
                    if (asCharCommand.Length == 5)
                        characterList[asCharCommand[1]].MovePosition(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[4]));
                    else
                        characterList[asCharCommand[1]]
                            .MovePosition(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[4]), float.Parse(asCharCommand[5]));
                    break;

                case "shakeX": // legacy
                case "xs":
                case "shake_x":
                    if (asCharCommand.Length == 4) characterList[asCharCommand[1]].ShakeX(float.Parse(asCharCommand[3]));
                    else characterList[asCharCommand[1]].ShakeX(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[4]));
                    break;

                case "shakeY": // legacy
                case "ys":
                case "shake_y":
                    if (asCharCommand.Length == 4) characterList[asCharCommand[1]].ShakeY(float.Parse(asCharCommand[3]));
                    else characterList[asCharCommand[1]].ShakeY(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[4]));
                    break;

                case "scale":
                    if (asCharCommand.Length == 3) characterList[asCharCommand[1]].Scale(float.Parse(asCharCommand[3]));
                    else characterList[asCharCommand[1]].Scale(float.Parse(asCharCommand[3]), float.Parse(asCharCommand[3]));
                    break;

                case "close":
                    characterList[asCharCommand[1]].Close();
                    break;

                case "back":
                    characterList[asCharCommand[1]].Back();
                    break;
            }
        }
    }
}
