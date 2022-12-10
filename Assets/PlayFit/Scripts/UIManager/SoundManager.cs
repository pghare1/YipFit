using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using UnityEngine.Networking;

public class SoundManager : MonoBehaviour
{
    public AudioSource dialogueAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClipHolder audioClipHolder;
    [SerializeField] AudioMixerGroup diloguesMixer;
    [SerializeField] AudioMixerGroup bgMusicMixer;
    [SerializeField] AudioMixer master;
    public CharacterAnimationHandler characterAnimationHandler;
    DialoguesClassification selectedAudio = null;
    public AudioClip audio;
    public bool isPlayingEngagingSound = false;
    List<AudioClip> selectedEngagingAudios;
    List<AudioClip> actionRelatedAudios;
    List<AudioClip> breakRelatedAudios;
    List<AudioClip> actionNameAudios;
    List<AudioClip> nonPerformingAudios;
    public int nonperformingCount = 0;

    public int lastSelectedRandom = 0;

    const string actionNameSound = "action-name-dialogues", actionRelatedDialogues = "action-related-dialogues", nonPerformingDialogues = "non-performing-dialogues",
    sfxEngagingAudio = "action-engaging-dialogues", motivationalSound = "motivational-dialogues";

    private void Start()
    {
        //EnableDisableDialogueMixer(true);
        //PlayEngagingSound("CD");
    }

    //Select mixer According to the Requirement
    public void PlayAudioClip(AudioClip clip, bool isDialgoueSound, bool playWithLoop)
    {
        if (clip == null)
            return;

        AudioSource audioSource = null;
        if (isDialgoueSound)
        {
            audioSource = dialogueAudioSource;
            audioSource.outputAudioMixerGroup = diloguesMixer;
        }
        else
        {
            audioSource = musicAudioSource;
            audioSource.outputAudioMixerGroup = bgMusicMixer;
        }

        audioSource.clip = clip;
        audioSource.loop = playWithLoop;
        audioSource.Play();
    }

    //Select Audio list from main list of audios as per specified type of dialogue
    public void SelectDialgoue(DialoguesClassification.DialogueType type)
    {
        
        List<DialoguesClassification> specificClassifiedAudios = new List<DialoguesClassification>();
        foreach (DialoguesClassification item in audioClipHolder.dialoguesClassifications)
        {
            if(item.dialogueType == type)
            {
                specificClassifiedAudios.Add(item);
            }
        }
        if (specificClassifiedAudios != null || specificClassifiedAudios.Count <= 0)
        {
            selectedAudio = specificClassifiedAudios[Random.Range(0, specificClassifiedAudios.Count)];
        }
        if(FindObjectOfType<PredefinedWorkoutManager>().workoutConfig.backgroundMusicSetting)
            StartCoroutine(ResetMusicSoundAfterDialogue(selectedAudio.dialoguesClip));
        
    }

    public void PlayAudioDialogue(int index)
    {
        
        List<DialoguesClassification> specificClassifiedAudios = new List<DialoguesClassification>();
        foreach (DialoguesClassification item in audioClipHolder.dialoguesClassifications)
        {
            if (item.dialogueType == DialoguesClassification.DialogueType.optionsPanelDialogues)
            {
                specificClassifiedAudios.Add(item);
            }
        }
        selectedAudio = specificClassifiedAudios[index];
        if (FindObjectOfType<PredefinedWorkoutManager>().workoutConfig.backgroundMusicSetting)
            StartCoroutine(ResetMusicSoundAfterDialogue(selectedAudio.dialoguesClip));
    }


    private IEnumerator ResetMusicSoundAfterDialogue(AudioClip clip)
    {
        LowerDownBGMUsicWhenDialogueOn(false);
        PlayAudioClip(selectedAudio.dialoguesClip, true, false);
        yield return new WaitForSeconds(clip.length);
        LowerDownBGMUsicWhenDialogueOn(true);

    }

