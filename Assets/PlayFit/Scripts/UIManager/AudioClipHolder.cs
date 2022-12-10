using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioFileHolder", menuName = "Create Audio File Holder")]
public class AudioClipHolder : ScriptableObject
{
    public List<AudioClip> warmUpClips;
    public List<AudioClip> coreClips;
    public List<AudioClip> cooldownClips;
    public List<AudioClip> breakClips;
    public List<AudioClip> mainMenuClips;
    public List<AudioClip> resultScreenClips;
    public List<AudioClip> transitionPanel;

    public List<DialoguesClassification> dialoguesClassifications;
    private Dictionary<string, List<DialoguesClassification>> workoutDialoguesClassifications = new Dictionary<string, List<DialoguesClassification>>();

    public Dictionary<string, List<DialoguesClassification>> WorkoutDialoguesClassifications { get => workoutDialoguesClassifications; set => workoutDialoguesClassifications = value; }
}

[System.Serializable]
public class DialoguesClassification
{
    public string audioName;
    public AudioClip dialoguesClip;
    public enum DialogueType { optionsPanelDialogues, startCinematicsDialogues, resultScreenDialogues, nextActionDialogue, actionDescriptionDialogue, actionNameDialgue,
        engagingDialogue, motivationalDialogue, nonPerformingDialogue
    }
    public DialogueType dialogueType;


    public DialoguesClassification(string audioName, AudioClip dialoguesClip, DialogueType dialogueType)
    {
        this.audioName = audioName;
        this.dialoguesClip = dialoguesClip;
        this.dialogueType = dialogueType;
    }

}




