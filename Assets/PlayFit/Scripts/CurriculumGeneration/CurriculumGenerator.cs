using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurriculumGenerator : MonoBehaviour
{
    const string LOW = "low";
    const string MEDIUM = "medium";
    const string HIGH = "high";
    const int MINLOW = 4, MINMED = 4, MAXMID = 10, MINHIGH = 6, MAXHIGH = 10;
    const int MIN_LOW_EQ = 4, MAX_LOW_EQ = 5, MIN_MED_EQ = 6, MAX_MED_EQ = 7, MIN_HIGH_EQ = 8, MAX_HIGH_EQ = 10, MIN_LightWorkout = 4, MAX_LightWorkout = 5;
    int MAXLOW = 7;
    public static CurriculumGenerator instance { get; set; }
    public UIManager uIManager;
    public PredefinedWorkoutManager predefinedWorkoutManager;
    public SectionAndDuration sectionAndDuration;
    public ActionMappedTargetAreaAndIntensity actionMappedTargetAreaAndIntensity;
    public float selectedTime;
    public float warmUpTime;
    public float totalCoreWorkoutTime;
    public float coolDownTime;
    public string selectedIntensity;
    public float coreActionDuration_30Percent;
    public float coreActionDuration_70Percent;
    public float totalStretchTime;

    public WorkoutConfig workoutConfig;

    const float PER30 = 0.3f;
    const float PER70 = 0.7f;

    public RunTimePredefinedWorkout runTimePredefinedWorkout;

    [SerializeField] private List<ActionsInfo> warmupActionList;
   [SerializeField] private List<ActionsInfo> coreActionList;
    [SerializeField] private List<ActionsInfo> SelectedIntensityWiseCoreActionList;
    [SerializeField] private List<ActionsInfo> coolDownActionList;
    [SerializeField] private List<ActionsInfo> stretchingActionList;
    public bool isDemo, is20Secs;

    bool isWarmupActionsFilled = false;
    bool isExactCoreActionsFilled = false;
    bool isCoreActionsFilled = false;
    bool isCooldownFilled = false;
    bool isStretchingFilled = false;
    
    int counter = 0;
    private int lastSelectedRandom = -1;
    private int lastSelectedRandom_For70 = -1;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        warmupActionList = new List<ActionsInfo>();
        coreActionList = new List<ActionsInfo>();
        coolDownActionList = new List<ActionsInfo>();
        SelectedIntensityWiseCoreActionList = new List<ActionsInfo>();
        stretchingActionList = new List<ActionsInfo>();
        if (is20Secs)
        {
            foreach (ActionsInfo item in actionMappedTargetAreaAndIntensity.ActionList)
            {
                item.timePerIntensityLevels.LowIntensity = 20f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectedTimeOfStretching()
    {
        selectedTime = workoutConfig.currentSelectedTime;
        totalStretchTime = selectedTime;
        CreateStretchingWorkout();
    }

   
    
    public void SelectedIntensityAndTime(bool isCoreWorkout)
    {
        
        selectedTime = workoutConfig.currentSelectedTime;//time;
        selectedIntensity = workoutConfig.currentSelectedIntensity;//intensity;
        if (workoutConfig.time >= 1200)
            MAXLOW = 10;
        //if (workoutConfig.currentSelectedIntensity != selectedIntensity || workoutConfig.currentSelectedTime != time)
        //{
        //    ResetRuntimePredefinedWorkouts();
        //}
        if(!isCoreWorkout)
        {
            DivideDurationAsPerIntensity(selectedIntensity);
        }
        else
        {
            coreActionDuration_30Percent = selectedTime * PER30;
            coreActionDuration_70Percent = selectedTime * PER70;
            CreateCoreWorkout();
        }

    }


    public void DivideDurationAsPerIntensity(string intensity)
    {
        switch(intensity)
        {
            case LOW:
                warmUpTime = selectedTime  * sectionAndDuration.sections[0].WarmUp;
                Debug.Log("Warmup time" + warmUpTime);
                if (!isDemo)
                {
                    totalCoreWorkoutTime = selectedTime * sectionAndDuration.sections[0].CoreAction;
                    coreActionDuration_30Percent = totalCoreWorkoutTime * PER30;
                    coreActionDuration_70Percent = totalCoreWorkoutTime * PER70;
                }
                else
                {
                    totalCoreWorkoutTime = selectedTime * sectionAndDuration.sections[0].CoreAction;
                    coreActionDuration_30Percent = totalCoreWorkoutTime * PER30;
                    coreActionDuration_70Percent = totalCoreWorkoutTime * PER70;
                }
                Debug.Log("core time" + totalCoreWorkoutTime);
                coolDownTime = selectedTime * sectionAndDuration.sections[0].CoolDown;
                Debug.Log("cool time" + coolDownTime);
                if(uIManager.inLightWorkout)
                {
                    CreateLightWorkout(intensity);
                }
                else
                {
                    CreateWorkout(intensity);
                }
                
                break;

            case MEDIUM:
                warmUpTime = selectedTime * sectionAndDuration.sections[1].WarmUp;
                Debug.Log("Warmup time" + warmUpTime);
                totalCoreWorkoutTime = selectedTime * sectionAndDuration.sections[1].CoreAction;
                coreActionDuration_30Percent = totalCoreWorkoutTime * PER30;
                coreActionDuration_70Percent = totalCoreWorkoutTime * PER70;
                Debug.Log("core time" + totalCoreWorkoutTime);
                coolDownTime = selectedTime * sectionAndDuration.sections[1].CoolDown;
                Debug.Log("cool time" + coolDownTime);

                if (uIManager.inLightWorkout)
                {
                    CreateLightWorkout(intensity);
                }
                else
                {
                    CreateWorkout(intensity);
                }
                break;

            case HIGH:
                warmUpTime = selectedTime * sectionAndDuration.sections[2].WarmUp;
                totalCoreWorkoutTime = selectedTime * sectionAndDuration.sections[2].CoreAction;
                coreActionDuration_30Percent = totalCoreWorkoutTime * PER30;
                coreActionDuration_70Percent = totalCoreWorkoutTime * PER70;
                coolDownTime = selectedTime * sectionAndDuration.sections[2].CoolDown;
                if (uIManager.inLightWorkout)
                {
                    CreateLightWorkout(intensity);
                }
                else
                {
                    CreateWorkout(intensity);
                }
                break;

        }

    }

    public void CreateWorkout(string workoutIntensity)
    {
        warmupActionList.Clear();
        coreActionList.Clear();
        coolDownActionList.Clear();
        SelectedIntensityWiseCoreActionList.Clear();

        if(!isDemo)
            FillWarmupActionIntoList();
        FillCoreActionIntoList( MinValuesForMixList(selectedIntensity), MaxValuesForMixList(selectedIntensity));
        FillIntensityWiseCoreActionIntoList(MinValuesForPerfectIntensityWiseList(selectedIntensity), MaxValuesForPerfectIntensityWiseList(selectedIntensity));
       
        if(!isDemo)
            FillCoolDownActionIntoList();
        CreateRunTimePredefinedWorkout(workoutIntensity);
    }

    public void CreateLightWorkout(string workoutIntensity)
    {
        warmupActionList.Clear();
        coreActionList.Clear();
        coolDownActionList.Clear();
        SelectedIntensityWiseCoreActionList.Clear();

        
            FillWarmupActionIntoList();
        FillCoreActionIntoList(MIN_LightWorkout, MAX_LightWorkout);
        FillIntensityWiseCoreActionIntoList(MIN_LightWorkout, MAX_LightWorkout);

        
            FillCoolDownActionIntoList();
        CreateRunTimeLightPredefinedWorkout(workoutIntensity);
    }

    public void CreateStretchingWorkout()
    {
        stretchingActionList.Clear();
        FillStretchingActionList();
        CreateRuntimeStretchingWorkout();
    }

    public void CreateCoreWorkout()
    {
        coreActionList.Clear();
        SelectedIntensityWiseCoreActionList.Clear();
        FillCoreActionIntoList(MinValuesForMixList(selectedIntensity), MaxValuesForMixList(selectedIntensity));
        FillIntensityWiseCoreActionIntoList(MinValuesForPerfectIntensityWiseList(selectedIntensity), MaxValuesForPerfectIntensityWiseList(selectedIntensity));
        CreateRuntimeCoreWorkout();
    }

    



    private void FillWarmupActionIntoList()
    {
        //TODO: if scriptable is null check, We might use predefined workouts
        if (warmupActionList.Count > 0)
            return; 

        foreach (ActionsInfo item in actionMappedTargetAreaAndIntensity.ActionList)
        {
            if (item.targetSection == ActionsInfo.TargetSection.WARMUP)
            {
                warmupActionList.Add(item);
            }
        }
    }

    private void FillStretchingActionList()
    {
        //TODO: if scriptable is null check, We might use predefined workouts
        if (stretchingActionList.Count > 0)
            return;

        foreach (ActionsInfo item in actionMappedTargetAreaAndIntensity.ActionList)
        {
            if (item.isStretch == true)
            {
                stretchingActionList.Add(item);
            }
        }
    }

    /*
* 1. Min and Max values for core List is defined as per the intensity level assigned to the Action Eg. For low range is 4-7 i.e min-4 max-7
* This function will add all appropriate actions in to list
*/
    private void FillCoreActionIntoList(int min, int max)
    {
        //TODO: create Common Function for 30% and 70% actions list
        if (coreActionList.Count > 0)
            return;

        foreach (ActionsInfo item in actionMappedTargetAreaAndIntensity.ActionList)
        {
            if (item.targetSection == ActionsInfo.TargetSection.COREACTION && CheckCurrentActionIsInGivenRange(selectedIntensity, item, min, max))
            {
                coreActionList.Add(item);
            }
        }
    }

    private void FillIntensityWiseCoreActionIntoList(int min, int max)
    {
        if (SelectedIntensityWiseCoreActionList.Count > 0)
            return;

        foreach (ActionsInfo item in actionMappedTargetAreaAndIntensity.ActionList)
        {
            if (item.targetSection == ActionsInfo.TargetSection.COREACTION && CheckCurrentActionIsInGivenRange(selectedIntensity, item, min, max))
            {
                SelectedIntensityWiseCoreActionList.Add(item);
            }
        }
    }

    public int MinValuesForMixList(string Intensity)
    {
        int value = 0;
        switch(Intensity)
        {
            case LOW:
                value = MINLOW;
                break;

            case MEDIUM:
                value = MINMED;
                break;

            case HIGH:
                value = MINHIGH;
                break;
        }
        return value;
    }

    public int MaxValuesForMixList(string Intensity)
    {
        int value = 0;
        switch (Intensity)
        {
            case LOW:
                value = MAXLOW;
                break;

            case MEDIUM:
                value = MAXMID;
                break;

            case HIGH:
                value = MAXHIGH;
                break;
        }
        return value;
    }

    public int MaxValuesForPerfectIntensityWiseList(string Intensity)
    {
        int value = 0;
        switch (Intensity)
        {
            case LOW:
                value = MAX_LOW_EQ;
                break;

            case MEDIUM:
                value = MAX_MED_EQ;
                break;

            case HIGH:
                value = MAX_HIGH_EQ;
                break;
        }
        return value;
    }

    public int MinValuesForPerfectIntensityWiseList(string Intensity)
    {
        int value = 0;
        switch (Intensity)
        {
            case LOW:
                value = MIN_LOW_EQ;
                break;

            case MEDIUM:
                value = MIN_MED_EQ;
                break;

            case HIGH:
                value = MIN_HIGH_EQ;
                break;
        }
        return value;
    }
    public bool CheckCurrentActionIsInGivenRange(string intensity, ActionsInfo currentSelectedAction, int minValue, int maxValue)
    {
        bool isInRange = false;

        switch(intensity)
        {
            case LOW:
                if (currentSelectedAction.intensityScale >= minValue && currentSelectedAction.intensityScale <= maxValue)
                    isInRange = true;
                break;

            case MEDIUM:
                if (currentSelectedAction.intensityScale >= minValue && currentSelectedAction.intensityScale <= maxValue)
                    isInRange = true;
                break;

            case HIGH:
                if (currentSelectedAction.intensityScale >= minValue && currentSelectedAction.intensityScale <= maxValue)
                    isInRange = true;
                break;
        }

        return isInRange;

    }


    private void FillCoolDownActionIntoList()
    {
        //TODO: create same function for warmup and cooldown
        if (coolDownActionList.Count > 0)
            return;

        foreach (ActionsInfo item in actionMappedTargetAreaAndIntensity.ActionList)
        {
            if (item.targetSection == ActionsInfo.TargetSection.COOLDOWN)
            {
                coolDownActionList.Add(item);
            }
        }
    }

    private void CreateRunTimePredefinedWorkout(string intensity)
    {
        
        if (!isWarmupActionsFilled)
        {
            FillOutActionForWarupOrCooldownList(intensity, warmUpTime, true, warmupActionList, ref isWarmupActionsFilled, SelectedActionsForWorkout.WorkoutType.Warmup);
        }
        if( !isExactCoreActionsFilled)
        {
            FillOutCoreActions30PercentOnly();
        }

        if (!isCoreActionsFilled)
        {
            FillOutCoreActions70PercentOnly();
            //try
            //{

            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogError("Curriculum Gen Error : " + e.StackTrace + "\nMessage : " + e.Message);
            //}
        }

        //try
        //{
            
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError("Curriculum Gen Error : " + e.StackTrace + "\nMessage : " + e.Message);
        //}

        if (!isCooldownFilled)
        {
            FillOutActionForWarupOrCooldownList(intensity, coolDownTime, false, coolDownActionList, ref isCooldownFilled, SelectedActionsForWorkout.WorkoutType.Cooldown);
        }
        
    }


    private void CreateRunTimeLightPredefinedWorkout(string intensity)
    {
        
        if (!isWarmupActionsFilled)
        {
            FillOutActionForWarupOrCooldownList(intensity, warmUpTime, true, warmupActionList, ref isWarmupActionsFilled, SelectedActionsForWorkout.WorkoutType.Warmup);
        }
        if( !isExactCoreActionsFilled)
        {
            FillOutCoreActions30PercentOnly_IfListExhausted();
        }
        try
        {
            if (!isCoreActionsFilled)
            {
                try
                {
                    FillOutCoreActions70PercentOnly_IfListExhausted();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Curriculum Gen Error : " + e.StackTrace + "\nMessage : " + e.Message);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Curriculum Gen Error : " + e.StackTrace + "\nMessage : " + e.Message);
        }

        if (!isCooldownFilled)
        {
            FillOutActionForWarupOrCooldownList(intensity, coolDownTime, false, coolDownActionList, ref isCooldownFilled, SelectedActionsForWorkout.WorkoutType.Cooldown);
        }
        
    }

    private void CreateRuntimeStretchingWorkout()
    {
        if(!isStretchingFilled)
        {
            FillStrtechingActionsWorkout();
        }
        
    }

    private void CreateRuntimeCoreWorkout()
    {
        if (!isExactCoreActionsFilled)
        {
            FillOutCoreActions30PercentOnly_IfListExhausted();
        }
        if (!isCoreActionsFilled)
        {
            FillOutCoreActions70PercentOnly_IfListExhausted();
        }
    }

    private void FillOutActionForWarupOrCooldownList(string intensity, float time, bool isWarmupAction, List<ActionsInfo> actionsInfo, ref bool selectedActionCompletion, SelectedActionsForWorkout.WorkoutType workoutType)
    {
        if (isDemo)
            return;
        if (!selectedActionCompletion)
        {
            
            //check the new added action duration to total warmup/ cooldown timer
            while (runTimePredefinedWorkout.ChooseWarmupOrCooldownDuration(isWarmupAction) <= time)
            {
                //if time left from warmup or cooldown is less than 15 , Ignore
                if (Mathf.Abs((runTimePredefinedWorkout.ChooseWarmupOrCooldownDuration(isWarmupAction) - time)) <= 15f)
                    return;
                //select action randomly
                int x = -1;
                SelectedActionsForWorkout selectedActionsForWorkout = null;
                x = Random.Range(0, actionsInfo.Count);
                selectedActionsForWorkout = new SelectedActionsForWorkout(
                   actionsInfo[x].actionId,
                   actionsInfo[x].timePerIntensityLevels.ReturnIntensityValue(intensity),
                   false,
                   workoutType,
                  //SelectedActionsForWorkout.WorkoutType.Warmup,
                   actionsInfo[x].intensityScale
               );//if it is first action add directly
                if (runTimePredefinedWorkout.selectedActionsForWorkouts.Count <= 0)
                {
                    runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                    runTimePredefinedWorkout.SetWarmupOrCooldownDuration(isWarmupAction, actionsInfo[x].timePerIntensityLevels.ReturnIntensityValue(intensity));
                }
                else
                {// check if the same action is already selected 
                    if (!CheckIfValueExistsInList(selectedActionsForWorkout))
                    { //in if current selected action not present in workout , Add the selected action
                        runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                        //update warmup or cooldown timer
                        runTimePredefinedWorkout.SetWarmupOrCooldownDuration(isWarmupAction, actionsInfo[x].timePerIntensityLevels.ReturnIntensityValue(intensity));
                        counter++;
                        //if warmup or cooldown timer is exhausted, mark current section as completed
                        if (runTimePredefinedWorkout.ChooseWarmupOrCooldownDuration(isWarmupAction) >= time)
                        {

                            selectedActionCompletion = true;
                        }

                    }
                    else
                    {
                        continue;
                    }
                }

            }
        }
    }

   

    private void FillOutCoreActions30PercentOnly()
    {
        //TODO: Use Common code for warmup, cooldown, core30 and core70 list fill up 
        if (!isExactCoreActionsFilled)
        {
            while (runTimePredefinedWorkout.coreActionDuration_30 <= coreActionDuration_30Percent)
            {
                if (Mathf.Abs((runTimePredefinedWorkout.coreActionDuration_30 - coreActionDuration_30Percent)) <= 15f)
                {
                    coreActionDuration_70Percent += Mathf.Abs((runTimePredefinedWorkout.coreActionDuration_30 - coreActionDuration_30Percent));
                    return;
                }
                    
               //rename x
                SelectedActionsForWorkout selectedActionsForWorkout = null;
                int selectedActionIndex = Random.Range(0, SelectedIntensityWiseCoreActionList.Count - 1);
                selectedActionsForWorkout = new SelectedActionsForWorkout(
                   SelectedIntensityWiseCoreActionList[selectedActionIndex].actionId,
                   SelectedIntensityWiseCoreActionList[selectedActionIndex].timePerIntensityLevels.ReturnIntensityValue(selectedIntensity),
                   false,
                   SelectedActionsForWorkout.WorkoutType.Core,
                   SelectedIntensityWiseCoreActionList[selectedActionIndex].intensityScale
                   );
                if (!CheckIfValueExistsInList(selectedActionsForWorkout))
                {
                    runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                    runTimePredefinedWorkout.coreActionDuration_30 += SelectedIntensityWiseCoreActionList[selectedActionIndex].timePerIntensityLevels.ReturnIntensityValue(selectedIntensity);
                    counter++;
                    if (runTimePredefinedWorkout.coreActionDuration_30 >= coreActionDuration_30Percent)
                    {

                        isExactCoreActionsFilled = true;

                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }

    private void FillOutCoreActions30PercentOnly_IfListExhausted()
    {
        //TODO: Use Common code for warmup, cooldown, core30 and core70 list fill up 
        if (!isExactCoreActionsFilled)
        {
            while (runTimePredefinedWorkout.coreActionDuration_30 <= coreActionDuration_30Percent)
            {
                if (Mathf.Abs((runTimePredefinedWorkout.coreActionDuration_30 - coreActionDuration_30Percent)) <= 15f)
                {
                    coreActionDuration_70Percent += Mathf.Abs((runTimePredefinedWorkout.coreActionDuration_30 - coreActionDuration_30Percent));
                    return;
                }

                //rename x
                SelectedActionsForWorkout selectedActionsForWorkout = null;
                ActionsInfo selectedActionIndex = SelectedIntensityWiseCoreActionList[SelectRandomIndexForActions()];
                selectedActionsForWorkout = new SelectedActionsForWorkout(
                   selectedActionIndex.actionId,
                   selectedActionIndex.timePerIntensityLevels.ReturnIntensityValue(selectedIntensity),
                   false,
                   SelectedActionsForWorkout.WorkoutType.Core,
                   selectedActionIndex.intensityScale
                   );

                runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                runTimePredefinedWorkout.coreActionDuration_30 += selectedActionIndex.timePerIntensityLevels.ReturnIntensityValue(selectedIntensity);
                counter++;
                if (runTimePredefinedWorkout.coreActionDuration_30 >= coreActionDuration_30Percent)
                {

                    isExactCoreActionsFilled = true;

                }

                //if (!CheckIfValueExistsInList_Twice(selectedActionsForWorkout))
                //{
                    
                //}
                //else
                //{
                //    continue;
                //}
            }
        }
    }

    private void FillOutCoreActions70PercentOnly()
    {
        
        if (!isCoreActionsFilled)
        {
            int intternalCounter = 0;
            Debug.LogError("Core 70 Counts : " + runTimePredefinedWorkout.selectedActionsForWorkouts.Count);
            while (runTimePredefinedWorkout.coreActionDuration_70 <= coreActionDuration_70Percent)
            { // why random is used
                int x = -1;
                SelectedActionsForWorkout selectedActionsForWorkout = null;
                x = Random.Range(0, coreActionList.Count - 1);
                selectedActionsForWorkout = new SelectedActionsForWorkout(
                   coreActionList[x].actionId,
                   coreActionList[x].timePerIntensityLevels.ReturnIntensityValue(selectedIntensity),
                   false,
                   SelectedActionsForWorkout.WorkoutType.Core,
                   coreActionList[x].intensityScale
                   );


                try
                {
                    if (!CheckIfValueExistsInList(selectedActionsForWorkout))
                    {
                        runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                        runTimePredefinedWorkout.coreActionDuration_70 += coreActionList[x].timePerIntensityLevels.ReturnIntensityValue(selectedIntensity);
                        counter++;
                        if (runTimePredefinedWorkout.coreActionDuration_70 >= coreActionDuration_70Percent)
                        {

                            isCoreActionsFilled = true;


                        }

                    }
                    else
                    {
                        continue;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Curriculum Gen Error : " + e.StackTrace + "\nMessage : " + e.Message);
                }
            }
            Debug.LogError("Core 70 Counts : " + runTimePredefinedWorkout.selectedActionsForWorkouts.Count);
        }
    }

    private void FillOutCoreActions70PercentOnly_IfListExhausted()
    {

        if (!isCoreActionsFilled)
        {
            int intternalCounter = 0;
            while (runTimePredefinedWorkout.coreActionDuration_70 <= coreActionDuration_70Percent)
            { // why random is used
                int x = -1;
                SelectedActionsForWorkout selectedActionsForWorkout = null;
                x = Random.Range(0, coreActionList.Count - 1);
                selectedActionsForWorkout = new SelectedActionsForWorkout(
                   coreActionList[x].actionId,
                   coreActionList[x].timePerIntensityLevels.ReturnIntensityValue(selectedIntensity),
                   false,
                   SelectedActionsForWorkout.WorkoutType.Core,
                   coreActionList[x].intensityScale
                   );


                try
                {
                    if (runTimePredefinedWorkout.selectedActionsForWorkouts.Count > 0)
                    {
                        if (runTimePredefinedWorkout.selectedActionsForWorkouts[runTimePredefinedWorkout.selectedActionsForWorkouts.Count - 1].actionId != selectedActionsForWorkout.actionId)
                        {
                            runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                            runTimePredefinedWorkout.coreActionDuration_70 += coreActionList[x].timePerIntensityLevels.ReturnIntensityValue(selectedIntensity);
                            counter++;
                            if (runTimePredefinedWorkout.coreActionDuration_70 >= coreActionDuration_70Percent)
                            {

                                isCoreActionsFilled = true;


                            }
                        }
                    }
                    else
                    {
                        runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                        runTimePredefinedWorkout.coreActionDuration_70 += coreActionList[x].timePerIntensityLevels.ReturnIntensityValue(selectedIntensity);
                        counter++;
                        if (runTimePredefinedWorkout.coreActionDuration_70 >= coreActionDuration_70Percent)
                        {

                            isCoreActionsFilled = true;


                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Curriculum Gen Error : " + e.StackTrace + "\nMessage : " + e.Message);
                }
            }
        }
    }

    private void FillStrtechingActionsWorkout()
    {
        //TODO: Use Common code for warmup, cooldown, core30 and core70 list fill up 
        if (!isStretchingFilled)
        {
            while (runTimePredefinedWorkout.stretchTime <= totalStretchTime)
            {
                if (Mathf.Abs((runTimePredefinedWorkout.stretchTime - totalStretchTime)) <= 15f)
                {
                    return;
                }

                //rename x
                SelectedActionsForWorkout selectedActionsForWorkout = null;
                int selectedActionIndex = Random.Range(0, stretchingActionList.Count - 1);
                selectedActionsForWorkout = new SelectedActionsForWorkout(
                   stretchingActionList[selectedActionIndex].actionId,
                   stretchingActionList[selectedActionIndex].stretchTime,
                   false,
                   SelectedActionsForWorkout.WorkoutType.Stretch,
                   stretchingActionList[selectedActionIndex].intensityScale
                   );
                if (!CheckIfValueExistsInList(selectedActionsForWorkout))
                {
                   
                    runTimePredefinedWorkout.stretchTime += stretchingActionList[selectedActionIndex].stretchTime;
                    runTimePredefinedWorkout.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                    counter++;
                    if (runTimePredefinedWorkout.stretchTime >= totalStretchTime)
                    {

                        isStretchingFilled = true;

                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }
    private bool CheckIfValueExistsInList(SelectedActionsForWorkout selectedActionsForWorkout)
    {
        bool exists = false;

        foreach (SelectedActionsForWorkout item in runTimePredefinedWorkout.selectedActionsForWorkouts)
        {
            if (item.actionId == selectedActionsForWorkout.actionId)
            {
                exists = true;
                return exists;
            }
            else
            {
                exists = false;
            }
        }

        return exists;
    }

    private bool CheckIfValueExistsInList_Twice(SelectedActionsForWorkout selectedActionsForWorkout)
    {
        int count = 0;
        bool exists = false;

        foreach (SelectedActionsForWorkout item in runTimePredefinedWorkout.selectedActionsForWorkouts)
        {
            if (item.actionId == selectedActionsForWorkout.actionId)
            {
                count++;
                
                
            }
            else
            {
                exists = false;
            }
        }
        if (count > 2)
        {
            exists = true;
            //return exists;
        }
        return exists;
    }

    private bool IsWarmupOrCooldownSelected(bool isWarmupSelected)
    {
        return isWarmupSelected ? true : false;
    }

    public void ResetRuntimePredefinedWorkouts()
    {
        counter = 0;
        runTimePredefinedWorkout.ClearDataOfWorkout();
        isWarmupActionsFilled = false;
        isExactCoreActionsFilled = false;
        isCoreActionsFilled = false;
        isCooldownFilled = false;
        warmupActionList.Clear();
        coreActionList.Clear();
        coolDownActionList.Clear();
        SelectedIntensityWiseCoreActionList.Clear();
    }

    private int SelectRandomIndexForActions()
    {
        if (SelectedIntensityWiseCoreActionList.Count <= 1)
            return 0;
        int x = lastSelectedRandom;
        while (x == lastSelectedRandom)
        {
            x = Random.Range(0, SelectedIntensityWiseCoreActionList.Count);
        }
        lastSelectedRandom = x;
        return x;
    }


    private int SelectRandomIndexForActionsFor70()
    {
        if (SelectedIntensityWiseCoreActionList.Count <= 1)
            return 0;
        int x = lastSelectedRandom_For70;
        while (x == lastSelectedRandom_For70)
        {
            x = Random.Range(0, SelectedIntensityWiseCoreActionList.Count);
        }
        lastSelectedRandom_For70 = x;
        return x;
    }

}