    //warm up background music selected
    public void WarmUpBg()
    {
        AudioClip clip = audioClipHolder.warmUpClips[Random.Range(0, audioClipHolder.warmUpClips.Count)];
        PlayAudioClip(clip, false, true);
    }

    //Core music selected for core section
    public void CoreBg()
    {
        AudioClip clip = audioClipHolder.coreClips[Random.Range(0, audioClipHolder.coreClips.Count)];
        PlayAudioClip(clip, false, true);
    }

    //Cool down music selected 
    public void CooldownBg()
    {
        AudioClip clip = audioClipHolder.cooldownClips[Random.Range(0, audioClipHolder.cooldownClips.Count)];
        PlayAudioClip(clip, false, true);
    }

    public void BreakBg()
    {
        AudioClip clip = audioClipHolder.cooldownClips[Random.Range(0, audioClipHolder.breakClips.Count)];
        PlayAudioClip(clip, false, true);
    }

    public void MainMenuBg()
    {
        AudioClip clip = audioClipHolder.mainMenuClips[Random.Range(0, audioClipHolder.mainMenuClips.Count)];
        PlayAudioClip(clip, false, true);
    }

    public void ResultScreenMusic()
    {
        //AudioClip clip = audioClipHolder.resultScreenClips[0];
        //PlayAudioClip(clip, false, true);
    }

    public void TransitionPanelBg()
    {
        AudioClip clip = audioClipHolder.transitionPanel[Random.Range(0, audioClipHolder.cooldownClips.Count)];
        PlayAudioClip(clip, false, true);
    }

    //Enable and disable Dialogue mixer
    public void EnableDisableDialogueMixer(bool turnOn)
    {

        float val;
        master.GetFloat("Dialogue", out val);
        if (!turnOn)
        {
            
            master.SetFloat("Dialogue", -80f);
        }
        else
        {
            
            master.SetFloat("Dialogue", 0f);
        }
    }


    public void EnableDialogueMixer()
    {

        if(FindObjectOfType<PredefinedWorkoutManager>().workoutConfig.dialoguesSettings)
        {
            master.SetFloat("Dialogue", 0f);
        }
    }
    //Enable and disable BGMusic Mixer
    public void EnableDisableBGMusicMixer(bool turnOn)
    {
        float val;
        master.GetFloat("BGMusic", out val);
        if (!turnOn)
        {
            
            master.SetFloat("BGMusic", -80f);
        }
        else
        {
            
            master.SetFloat("BGMusic", 0f);
        }
    }

    public void EnableDisableMasterMixer(bool turnOn)
    {
        float val;
        master.GetFloat("MasterVolume", out val);
        if (!turnOn)
        {

            master.SetFloat("MasterVolume", -80f);
        }
        else
        {
            //StartCoroutine(ReduceAudioSound("MasterVolume", 0.001f, false));
            master.SetFloat("MasterVolume", 0f);
        }
    }

    public void LowerDownBGMUsicWhenDialogueOn(bool turnOn)
    {
        if (FindObjectOfType<UIManager>().isBgMusicOn)
        {
           // float val = 0f;
            if (!turnOn)
            {
                StartCoroutine(ReduceAudioSound("BGMusic", -10f, true));
            }
            else
            {

                //master.SetFloat("BGMusic", Mathf.Lerp(val, 0f, Time.deltaTime));
                StartCoroutine(ReduceAudioSound("BGMusic", -20f, true));
            }
        }
    }

    public void PlayActionNameSound(string actionid)
    {

        PlayAudioClip(characterAnimationHandler.GetVoiceOverClip(actionid), true, false);

        
    }

