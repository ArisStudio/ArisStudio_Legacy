using System;
using System.Reflection;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace ArisStudio.Utils
{
    public static class SpineHelper
    {
        public static SkeletonDataAsset CreateRuntimeInstance(SkeletonData skeletonData, AnimationStateData stateData)
        {
            // Create a new instance of SkeletonDataAsset
            SkeletonDataAsset skeletonDataAsset = ScriptableObject.CreateInstance<SkeletonDataAsset>();

            // Get the type of SkeletonDataAsset
            Type skeletonDataAssetType = skeletonDataAsset.GetType();

            // Get the skeletonData and stateData fields
            FieldInfo skeletonDataField = skeletonDataAssetType.GetField("skeletonData", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo stateDataField = skeletonDataAssetType.GetField("stateData", BindingFlags.NonPublic | BindingFlags.Instance);

            // Set the values of skeletonData and stateData
            skeletonDataField.SetValue(skeletonDataAsset, skeletonData);
            stateDataField.SetValue(skeletonDataAsset, stateData);

            // Set a dummy value to skeletonJSON to avoid unexpected errors.
            skeletonDataAsset.skeletonJSON = new TextAsset("Aris Studio");

            // Return the SkeletonDataAsset
            return skeletonDataAsset;
        }
    }
}
