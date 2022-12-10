using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionMappedTargetAreaAndIntensity", menuName = "Create ActionMappedTargetAreaAndIntensity")]
public class ActionMappedTargetAreaAndIntensity :  ScriptableObject
{
    public List<ActionsInfo> ActionList;

}

[System.Serializable]
public class ActionsInfo
{
    public string actionName;
    public string actionId;
    public int intensityScale;
    public enum BodyPart { FULLBODY, LOWERBODY, UPPERBODY}
    public BodyPart bodyPart;
    //public string bodyPart;
    //public string targetSection;
    public enum TargetSection { WARMUP, COREACTION, COOLDOWN}
    public TargetSection targetSection;
    public bool isStretch = false;
    public TimePerIntensityLevel timePerIntensityLevels;
    public int stretchTime = 50;


    // public TimePerIntensityLevel timePerIntensityLevel;
}


[System.Serializable]
public class TimePerIntensityLevel
{
    public float LowIntensity;
    public float mediumIntensity;
    public float highIntensity;

    public float ReturnIntensityValue(string intensity)
    {
        switch (intensity)
        {
            case "low":
                return LowIntensity;
            case "medium":
                return mediumIntensity;
            case "high":
                return highIntensity;
            default:
                return -1f;
        }
    }

}



//[System.Serializable]
//public class TimePerIntensityLevel
//{
//    public enum IntesityLevel { LOW, MED, HIGH}
//    public IntesityLevel intesityLevel;
//    public float time;
//}
