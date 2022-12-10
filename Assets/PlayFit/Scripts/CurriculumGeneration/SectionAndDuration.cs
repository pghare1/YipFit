using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SectionAndDuration", menuName = "Create SectionAndDuration")]
public class SectionAndDuration : ScriptableObject
{
    // public List<Section> Section;
    public Section[] sections;

}

//[System.Serializable]
//public class SectionInfo
//{
//    public Section section;
//}

[System.Serializable]
public class Section
{
    
    public enum IntesityLevel { LOW, MED, HIGH }
    public IntesityLevel intesityLevel;
    public float WarmUp;
    public float CoreAction;
    public float CoolDown;

}