    private IEnumerator ReduceAudioSound(string mixerName, float volume, bool makeNegative)
    {
        float val = 0f;
        Debug.Log("Sound mixer name : " + mixerName);
        Debug.Log("Sound Val : " + val);

        if (volume <= 0f)
        {
            val = -10f;
            while (val != -18f)
            {

                master.SetFloat(mixerName, val);
                val--;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            val = -10f;
            while (val != 0f)
            {
                master.SetFloat(mixerName, val);
                val++;
                yield return new WaitForSeconds(0.05f);
            }
        }

        
    }

    //According to Workout Type Warmup, core, Cooldown Different Engaging Voice overs are Played
    //Engaging Audio Format F/M_EngagingSound_W/C/S_Version.mp3 W- Warmup, C-Core, S-CoolDown
    //make final list according to workout type
    //Choose randomly from list and play
    public void PlayEngagingSound(string workoutType)
    {

        //DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/SFX_Engaging_Audio");
        Debug.LogError("<SM> Dic Count engagingAudioFileNames : " + audioClipHolder.WorkoutDialoguesClassifications[sfxEngagingAudio].Count);
        List<DialoguesClassification> engagingAudioFileNames = audioClipHolder.WorkoutDialoguesClassifications[sfxEngagingAudio];
        Debug.LogError("<SM> engagingAudioFileNames : " + engagingAudioFileNames.Count);
        selectedEngagingAudios = new List<AudioClip>();
        //LowerDownBGMUsicWhenDialogueOn(false);//
        foreach (DialoguesClassification file in engagingAudioFileNames)
        {   //Workout Type is at position [2] in array
            string[] str = file.audioName.Split('_');
            Debug.LogError("workout Type : " + workoutType.ToLower() + "  str[1] " + str[1]);
            if (str[1] == workoutType.ToLower())//Format Changed In audio files
            {
                selectedEngagingAudios.Add(file.dialoguesClip);
            }
        }
        Debug.LogError("<SM> Received selectedEngagingAudios : " + selectedEngagingAudios.Count);
        AudioClip playSelectedAudioFromList = selectedEngagingAudios[SelectRandomIndex()]; //selectedEngagingAudios[0];
        Debug.LogError("<SM> Received EngagingAudiosFileName : " + playSelectedAudioFromList.name);
        StartCoroutine(StreamAudioFileForEngagingSound(playSelectedAudioFromList));
        //LowerDownBGMUsicWhenDialogueOn(true);
        
    }

    public void PlayActionSound(string actionId)
    {   //
        //if (!FindObjectOfType<PredefinedWorkoutManager>().inNonPerformingVoices)
        //{  
        // All Action Related Dialogues are In Folder streamingAssets/ActionRelatedDialogues 
        //Naming Format For Action Related Dialogues is F/M_ActionId_Version.mp3
        //Selecting all Action related files from streaming Assets folder 
        //Split all file names to check whether the actionId from file name matches with the current selected action Id
        //If matched add audio file to list, after adding all Audio files matching the same action id choose randomly and play 
        //DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Action_Related_Dialogues");
        //FileInfo[] engagingAudioFileNames = directoryInfo.GetFiles("*.mp3");
        Debug.LogError("<SM> Dic Count actionRelatedDialoguesAudio : " + audioClipHolder.WorkoutDialoguesClassifications[actionRelatedDialogues].Count);
        List<DialoguesClassification> actionRelatedDialoguesAudio = audioClipHolder.WorkoutDialoguesClassifications[actionRelatedDialogues];
        Debug.LogError("<SM> Action Related Audio Len : " + actionRelatedDialoguesAudio.Count);
            actionRelatedAudios = new List<AudioClip>();
            foreach (DialoguesClassification file in actionRelatedDialoguesAudio)
            {
                string[] str = file.audioName.Split('_');
                if (str[1] == actionId.ToLower())
                {
                    actionRelatedAudios.Add(file.dialoguesClip);
                }
            }
        Debug.LogError("<SM> Received actionRelatedAudios : " + actionRelatedAudios.Count);
            if (actionRelatedAudios.Count > 0)
            {
                AudioClip playSelectedAudioFromList = actionRelatedAudios[Random.Range(0, actionRelatedAudios.Count)];
                //LowerDownBGMUsicWhenDialogueOn(false);//
                StartCoroutine(StreamAudioFileFromDictionary(playSelectedAudioFromList));
            }
        //}
    }

    public void PlayBreakMotivationalSound()
    {
        Debug.LogError("<SM> Dic Count motivationalSound : " + audioClipHolder.WorkoutDialoguesClassifications[motivationalSound].Count);
        if (!FindObjectOfType<PredefinedWorkoutManager>().inNonPerformingVoices)
        {
            List<DialoguesClassification> engagingAudioFileNames = audioClipHolder.WorkoutDialoguesClassifications[motivationalSound];
            Debug.LogError("<SM> engagingAudioFileNames : " + engagingAudioFileNames.Count);
            breakRelatedAudios = new List<AudioClip>();
            foreach (DialoguesClassification file in engagingAudioFileNames)
            {
                
                
                    breakRelatedAudios.Add(file.dialoguesClip);
                
            }
            Debug.LogError("<SM> Received breakRelatedAudios : " + breakRelatedAudios.Count);
            if (breakRelatedAudios.Count > 0)
            {
                AudioClip playSelectedAudioFromList = breakRelatedAudios[Random.Range(0, breakRelatedAudios.Count)];
                //LowerDownBGMUsicWhenDialogueOn(false);//
                StartCoroutine(StreamAudioFileFromDictionary(playSelectedAudioFromList));
            }
        }
    }

    public void PlayActionNameAudio(string actionId)
    {
        Debug.LogError("<SM> Dic Count actionNameSound : " + audioClipHolder.WorkoutDialoguesClassifications[actionNameSound].Count);
        List<DialoguesClassification> actionNameAudioFiles = audioClipHolder.WorkoutDialoguesClassifications[actionNameSound];
        Debug.LogError("<SM> actionNameAudioFiles : " + actionNameAudioFiles.Count);
        actionNameAudios = new List<AudioClip>();
        foreach (DialoguesClassification file in actionNameAudioFiles)
        {
            string[] str = file.audioName.Split('_');
            if (str[1] == actionId.ToLower())
            {
                actionNameAudios.Add(file.dialoguesClip);
            }
        }
        Debug.LogError("<SM> Received actionNameAudios : " + actionNameAudios.Count);
        if (actionNameAudios.Count > 0)
        {
            AudioClip playSelectedAudioFromList = actionNameAudios[0];
            StartCoroutine(StreamAudioFileFromDictionary(playSelectedAudioFromList));
        }
    }


    //Read all files from StreamingAssetsFolder/Non_performing_Dialogues
    //Make list of Non Performing Dialogues 
    // Choose randomly from list and Play
    public void PlayNonPerformActionSound()
    {
        Debug.LogError("<SM> Dic Count nonPerformingDialogues : " + audioClipHolder.WorkoutDialoguesClassifications[nonPerformingDialogues].Count);
        if (!FindObjectOfType<PredefinedWorkoutManager>().audioEngagingVoicePlayed || !FindObjectOfType<PredefinedWorkoutManager>().actionNameAudioPlayed)
        {
            Debug.LogError("<SM> Dic Count nonPerformingDialogues : " + audioClipHolder.WorkoutDialoguesClassifications[nonPerformingDialogues].Count);
            List<DialoguesClassification> engagingAudioFileNames = audioClipHolder.WorkoutDialoguesClassifications[nonPerformingDialogues];
            Debug.LogError("<SM> engagingAudioFileNames : " + engagingAudioFileNames.Count);
            nonPerformingAudios = new List<AudioClip>();
            foreach (DialoguesClassification file in engagingAudioFileNames)
            {
                nonPerformingAudios.Add(file.dialoguesClip);
            }
            Debug.LogError("<SM> Received nonPerformingAudios : " + nonPerformingAudios.Count);
            if (nonPerformingAudios.Count > 0)
            {
                AudioClip playSelectedAudioFromList = nonPerformingAudios[Random.Range(0, nonPerformingAudios.Count)];
                StartCoroutine(StreamAudioFileForEngagingSound(playSelectedAudioFromList));
            }
        }

    }

    private IEnumerator StreamAudioFile(string audioFileName)
    {
        Debug.LogError("<SM> Audio File Name : " + audioFileName);
        //Local reading from file (Will be changed in dlc)
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioFileName, AudioType.MPEG);
        yield return request.SendWebRequest();
        audio = DownloadHandlerAudioClip.GetContent(request);
        //Current Audio Clip Selected
        FindObjectOfType<PredefinedWorkoutManager>().currentVoiceOverClip = audio;
        LowerDownBGMUsicWhenDialogueOn(false);//
        PlayAudioClip(audio, true, false);// Audio clip, isThisDialogueSound, loop
        yield return new WaitForSeconds(audio.length);
        LowerDownBGMUsicWhenDialogueOn(true);//Lower Background Music When Dialogues are on to make dialogues Audible
    }

