using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkoutConfig", menuName = "Create WorkoutConfig")]
public class WorkoutConfig : ScriptableObject
{
    public float time;
    public float workoutTotalTime;
    public string intensity;
    public float warmup;
    public float coreEx;
    public float cooldown;

    public float currentSelectedTime;
    public string currentSelectedIntensity;

    public float triggerBreakTimer = 0f;
    public float breakTime = 30f;

    public float nonPerformingTime = 0f;

    public float totalTimeOfWorkoutIncludingBrakes = 0f;
    public float totalTimeWithBreakIncluded = 0f;

    public bool backgroundMusicSetting;
    public bool dialoguesSettings;

    public bool GetBackgroundMusicSetting()
    {
        return (PlayerPrefs.GetInt("bgmusic", 0) == 1) ? true : false;
    }

    public void SetBackgroundMusicSetting(bool setting)
    {
        PlayerPrefs.SetInt("bgmusic", (setting) ? 1 : 0);
    }

    public bool GetDialogueSetting()
    {
        return (PlayerPrefs.GetInt("dialogue", 0) == 1) ? true : false;
    }

    public void SetDialogueSetting(bool setting)
    {
        PlayerPrefs.SetInt("dialogue", (setting) ? 1 : 0);
    }


}




