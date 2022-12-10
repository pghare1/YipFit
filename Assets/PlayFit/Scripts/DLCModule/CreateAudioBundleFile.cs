using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CreateAudioBundleFile
{
    [MenuItem("Assets/ Generate All Audio Bundles")]
    static void GenerateAudioBundleFiles()
    {
        BuildPipeline.BuildAssetBundles("Assets/Resources", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
#endif
