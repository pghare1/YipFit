using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerProgress", menuName = "Create playerProgress")]
public class playerProgress : ScriptableObject
{
    public string playerId;
    public float privousTimeSelectedByUser;
    public string privousIntensitySelectedByUser;
    public string previousFeedback;
    public int noOfWorkoutComplete = 0;
    public float totalCalories;
    public float totalFitnessPoints;
    public float totalDurationOfGame;
    public float totalCaloriesCount;
    public float lastAvgWorkoutIntensity;
    public int unfinishedWorkoutNumbers;
    public int nonPerformingCount = 0;
    public string newPlayer = "1";

    public float PrivousTimeSelectedByUser { get => privousTimeSelectedByUser; set => privousTimeSelectedByUser = value; }
    public string PrivousIntensitySelectedByUser { get => privousIntensitySelectedByUser; set => privousIntensitySelectedByUser = value; }
    public string PreviousFeedback { get => previousFeedback; set => previousFeedback = value; }
}
