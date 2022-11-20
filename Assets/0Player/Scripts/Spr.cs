using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Spr
{
    SpineAtlasAsset runtimeAtlasAsset;
    SkeletonDataAsset runtimeSkeletonDataAsset;
    SkeletonAnimation runtimeSkeletonAnimation;

    string atlasPath, skelPath, sprPath, atlasTxt;
    byte[] imageData;

    public IEnumerator CreateNewSpineGameObject(string nameId, string sprName)
    {
        sprPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Data", "Spr", sprName);
        atlasPath = sprPath + ".atlas.prefab";
        skelPath = sprPath + ".skel.prefab";

        using (UnityWebRequest uwr = UnityWebRequest.Get(atlasPath))
        {
            yield return uwr.SendWebRequest();
            atlasTxt = uwr.downloadHandler.text;
        }

        TextAsset atlasTextAsset = new TextAsset(atlasTxt);

        Texture2D[] textures = new Texture2D[1];
        Texture2D texture = new Texture2D(1, 1);

        using (UnityWebRequest uwr = UnityWebRequest.Get(sprPath + ".png"))
        {
            yield return uwr.SendWebRequest();
            imageData = uwr.downloadHandler.data;
        }

        texture.LoadImage(imageData);
        texture.name = sprName;
        textures[0] = texture;

        runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, Shader.Find("SFill"), true);

        AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(runtimeAtlasAsset.GetAtlas());
        SkeletonBinary binary = new SkeletonBinary(attachmentLoader);

        binary.Scale *= 0.012f;
        SkeletonData skeletonData = binary.ReadSkeletonData(skelPath);

        AnimationStateData stateData = new AnimationStateData(skeletonData);

        runtimeSkeletonDataAsset = SkeletonDataAsset.CreateSkeletonDataAsset(skeletonData, stateData);
        runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(nameId, runtimeSkeletonDataAsset);

        runtimeSkeletonAnimation.Initialize(false);
        runtimeSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
        runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "Idle_01", true);
        //runtimeSkeletonAnimation.AnimationState.SetAnimation(1, "00", false);
        runtimeSkeletonAnimation.transform.Translate(Vector3.down * 9);

        runtimeSkeletonAnimation.gameObject.AddComponent<SprState>();
        runtimeSkeletonAnimation.gameObject.SetActive(false);
    }

    public SkeletonAnimation LoadSpr(string nameId, string sprName)
    {
        CreateNewSpineGameObject(nameId, sprName);

        return runtimeSkeletonAnimation;
    }
}
