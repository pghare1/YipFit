using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curriculumManager : MonoBehaviour
{
    [SerializeField] private List<PredefinedWorkout> allWorkouts;// = new List<PredefinedWorkout>();
     public int currentWorkoutIndex = 0;
    [SerializeField]
    private List<PredefinedWorkout> matchingWorkouts;

    private void Start()
    {
        ResetAllWorkouts();
    }

    public PredefinedWorkout GetProperWorkout(float time, string intensity)
    {
        matchingWorkouts = new List<PredefinedWorkout>();
        foreach (PredefinedWorkout workout in allWorkouts)
        {
            
            if(workout.TotalTime == time && workout.Intensity == intensity)
            {  
                matchingWorkouts.Add(workout);
            }
        }

      int  randomNumber = Random.Range(0, matchingWorkouts.Count);
        return matchingWorkouts[randomNumber];

    }

    public void ResetAllWorkouts()
    {
        foreach (PredefinedWorkout workout in allWorkouts)
        {
            foreach (workout w in workout.Workout)
            {
                foreach (actions a in w.actions)
                {
                    a.isCompleted = false;
                }
            }
        }
    }

}