    private IEnumerator StreamAudioFileFromDictionary(AudioClip audioFileName)
    {
        Debug.LogError("<SM> Received Audio File clip : " + audioFileName.name);
        Debug.LogError("<SM> Received Audio File clip Len : " + audioFileName.length);
        audio = audioFileName;
        //Current Audio Clip Selected
        FindObjectOfType<PredefinedWorkoutManager>().currentVoiceOverClip = audio;
        LowerDownBGMUsicWhenDialogueOn(false);//
        PlayAudioClip(audio, true, false);// Audio clip, isThisDialogueSound, loop
        yield return new WaitForSeconds(audio.length);
        LowerDownBGMUsicWhenDialogueOn(true);//Lower Background Music When Dialogues are on to make dialogues Audible
    }

    private IEnumerator StreamAudioFileForEngagingSound(string audioFileName)
    {
        Debug.LogError("<SM> Audio File Name : " + audioFileName);
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioFileName, AudioType.MPEG);
        yield return request.SendWebRequest();
        audio = DownloadHandlerAudioClip.GetContent(request);
        FindObjectOfType<PredefinedWorkoutManager>().currentVoiceOverClip = audio;
        LowerDownBGMUsicWhenDialogueOn(false);//
        PlayAudioClip(audio, true, false);
        isPlayingEngagingSound = true;
        FindObjectOfType<PredefinedWorkoutManager>().audioEngagingVoicePlayed = true;
        yield return new WaitForSeconds(audio.length);//
        nonperformingCount++;
        Debug.Log("<SM> nonperformingCount :" + nonperformingCount);
        LowerDownBGMUsicWhenDialogueOn(true);
        isPlayingEngagingSound = false;
        FindObjectOfType<PredefinedWorkoutManager>().audioEngagingVoicePlayed = false;
    }

    private IEnumerator StreamAudioFileForEngagingSound(AudioClip audioFileName)
    {
        audio = audioFileName;
        FindObjectOfType<PredefinedWorkoutManager>().currentVoiceOverClip = audio;
        LowerDownBGMUsicWhenDialogueOn(false);//
        PlayAudioClip(audio, true, false);
        isPlayingEngagingSound = true;
        FindObjectOfType<PredefinedWorkoutManager>().audioEngagingVoicePlayed = true;
        yield return new WaitForSeconds(audio.length);//
        nonperformingCount++;
        Debug.Log("<SM> nonperformingCount :" + nonperformingCount);
        LowerDownBGMUsicWhenDialogueOn(true);
        isPlayingEngagingSound = false;
        FindObjectOfType<PredefinedWorkoutManager>().audioEngagingVoicePlayed = false;
    }

    private int SelectRandomIndex()
    {
        if (selectedEngagingAudios.Count <= 1)
            return 0;
        int x = lastSelectedRandom;
        while (x == lastSelectedRandom)
        {
            x = Random.Range(0, selectedEngagingAudios.Count);
        }
        lastSelectedRandom = x;
        return x;
    }

    public void PauseDialogue()
    {
        dialogueAudioSource.Stop();
    }

    
        
        


}
