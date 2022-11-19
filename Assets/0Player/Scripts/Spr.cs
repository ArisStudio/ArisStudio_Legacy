using Spine;
using Spine.Unity;
using System.IO;
using UnityEngine;

public class Spr
{
    SpineAtlasAsset runtimeAtlasAsset;
    SkeletonDataAsset runtimeSkeletonDataAsset;
    SkeletonAnimation runtimeSkeletonAnimation;

    string atlasPath, skelPath, sprPath;

    public void CreateNewSpineGameObject(string nameId, string sprName)
    {
        sprPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Data", "Spr", sprName);
        atlasPath = sprPath + ".atlas.prefab";
        skelPath = sprPath + ".skel.prefab";

        string atlasTxt = File.ReadAllText(atlasPath);
        TextAsset atlasTextAsset = new TextAsset(atlasTxt);

        Texture2D[] textures = new Texture2D[1];
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(File.ReadAllBytes(sprPath + ".png"));
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
    }

    public SkeletonAnimation LoadSpr(string nameId, string sprName)
    {
        CreateNewSpineGameObject(nameId, sprName);

        runtimeSkeletonAnimation.Initialize(false);
        runtimeSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
        runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "Idle_01", true);
        //runtimeSkeletonAnimation.AnimationState.SetAnimation(1, "00", false);
        runtimeSkeletonAnimation.transform.Translate(Vector3.down * 9);

        runtimeSkeletonAnimation.gameObject.AddComponent<SprState>();
        runtimeSkeletonAnimation.gameObject.SetActive(false);

        return runtimeSkeletonAnimation;
    }
}
