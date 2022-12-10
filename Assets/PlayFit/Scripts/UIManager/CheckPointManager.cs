using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointManager : MonoBehaviour
{
    public WorkoutConfig workoutConfig;
    public RunTimePredefinedWorkout runTimePredefinedWorkout;
    public PredefinedWorkoutManager predefinedWorkoutManager;
    public TextMeshProUGUI triggerUpdateText, triggerUpdateTextForPause;
    public bool coreStarted = false;
    public bool warmUpStart = false;
    public bool midOfCore = false, endOfCore = false, inBreak = false, startOfCool = false, endOfCool = false, endOfWarm = false;
    public SoundManager soundManager;
    public string workoutState = null;

    private void Start()
    {
        triggerUpdateText.gameObject.SetActive(true);
    }

    //At start of the core section
    public void TriggerAtStartOfTheCoreSection()
    {
        //float totalCoreTimeHalf = (runTimePredefinedWorkout.coreActionDuration_30 + runTimePredefinedWorkout.coreActionDuration_70) / 2;
        //float WarmUpDeductedFromTotal = workoutConfig.workoutTotalTime - runTimePredefinedWorkout.warmupActionDuration;
        //float WarmUpAndHalfCoreDeductedFromTotal = WarmUpDeductedFromTotal - totalCoreTimeHalf;

        //Deduct time till start of the core
        if (workoutConfig.time <= workoutConfig.workoutTotalTime - runTimePredefinedWorkout.warmupActionDuration  )
        {
            Debug.Log("Core Started");
            soundManager.CoreBg();//core background music started
            //soundManager.SelectDialgoue(DialoguesClassification.DialogueType.CoreStart);//VoiceOver For Core Section Started
            triggerUpdateText.gameObject.SetActive(true);
          //  triggerUpdateText.text = "Core Started";
            StartCoroutine(DisableTriggerAfterThisTime());//Disable Trigger hit text after 10sec of time interval
            coreStarted = true;
            workoutState = "C";//TODO: Change to Enum
        }
        
        
    }

    //Trigger at mid of the core section added
    public void TriggerAtMidOfTheCore()
    {
        //calculates time till mid of the core section
        float totalCoreTimeHalf = (runTimePredefinedWorkout.coreActionDuration_30 + runTimePredefinedWorkout.coreActionDuration_70) / 2;
        float WarmUpDeductedFromTotal = workoutConfig.workoutTotalTime - runTimePredefinedWorkout.warmupActionDuration;
        float WarmUpAndHalfCoreDeductedFromTotal = WarmUpDeductedFromTotal - totalCoreTimeHalf;

        if (workoutConfig.time <= WarmUpAndHalfCoreDeductedFromTotal)
        {
            
           // soundManager.SelectDialgoue(DialoguesClassification.DialogueType.CoreMid);//Voice Over for mid of the core played
            triggerUpdateText.gameObject.SetActive(true);
           // triggerUpdateText.text = "Mid of Core";
            StartCoroutine(DisableTriggerAfterThisTime());
            midOfCore = true;
        }
    }

    //Trigger Set at end of the core section
    public void TriggerAtEndOfTheCore()
    {
        // Time is calculated for End of the core section
        float totalCore = runTimePredefinedWorkout.coreActionDuration_30 + runTimePredefinedWorkout.coreActionDuration_70;
        float warmUpAndCoreDeductedFromTotalTime = runTimePredefinedWorkout.warmupActionDuration + totalCore;
        
        
        if (workoutConfig.time <= (workoutConfig.workoutTotalTime - warmUpAndCoreDeductedFromTotalTime) + 6f )
        {
            
           // soundManager.SelectDialgoue(DialoguesClassification.DialogueType.CoreEnd);// Voice over for core end is played
            triggerUpdateText.gameObject.SetActive(true);
           // triggerUpdateText.text = "End of Core";
            StartCoroutine(DisableTriggerAfterThisTime());
            endOfCore = true;
        }
    }

    //Trigger set for end of cool down
    public void TriggerAtEndOFCoolDown()
    {
        float totalCore = runTimePredefinedWorkout.coreActionDuration_30 + runTimePredefinedWorkout.coreActionDuration_70;
        float warmUpAndCoreCoolDeductedFromTotalTime = runTimePredefinedWorkout.warmupActionDuration + totalCore + runTimePredefinedWorkout.coolDownActionDuration;


        if (workoutConfig.time <= (workoutConfig.workoutTotalTime - warmUpAndCoreCoolDeductedFromTotalTime) + 4f)
        {
            
            //soundManager.SelectDialgoue(DialoguesClassification.DialogueType.CoolDownEnd);
            triggerUpdateText.gameObject.SetActive(true);
           // triggerUpdateText.text = "Workout End";
            StartCoroutine(DisableTriggerAfterThisTime());
            endOfCool = true;
        }

    }

    // Trigger set for End Of warm up
    public void TriggerAtWarmUpEnd()
    {
        float warmupTimeDecucted = workoutConfig.workoutTotalTime - runTimePredefinedWorkout.warmupActionDuration;
        if (workoutConfig.time <= warmupTimeDecucted + 7f)
        {
            
           // soundManager.SelectDialgoue(DialoguesClassification.DialogueType.WarmUpEnd);
            triggerUpdateText.gameObject.SetActive(true);
           // triggerUpdateText.text = "WarmUp End";
            StartCoroutine(DisableTriggerAfterThisTime());
            endOfWarm = true;
        }
    }

    //Called At Play Button
    //Trigger Set for Start Of WarmUp
    public void TriggerAtStartOfWarmUp()
    {
        workoutState = "W";
        soundManager.WarmUpBg();
       // soundManager.SelectDialgoue(DialoguesClassification.DialogueType.WarmUpStart);
        triggerUpdateText.gameObject.SetActive(true);
       // triggerUpdateText.text = "Start Of WarmUp";
        warmUpStart = true;
        StartCoroutine(DisableTriggerAfterThisTime());
        
    }

    //Trigger Set For start Of Cool Down
    public void TriggerAtStartOfTheCooldown()
    {
        float totalCore = runTimePredefinedWorkout.coreActionDuration_30 + runTimePredefinedWorkout.coreActionDuration_70;
        float warmUpAndCoreDeductedFromTotalTime = workoutConfig.workoutTotalTime - (runTimePredefinedWorkout.warmupActionDuration + totalCore);
        if (workoutConfig.time <= warmUpAndCoreDeductedFromTotalTime)
        {
            soundManager.CooldownBg();
           // soundManager.SelectDialgoue(DialoguesClassification.DialogueType.CoolDownStart);
            triggerUpdateText.gameObject.SetActive(true);
           // triggerUpdateText.text = "Start Of CoolDown";
            StartCoroutine(DisableTriggerAfterThisTime());
            startOfCool= true;
            workoutState = "S";
        }

    }

    
    //On starting break
    public void TriggerAtBreak()
    {
        
            //soundManager.SelectDialgoue(DialoguesClassification.DialogueType.InBreak);
            triggerUpdateText.gameObject.SetActive(true);
           // triggerUpdateText.text = "In Break";
        inBreak = true;
        StartCoroutine(DisableTriggerAfterThisTimeForBreak());
            
        
    }

    //For Pause Timer Getting over , triggered at last 10sec
    public void TriggerWhenBreakISAboutTOOVer()
    {
        triggerUpdateTextForPause.gameObject.SetActive(true);
        triggerUpdateTextForPause.text = "Pause timer is about to complete";
        StartCoroutine(DisableTriggerAfterPauseTime());

    }

    public IEnumerator DisableTriggerAfterThisTime()
    {
        
        yield return new WaitForSeconds(10f);
        triggerUpdateText.gameObject.SetActive(false);
        Debug.Log("Core coroutine done");
    }

    public IEnumerator DisableTriggerAfterThisTimeForBreak()
    {

        soundManager.BreakBg();
        yield return new WaitForSeconds(5f);
        soundManager.PlayBreakMotivationalSound();
        yield return new WaitForSeconds(5f);
        triggerUpdateText.gameObject.SetActive(false);
        Debug.Log("Core coroutine done");
        
        inBreak = false;
    }

    public IEnumerator DisableTriggerAfterPauseTime()
    {

        yield return new WaitForSeconds(10f);
        triggerUpdateText.gameObject.SetActive(false);
        Debug.Log("Core coroutine done");
    }
}
