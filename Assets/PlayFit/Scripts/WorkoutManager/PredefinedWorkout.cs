using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PredefinedWorkout", menuName = "Create PredefinedWorkout")]
public class PredefinedWorkout : ScriptableObject
{
   [SerializeField] private List<workout> workout;
    [SerializeField] private float totalTime;
    [SerializeField] private string intensity;

    public List<workout> Workout { get => workout; set => workout = value; }
    public float TotalTime { get => totalTime; set => totalTime = value; }
    public string Intensity { get => intensity; set => intensity = value; }

    public void ResetWorkout()
    {
        foreach (workout w in Workout)
        {
            foreach (actions a in w.actions)
            {
                a.isCompleted = false;
            }
        }
    }

}

[System.Serializable]
public class workout
{
    public int workoutId;
    public int age;
    public float totalTimeFromBackend;
    public string intesityFromBackend;
    public List<actions> actions;
    public float workoutBreak; 

}

[System.Serializable]
public class actions
{
    public string actionId;
   // public string actionName;
    public float actionTime;
    public bool isCompleted = false;
}

