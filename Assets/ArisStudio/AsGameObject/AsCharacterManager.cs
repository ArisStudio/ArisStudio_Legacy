﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArisStudio.AsGameObject.Character;
using ArisStudio.Core;
using ArisStudio.Utils;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.AsGameObject
{
    public class AsCharacterManager : Singleton<AsCharacterManager>
    {
        [SerializeField]
        private GameObject asCharacterBase;
        private Material defaultMaterial,
            communicationMaterial;

        private const float SprScale = 0.0136f;

        private readonly Dictionary<string, AsCharacter> characterList =
            new Dictionary<string, AsCharacter>();
        private readonly List<string> showCharList = new List<string>();

        private SettingsManager settingsManager;
        private DebugConsole debugConsole;

        private const float BehaviourDuration = 0.5f;
        private const int DefaultVibrato = 6;

        private const int DefaultTrackIndex = 1;

        void Awake()
        {
            settingsManager = SettingsManager.Instance;
            debugConsole = DebugConsole.Instance;
        }

        private void Start()
        {
            defaultMaterial = Resources.Load<Material>("Materials/AsCharacter_Default");
            communicationMaterial = Resources.Load<Material>("Materials/AsCharacter_Communication");
        }

        public void AsCharacterInit()
        {
            foreach (AsCharacter i in characterList.Values)
                Destroy(i.transform.parent.gameObject);

            characterList.Clear();
            showCharList.Clear();

            debugConsole.PrintLog("AsCharacterInit");
        }

        #region Load Character

        // Todo: load with pure image

        public void LoadAsCharacter(string[] asCharLoadCommand, bool isCommunication)
        {
            // load spr/sprc hihumi hihumi_spr
            if (asCharLoadCommand.Length == 4)
                LoadSpineCharacter(asCharLoadCommand[2], asCharLoadCommand[3], isCommunication);
            // load spr/sprc h 1 Idle_01 someone_spr someone_spr.png,someone_spr2.png
            else
                LoadSpineCharacter(
                    asCharLoadCommand[2],
                    float.Parse(asCharLoadCommand[3]),
                    asCharLoadCommand[4],
                    asCharLoadCommand[5],
                    asCharLoadCommand[6].Split(','),
                    isCommunication
                );
        }

        private void LoadSpineCharacter(string nameId, string sprName, bool isCommunication)
        {
            LoadSpineCharacter(
                nameId,
                1,
                "Idle_01",
                sprName,
                new[] { $"{sprName}.png" },
                isCommunication
            );
        }

        private void LoadSpineCharacter(
            string nameId,
            float scale,
            string idle,
            string sprName,
            string[] imgList,
            bool isCommunication
        )
        {
            StartCoroutine(
                CreateSpineAsCharacter(nameId, scale, idle, sprName, imgList, isCommunication)
            );
        }

        private IEnumerator CreateSpineAsCharacter(
            string nameId,
            float scale,
            string idle,
            string sprName,
            string[] imgList,
            bool isCommunication
        )
        {
            GameObject sprCharBaseClone = Instantiate(
                asCharacterBase,
                asCharacterBase.transform.parent,
                true
            );
            sprCharBaseClone.name = nameId;
            GameObject sprCharGo = sprCharBaseClone.transform.Find("AsCharacter").gameObject;

            string sprPath = Path.Combine(settingsManager.currentSprPath, sprName);
            string atlasPath = $"{sprPath}.atlas";
            string skelPath = $"{sprPath}.skel";

            # region Load Atlas

            string atlasTxt;
            using (UnityWebRequest uwr = UnityWebRequest.Get(atlasPath))
            {
                yield return uwr.SendWebRequest();
                atlasTxt = uwr.downloadHandler.text;
            }

            TextAsset atlasTextAsset = new TextAsset(atlasTxt);

            #endregion

            # region Load Textures

            Texture2D[] textures = new Texture2D[imgList.Length];
            for (int i = 0; i < imgList.Length; i++)
            {
                byte[] imageData;
                string imgPath = Path.Combine(settingsManager.currentSprPath, imgList[i].Trim());
                using (UnityWebRequest uwr = UnityWebRequest.Get(imgPath))
                {
                    yield return uwr.SendWebRequest();
                    imageData = uwr.downloadHandler.data;
                }

                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(imageData);
                texture.name = Path.GetFileNameWithoutExtension(imgPath);
                textures[i] = texture;
            }

            SpineAtlasAsset sprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(
                atlasTextAsset,
                textures,
                isCommunication ? communicationMaterial : defaultMaterial,
                true
            );

            #endregion

            # region Load Skeleton

            AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(
                sprAtlasAsset.GetAtlas()
            );
            SkeletonBinary binary = new SkeletonBinary(attachmentLoader);
            binary.Scale *= SprScale * scale;

            byte[] skelData;
            using (UnityWebRequest uwr = UnityWebRequest.Get(skelPath))
            {
                yield return uwr.SendWebRequest();
                skelData = uwr.downloadHandler.data;
            }

            Stream skelStream = new MemoryStream(skelData);

            SkeletonData skeletonData = binary.ReadSkeletonData(skelStream);
            skeletonData.Name = Path.GetFileNameWithoutExtension(skelPath);
            AnimationStateData stateData = new AnimationStateData(skeletonData);
            SkeletonDataAsset asCharSkeletonDataAsset = SpineHelper.CreateRuntimeInstance(
                skeletonData,
                stateData
            );

            # endregion

            SkeletonAnimation skeletonAnimation = SkeletonAnimation.AddToGameObject(
                sprCharGo,
                asCharSkeletonDataAsset
            );

            skeletonAnimation.Initialize(false);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);

            AsCharacter asChar = AsCharacter.GetAsCharacter(sprCharGo);
            asChar.AsCharacterInit(isCommunication);

            characterList.Add(nameId, asChar);

            debugConsole.PrintLoadLog($"{(isCommunication ? "spr_c" : "spr")}", sprName, nameId);
        }

        #endregion

        public void TextWithHighlight(string nameId)
        {
            foreach (string i in showCharList)
            {
                AsCharacter asChar = characterList[i];

                if (asChar.IsCommunication)
                    continue;

                if (i == nameId)
                    asChar.Highlight(0.5f, 0);
                else
                    asChar.Highlight(0, 0);
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
                    characterList[asCharCommand[1]].Highlight(
                        float.Parse(asCharCommand[3]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? float.Parse(asCharCommand[4])
                            : 0
                    );
                    break;

                case "fade":
                    // TODO: character fade
                    break;

                case "state":
                    characterList[asCharCommand[1]].State(
                        asCharCommand[3],
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? int.Parse(asCharCommand[4])
                            : DefaultTrackIndex,
                        !ArrayHelper.IsIndexInRange(asCharCommand, 5)
                            || bool.Parse(asCharCommand[5])
                    );
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
                case "p":
                case "pos":
                case "position":
                    characterList[asCharCommand[1]].Position(
                        float.Parse(asCharCommand[3]),
                        float.Parse(asCharCommand[4])
                    );
                    break;

                case "moveX": // legacy
                case "xm":
                case "move":
                case "move_x":
                    characterList[asCharCommand[1]].MoveX(
                        float.Parse(asCharCommand[3]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? float.Parse(asCharCommand[4])
                            : BehaviourDuration
                    );
                    break;
                case "moveY": // legacy
                case "ym":
                case "move_y":
                    characterList[asCharCommand[1]].MoveY(
                        float.Parse(asCharCommand[3]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? float.Parse(asCharCommand[4])
                            : BehaviourDuration
                    );
                    break;
                case "pm":
                case "move_pos":
                case "move_position":
                    characterList[asCharCommand[1]].MovePosition(
                        float.Parse(asCharCommand[3]),
                        float.Parse(asCharCommand[4]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 5)
                            ? float.Parse(asCharCommand[5])
                            : BehaviourDuration
                    );
                    break;

                case "shakeX": // legacy
                case "xs":
                case "shake_x":
                    characterList[asCharCommand[1]].ShakeX(
                        float.Parse(asCharCommand[3]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? float.Parse(asCharCommand[4])
                            : BehaviourDuration,
                        ArrayHelper.IsIndexInRange(asCharCommand, 5)
                            ? int.Parse(asCharCommand[5])
                            : DefaultVibrato
                    );
                    break;
                case "shakeY": // legacy
                case "ys":
                case "shake_y":
                    characterList[asCharCommand[1]].ShakeY(
                        float.Parse(asCharCommand[3]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? float.Parse(asCharCommand[4])
                            : BehaviourDuration,
                        ArrayHelper.IsIndexInRange(asCharCommand, 5)
                            ? int.Parse(asCharCommand[5])
                            : DefaultVibrato
                    );
                    break;
                case "shake":
                    characterList[asCharCommand[1]].Shake(
                        float.Parse(asCharCommand[3]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? float.Parse(asCharCommand[4])
                            : BehaviourDuration,
                        ArrayHelper.IsIndexInRange(asCharCommand, 5)
                            ? int.Parse(asCharCommand[5])
                            : DefaultVibrato
                    );
                    break;

                case "scale":
                    characterList[asCharCommand[1]].Scale(
                        float.Parse(asCharCommand[3]),
                        ArrayHelper.IsIndexInRange(asCharCommand, 4)
                            ? float.Parse(asCharCommand[4])
                            : 0
                    );
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
