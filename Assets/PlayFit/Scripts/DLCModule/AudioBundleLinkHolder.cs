using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioBundleLinkHolder", menuName = "DLC/Create AudioBundleLinkHolder")]
public class AudioBundleLinkHolder : ScriptableObject
{
    [SerializeField] private List<string> actionNameSoundURL;
    [SerializeField] private List<string> actionRelatedDialoguesURL;
    [SerializeField] private List<string> nonPerformingDialoguesURL;
    [SerializeField] private List<string> sfxEngagingAudioURL;
    [SerializeField] private List<string> motivationalAudioURL;

    public List<string> ActionNameSoundURL { get => actionNameSoundURL; set => actionNameSoundURL = value; }
    public List<string> ActionRelatedDialoguesURL { get => actionRelatedDialoguesURL; set => actionRelatedDialoguesURL = value; }
    public List<string> NonPerformingDialoguesURL { get => nonPerformingDialoguesURL; set => nonPerformingDialoguesURL = value; }
    public List<string> SfxEngagingAudioURL { get => sfxEngagingAudioURL; set => sfxEngagingAudioURL = value; }
    public List<string> MotivationalAudioURL { get => motivationalAudioURL; set => motivationalAudioURL = value; }
}
