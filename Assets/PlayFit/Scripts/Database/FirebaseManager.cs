using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class FirebaseManager
{
    // public static FirebaseManager instance { get; set; }



    public static void  GetWorkoutData(float userTime, string userIntensity)
    {
        //Will accept intensity and time
        Debug.Log("Get Workout");
        Debug.Log("Time " + userTime);
        Debug.Log("Intensity " + userIntensity);

    }

    public static void GetActionData()
    {

    }


    //Define parameters later
    public static void PostSession()  
    {

    }

    public static void GetMilestones()
    {

    }

    public static void GetPlayerSavedData()
    {

    }

    public static void PostPlayerProgress()
    {

    }

    public static void GetTTSDialogues()
    {

    }

}
