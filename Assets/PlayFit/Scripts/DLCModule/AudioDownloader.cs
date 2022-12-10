using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

public class AudioDownloader : MonoBehaviour
{

    public AudioBundleLinkHolder audioBundleLinkHolder;
    public DownloadAudioFiles downloadAudioFiles;
    public AudioClipHolder audioClipHolder;
    public Image downloadProgressFiller;
    public TextMeshProUGUI downloadProgressText;
    public TextMeshProUGUI bundleNameDisplay;

    const string actionNameSound = "action-name-dialogues", actionRelatedDialogues = "action-related-dialogues", nonPerformingDialogues = "non-performing-dialogues",
    sfxEngagingAudio = "action-engaging-dialogues", motivationalSound = "motivational-dialogues";

    public bool printDictionaryData = false;
    public bool clearCacheStorage = false;

    private void Awake()
    {
        Caching.compressionEnabled = false;
        if (clearCacheStorage)
            Caching.ClearCache();
    }

    public void DownloadAudioFiles()
    {
        ClearBundleListData();
        InitializeDictionary();
        StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.SfxEngagingAudioURL, sfxEngagingAudio));
        StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.ActionNameSoundURL, actionNameSound));
        StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.ActionRelatedDialoguesURL, actionRelatedDialogues));
        StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.MotivationalAudioURL, motivationalSound));
        StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.NonPerformingDialoguesURL, nonPerformingDialogues));
    }

    private IEnumerator AudioFileDownloadProcess(List<string> bundleURLs, string directoryName)
    {
        UnityWebRequest bundleRequest = null;
        AudioClip[] downloadedVoiceOvers = null;

        bundleNameDisplay.text = "Preparing External Sounds";
        foreach (string url in bundleURLs)
        {
            bundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(url, 0, 0);
            UnityWebRequestAsyncOperation operation = bundleRequest.SendWebRequest();
            while (!operation.isDone)
            {
                downloadProgressFiller.fillAmount = operation.progress;
                downloadProgressText.text = (operation.progress * 100f).ToString("0") + "%";
                yield return new WaitForSeconds(0.1f);
            }

            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(bundleRequest);
            
            switch (directoryName)
            {
                case actionNameSound:
                    downloadAudioFiles.AudioNameBundleDictionary.Add(assetBundle);
                    //StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.ActionRelatedDialoguesURL, actionRelatedDialogues));
                    break;
                case actionRelatedDialogues:
                    downloadAudioFiles.AudioDescriptionBundleDictionary.Add(assetBundle);
                    //StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.MotivationalAudioURL, motivationalSound));
                    break;
                case nonPerformingDialogues:
                    downloadAudioFiles.NonPerformingBundleDictionary.Add(assetBundle);
                    break;
                case sfxEngagingAudio:
                    downloadAudioFiles.EngagingBundleDictionary.Add(assetBundle);
                    //StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.ActionNameSoundURL, actionNameSound));
                    break;
                case motivationalSound:
                    downloadAudioFiles.MotivationalBundleDictionary.Add(assetBundle);
                    //StartCoroutine(AudioFileDownloadProcess(audioBundleLinkHolder.NonPerformingDialoguesURL, nonPerformingDialogues));
                    break;
            }
        }
        //Continue to add the audio clips in scriptable files.
        
        StartExtractingOfAudioBundles(directoryName);
    }

    private string ReturnBundleNameDisplay(string directoryName)
    {
        switch (directoryName)
        {
            case actionNameSound:
                return "Action Name Sound";
            case actionRelatedDialogues:
                return "Action Dialogues Sound";
            case nonPerformingDialogues:
                return "Non Performing Sound";
            case sfxEngagingAudio:
                return "Engaging Sound";
            case motivationalSound:
                return "Motivational Sound";
            default:
                return "";
        }
    }

    private void ClearBundleListData()
    {
        downloadAudioFiles.AudioDescriptionBundleDictionary.Clear();
        downloadAudioFiles.AudioNameBundleDictionary.Clear();
        downloadAudioFiles.NonPerformingBundleDictionary.Clear();
        downloadAudioFiles.EngagingBundleDictionary.Clear();
        downloadAudioFiles.MotivationalBundleDictionary.Clear();
    }

    private void InitializeDictionary()
    {
        audioClipHolder.WorkoutDialoguesClassifications.Add(actionNameSound, new List<DialoguesClassification>());
        audioClipHolder.WorkoutDialoguesClassifications.Add(actionRelatedDialogues, new List<DialoguesClassification>());
        audioClipHolder.WorkoutDialoguesClassifications.Add(nonPerformingDialogues, new List<DialoguesClassification>());
        audioClipHolder.WorkoutDialoguesClassifications.Add(sfxEngagingAudio, new List<DialoguesClassification>());
        audioClipHolder.WorkoutDialoguesClassifications.Add(motivationalSound, new List<DialoguesClassification>());
    }

    private void StartExtractingOfAudioBundles(string bundleName)
    {
        switch (bundleName)
        {
            case actionNameSound:
                ExtractAudioClips(downloadAudioFiles.AudioNameBundleDictionary, actionNameSound);
                break;
            case actionRelatedDialogues:
                ExtractAudioClips(downloadAudioFiles.AudioDescriptionBundleDictionary, actionRelatedDialogues);
                break;
            case nonPerformingDialogues:
                ExtractAudioClips(downloadAudioFiles.NonPerformingBundleDictionary, nonPerformingDialogues);
                break;
            case sfxEngagingAudio:
                ExtractAudioClips(downloadAudioFiles.EngagingBundleDictionary, sfxEngagingAudio);
                break;
            case motivationalSound:
                ExtractAudioClips(downloadAudioFiles.MotivationalBundleDictionary, motivationalSound); 
                break;
        }
        
    }

    private void ExtractAudioClips(List<AssetBundle> bundle, string bundleName)
    {
        List<string> generatedNames = new List<string>();
        List<AudioClip> receivedAudioClips = new List<AudioClip>();
        AudioClip[] clipData;
        string[] audioFileNames; 
        foreach (AssetBundle item in bundle)
        {
            audioFileNames = item.GetAllAssetNames();
            clipData = item.LoadAllAssets<AudioClip>();
            FillAudioFileNamesIntoList(ref generatedNames, audioFileNames);
            FillAudioClipIntoList(ref receivedAudioClips, clipData);
        }

        StoreAudioFilesInDicitionary(bundleName, receivedAudioClips, generatedNames);

        if (bundleName == motivationalSound)
            LoadGameScene();
    }

    private void FillAudioFileNamesIntoList(ref List<string> namesList, string[] files)
    {
        foreach (string item in files)
        {
            namesList.Add(Path.GetFileNameWithoutExtension(item));
        }
    }

    private void FillAudioClipIntoList(ref List<AudioClip> audios, AudioClip[] files)
    {
        foreach (AudioClip item in files)
        {
            audios.Add(item);
        }
    }

    private void StoreAudioFilesInDicitionary(string bundleName, List<AudioClip> audioClips, List<string> audioFileNames)
    {
        DialoguesClassification.DialogueType dialogueType = DialoguesClassification.DialogueType.actionNameDialgue;
        switch (bundleName)
        {
            case actionNameSound:
                dialogueType = DialoguesClassification.DialogueType.actionNameDialgue;
                break;
            case actionRelatedDialogues:
                dialogueType = DialoguesClassification.DialogueType.actionDescriptionDialogue;
                break;
            case nonPerformingDialogues:
                dialogueType = DialoguesClassification.DialogueType.nonPerformingDialogue;
                break;
            case sfxEngagingAudio:
                dialogueType = DialoguesClassification.DialogueType.engagingDialogue;
                break;
            case motivationalSound:
                dialogueType = DialoguesClassification.DialogueType.motivationalDialogue;
                break;
        }
        int i = 0;
        foreach (AudioClip item in audioClips)
        {
            audioClipHolder.WorkoutDialoguesClassifications[bundleName].Add(new DialoguesClassification(audioFileNames[i], item, dialogueType));
            i++;
        }

        if (printDictionaryData)
        {
            PrintAudioDictionaryData(audioClipHolder.WorkoutDialoguesClassifications, bundleName);
        }

    }

    private void PrintAudioDictionaryData(Dictionary<string, List<DialoguesClassification>> keyValuePairs, string dicName)
    {
        foreach (KeyValuePair<string, List<DialoguesClassification>> item in keyValuePairs)
        {
            Debug.LogError("Dict Name : " + dicName + "\n" + item.Key + " :: " + item.Value.Count);
        }
    }

    public void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UpdateScene_Demo");
    }

}
