using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ActionInfo", menuName = "Create ActionInfo")]
public class ActionInfo : ScriptableObject
{
    [SerializeField] private string actionName;
    [SerializeField] private string workoutType;
    [SerializeField] private string workoutIntensity;
    [SerializeField] private string ageCategory;
    [SerializeField] private float workoutTime;
    [SerializeField] private string accuracy;
    [SerializeField] private string sex;

    public string ActionName { get => actionName; set => actionName = value; }
    public string WorkoutType { get => workoutType; set => workoutType = value; }
    public string WorkoutIntensity { get => workoutIntensity; set => workoutIntensity = value; }
    public string AgeCategory { get => ageCategory; set => ageCategory = value; }
    public float WorkoutTime { get => workoutTime; set => workoutTime = value; }
    public string Accuracy { get => accuracy; set => accuracy = value; }
    public string Sex { get => sex; set => sex = value; }
}
