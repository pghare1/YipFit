using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DownloadAudioFiles", menuName = "DLC/Create DownloadAudioFiles")]
public class DownloadAudioFiles : ScriptableObject
{


    [SerializeField] private List<AssetBundle> audioNameBundleDictionary;
    [SerializeField] private List<AssetBundle> audioDescriptionBundleDictionary;
    [SerializeField] private List<AssetBundle> motivationalBundleDictionary;
    [SerializeField] private List<AssetBundle> engagingBundleDictionary;
    [SerializeField] private List<AssetBundle> nonPerformingBundleDictionary;

    public List<AssetBundle> AudioNameBundleDictionary { get => audioNameBundleDictionary; set => audioNameBundleDictionary = value; }
    public List<AssetBundle> AudioDescriptionBundleDictionary { get => audioDescriptionBundleDictionary; set => audioDescriptionBundleDictionary = value; }
    public List<AssetBundle> MotivationalBundleDictionary { get => motivationalBundleDictionary; set => motivationalBundleDictionary = value; }
    public List<AssetBundle> EngagingBundleDictionary { get => engagingBundleDictionary; set => engagingBundleDictionary = value; }
    public List<AssetBundle> NonPerformingBundleDictionary { get => nonPerformingBundleDictionary; set => nonPerformingBundleDictionary = value; }
}
