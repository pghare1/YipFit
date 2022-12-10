using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkoutTimeAndIntensity", menuName = "Create WorkoutTimeAndIntensity")]
public class WorkoutTimeAndIntensity : ScriptableObject
{
    public List<IntensityWiseDuration> IntensityWiseDuration;
}

[System.Serializable]
public class IntensityWiseDuration
{
    public enum IntesityLevel { LOW, MED, HIGH }
    public IntesityLevel intesityLevel;
    public float breakTime;
    public float breakTriggerTime;
}
