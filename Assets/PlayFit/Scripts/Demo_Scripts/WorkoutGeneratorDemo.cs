using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutGeneratorDemo : MonoBehaviour
{
  
    public RunTimePredefinedWorkout runTimePredefined;
    public RunTimePredefinedWorkout actualWorkoutRunTimeScript;
    public WorkoutConfig workoutConfig;
    public float warmUpTime;
    public float coreWorkoutTime;
    public float coolDownTime;
    public float newTime = 0f;
    public ActionMappedTargetAreaAndIntensity actionMappedTargetAreaAndIntensity;


    // Start is called before the first frame update
    void Start()
    {
        runTimePredefined.ClearDataOfWorkout();
        AssignSectionWiseTime();
        FillDemoWorkoutList();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDemoWorkout()
    {
        Debug.LogError("Demo button");
        workoutConfig.time = coreWorkoutTime;
        workoutConfig.currentSelectedTime = coreWorkoutTime;
        actualWorkoutRunTimeScript.selectedActionsForWorkouts = runTimePredefined.selectedActionsForWorkouts;
    }


    public void AssignSectionWiseTime()
    {
        warmUpTime = 0f;
        coreWorkoutTime = 120f;
        coolDownTime = 0f;

    }

    public void FillDemoWorkoutList()
    {
        foreach (ActionsInfo info in actionMappedTargetAreaAndIntensity.ActionList)
        {
            if (newTime <= coreWorkoutTime)
            {
                if (info.targetSection == ActionsInfo.TargetSection.COREACTION)
                {
                    if (info.intensityScale < 8)
                    {
                        SelectedActionsForWorkout selectedActionsForWorkout
                            = new SelectedActionsForWorkout(info.actionId, info.timePerIntensityLevels.LowIntensity,
                            false, SelectedActionsForWorkout.WorkoutType.Core, info.intensityScale);
                        runTimePredefined.selectedActionsForWorkouts.Add(selectedActionsForWorkout);
                        newTime += info.timePerIntensityLevels.LowIntensity;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        //actualWorkoutRunTimeScript.selectedActionsForWorkouts = runTimePredefined.selectedActionsForWorkouts;
    }
}
