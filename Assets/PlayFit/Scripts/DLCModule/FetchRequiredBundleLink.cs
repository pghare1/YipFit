using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;

public class FetchRequiredBundleLink : MonoBehaviour
{
    static FirebaseAuth firebaseAuth;
   
    static FirebaseDatabase firebaseDatabase;

    public AudioBundleLinkHolder audioBundleLinkHolder;

    public AudioDownloader audioDownloader;

    const string actionNameSound = "action-name-dialogues", actionRelatedDialogues = "action-related-dialogues", nonPerformingDialogues = "non-performing-dialogues",
        sfxEngagingAudio = "action-engaging-dialogues", motivationalSound = "motivational-dialogues";
    const string storageURLKey = "storage-link";

    private void Start()
    {
        //This is only for test db connectivity. Change this to FirebaseDatabase.DefaultInstance later when node is present on actual DB.
        firebaseAuth = FirebaseAuth.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.DefaultInstance;
        ClearLinkData();
        GetAudioBundleLinks();
    }

    private void ClearLinkData()
    {
        audioBundleLinkHolder.ActionNameSoundURL.Clear();
        audioBundleLinkHolder.ActionRelatedDialoguesURL.Clear();
        audioBundleLinkHolder.NonPerformingDialoguesURL.Clear();
        audioBundleLinkHolder.MotivationalAudioURL.Clear();
        audioBundleLinkHolder.SfxEngagingAudioURL.Clear();
    }

    private async void GetAudioBundleLinks()
    {

        string currentPlatform = null;

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                currentPlatform = "android";
                break;
            case RuntimePlatform.IPhonePlayer:
                currentPlatform = "ios";
                break;
            case RuntimePlatform.WindowsPlayer:
                currentPlatform = "windows";
                break;
            case RuntimePlatform.WindowsEditor:
                currentPlatform = "windows";
                break;
            default:
                currentPlatform = null;
                break;
        }

        if (string.IsNullOrEmpty(currentPlatform))
        {
            return;
        }

        DatabaseReference databaseReference = firebaseDatabase.GetReference("inventory").Child("dlc").Child("yipfit");

        
        
        try
        {
            DataSnapshot snapshot = await databaseReference.Child(currentPlatform).GetValueAsync();
            if (snapshot.Value != null)
            {
                DataSnapshot commonSnapshot = null;
                commonSnapshot = snapshot.Child(sfxEngagingAudio);

                for (int i = 0; i < commonSnapshot.ChildrenCount; i++)
                {
                    audioBundleLinkHolder.SfxEngagingAudioURL.Add(commonSnapshot.Child(i.ToString()).Child(storageURLKey).Value?.ToString() ?? "");
                }

                commonSnapshot = snapshot.Child(actionNameSound);

                for (int i = 0; i < commonSnapshot.ChildrenCount; i++)
                {
                    audioBundleLinkHolder.ActionNameSoundURL.Add(commonSnapshot.Child(i.ToString()).Child(storageURLKey).Value?.ToString() ?? "");
                }

                commonSnapshot = snapshot.Child(actionRelatedDialogues);

                for (int i = 0; i < commonSnapshot.ChildrenCount; i++)
                {
                    audioBundleLinkHolder.ActionRelatedDialoguesURL.Add(commonSnapshot.Child(i.ToString()).Child(storageURLKey).Value?.ToString() ?? "");
                }

                commonSnapshot = snapshot.Child(nonPerformingDialogues);

                for (int i = 0; i < commonSnapshot.ChildrenCount; i++)
                {
                    audioBundleLinkHolder.NonPerformingDialoguesURL.Add(commonSnapshot.Child(i.ToString()).Child(storageURLKey).Value?.ToString() ?? "");
                }

                commonSnapshot = snapshot.Child(motivationalSound);

                for (int i = 0; i < commonSnapshot.ChildrenCount; i++)
                {
                    audioBundleLinkHolder.MotivationalAudioURL.Add(commonSnapshot.Child(i.ToString()).Child(storageURLKey).Value?.ToString() ?? "");
                }

            }
        }
        catch (Exception e) { Debug.LogError("Firebase Exception : " + e.StackTrace + "\nMessage " + e.Message); }

        audioDownloader.DownloadAudioFiles();

    }

}
