using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunTimePredefinedWorkout", menuName = "Create RunTimePredefinedWorkout")]
public class RunTimePredefinedWorkout : ScriptableObject
{
    public List<SelectedActionsForWorkout> selectedActionsForWorkouts;
    public float warmupActionDuration;
    public float coreActionDuration_30;
    public float coreActionDuration_70;
    public float coolDownActionDuration;
    public float stretchTime;
    public float ChooseWarmupOrCooldownDuration(bool isWarmupSelected)
    {
        if (isWarmupSelected)
            return warmupActionDuration;
        else
            return coolDownActionDuration;
    }

    public void SetWarmupOrCooldownDuration(bool isWarmupSelected, float time)
    {
        if (isWarmupSelected)
            warmupActionDuration += time;
        else
            coolDownActionDuration += time;
    }

    public void ClearDataOfWorkout()
    {
        selectedActionsForWorkouts.Clear();
        warmupActionDuration = 0f;
        coreActionDuration_30 = 0f;
        coreActionDuration_70 = 0f;
        coolDownActionDuration = 0f;
        stretchTime = 0f;
    }

}

[System.Serializable]
public class SelectedActionsForWorkout
{
    public string actionId;
    public float currentWorkoutActionDuration;
    public bool isActionCompleted;
    public enum WorkoutType { Warmup, Core, Cooldown, Stretch}
    public WorkoutType workoutType;
    public int intensityNumber;

    public SelectedActionsForWorkout(string actionId, float currentWorkoutActionDuration,
        bool isActionCompleted, WorkoutType workoutType, int intensityNumber)
    {
        this.actionId = actionId;
        this.currentWorkoutActionDuration = currentWorkoutActionDuration;
        this.isActionCompleted = isActionCompleted;
        this.workoutType = workoutType;
        this.intensityNumber = intensityNumber;
    }

}
