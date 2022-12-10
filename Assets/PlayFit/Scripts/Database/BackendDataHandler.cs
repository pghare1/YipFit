using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

public class BackendDataHandler : MonoBehaviour
{
    public static BackendDataHandler instance { get; set; }

    public YipliConfig currentYipliConfig;
    public WorkoutConfig workoutConfig;
    public playerProgress playerProgress;
    public PredefinedWorkoutManager predefinedWorkout;
    public PauseWorkoutManager pauseWorkout;
    public RunTimePredefinedWorkout runTimePredefinedWorkout;
    public List<WorkoutDataForFirebase> workoutData = new List<WorkoutDataForFirebase>();
    public string gotFeedback = "";

    public string guid = null;

    static Firebase.Auth.FirebaseAuth auth;

    const string VOICE_OVER_ACCENT_TYPE = "M";

    public string WORKOUT_TYPE = "full";

    bool isStored = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;



        //FindObjectOfType<UIManager>().FillCaloriesAndWorkoutPoints(playerProgress.totalCalories, playerProgress.noOfWorkoutComplete);



    }

    public async Task GetData()
    {
        DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        DataSnapshot dataSnapshot = PlayerSession.Instance.GetGameData(); 
        //DataSnapshot gameDatashot = 

        //try
        //{
        //    if (gameDatashot.Value != null)
        //    {
        //        playerProgress.newPlayer = gameDatashot.Child("is-fresh-player").Value.ToString() ?? "1";
        //    }
        //    else
        //    {
        //        playerProgress.newPlayer = "1";
        //    }
        //}
        //catch (Exception e)
        //{
        //    playerProgress.newPlayer = "1";
        //}

        //if (playerProgress.newPlayer.Equals("0"))
        //{
        //    Debug.LogError("Player id : " + PlayerSession.Instance.GetCurrentPlayerId());
        //    try
        //    {
        //        databaseReference = databaseReference.Child("fgd")?.Child(currentYipliConfig.userId)?
        //            .Child(PlayerSession.Instance.GetCurrentPlayerId())?.Child(currentYipliConfig.gameId)?.Child("workout-summary");
        //        dataSnapshot = await databaseReference.GetValueAsync() ?? null;
        //    }
        //    catch (Firebase.FirebaseException e)
        //    {
        //        Debug.LogError("GetData : " + e.StackTrace + " message " + e.Message);
        //        playerProgress.noOfWorkoutComplete = 0;
        //        playerProgress.totalCalories = 0f;
        //        playerProgress.totalFitnessPoints = 0f;
        //        playerProgress.totalDurationOfGame = 0f;
        //        playerProgress.unfinishedWorkoutNumbers = 0;
        //    }
        //}
        //else
        //{
        //    playerProgress.noOfWorkoutComplete = 0;
        //    playerProgress.totalCalories = 0f;
        //    playerProgress.totalFitnessPoints = 0f;
        //    playerProgress.totalDurationOfGame = 0f;
        //    playerProgress.unfinishedWorkoutNumbers = 0;
        //}

        float fp, cal, dura;
        int ufwk;
        try
        {
            if (dataSnapshot.Value != null)
            {
                if (dataSnapshot.Child("workout-summary").Value != null)
                {
                    DataSnapshot summarySnapshot = dataSnapshot.Child("workout-summary");
                    playerProgress.noOfWorkoutComplete = int.Parse(summarySnapshot.Child("workout-complete").Value?.ToString() ?? "0");
                    float.TryParse(summarySnapshot.Child("total-calories").Value?.ToString() ?? "0", out cal);
                    playerProgress.totalCalories = cal;
                    float.TryParse(summarySnapshot.Child("total-duration").Value?.ToString() ?? "0", out dura);
                    playerProgress.totalDurationOfGame = dura;

                    float.TryParse(summarySnapshot.Child("total-fitness").Value?.ToString() ?? "0", out fp);
                    playerProgress.totalFitnessPoints = fp;
                    int.TryParse(summarySnapshot.Child("unfinished-workout").Value?.ToString() ?? "0", out ufwk);
                    playerProgress.unfinishedWorkoutNumbers = ufwk;
                }
                else
                {
                    playerProgress.noOfWorkoutComplete = 0;
                    playerProgress.totalCalories = 0f;
                    playerProgress.totalFitnessPoints = 0f;
                    playerProgress.totalDurationOfGame = 0f;
                    playerProgress.unfinishedWorkoutNumbers = 0;
                }
            }
            else
            {
                playerProgress.noOfWorkoutComplete = 0;
                playerProgress.totalCalories = 0f;
                playerProgress.totalFitnessPoints = 0f;
                playerProgress.totalDurationOfGame = 0f;
                playerProgress.unfinishedWorkoutNumbers = 0;
            }
        }
        catch (System.Exception e)
        {
            playerProgress.noOfWorkoutComplete = 0;
            playerProgress.totalCalories = 0f;
            playerProgress.totalFitnessPoints = 0f;
            playerProgress.totalDurationOfGame = 0f;
            playerProgress.unfinishedWorkoutNumbers = 0;
        }
        
    }

    public async void StoreData(int workoutCompleted, float totalCalories, float totalFitnessPoint, float totalDuration, int unfinishedWorkout, 
        string feedback, int nonPerformingCount, int pauseTime = 0, bool isWorkoutCompleted = false, int unfinishedWorkoutDuration = 0)
    {
       
            playerProgress.noOfWorkoutComplete = workoutCompleted;
            playerProgress.totalCalories = totalCalories;
            playerProgress.totalFitnessPoints = totalFitnessPoint;
            playerProgress.totalDurationOfGame = totalDuration;
            playerProgress.unfinishedWorkoutNumbers = unfinishedWorkout;
            playerProgress.nonPerformingCount = nonPerformingCount;
            Dictionary<string, object> data = new Dictionary<string, object>();

            data.Add("workout-complete", playerProgress.noOfWorkoutComplete);
            data.Add("total-calories", (int)playerProgress.totalCalories);
            data.Add("total-fitness", (int)playerProgress.totalFitnessPoints);
            data.Add("total-duration", (int)playerProgress.totalDurationOfGame);
            data.Add("unfinished-workout", playerProgress.unfinishedWorkoutNumbers);
            data.Add("non-performing-count", playerProgress.nonPerformingCount);

        float fitnessPoint = (PlayerSession.Instance.GetFitnessPoints() + predefinedWorkout.OverallFitnessPointsOfWorkout);
            await StoreWorkoutSummary(currentYipliConfig.userId, PlayerSession.Instance.GetCurrentPlayerId(), currentYipliConfig.gameId, data);
            StoreWorkoutData(workoutConfig.intensity, workoutConfig.workoutTotalTime, predefinedWorkout.displayCaloriesTillNow,
                    fitnessPoint, playerProgress.nonPerformingCount, gotFeedback, pauseTime, isWorkoutCompleted, unfinishedWorkoutDuration);


            DataTrackingCheck.trackingHasDone = true;
            if (YipliHelper.checkInternetConnection())
            {

            }
        
    }

    public async void StoreWorkoutData(string intensity, float duration, float calories, 
        float fpPoint, int nonPerformingCount, string feedback = "",
        int pauseTime = 0, bool workoutCompleted = false, int unfinishedWorkoutDuration = 0)
    {
        Dictionary<string, WorkoutDataForFirebase> actionChronologyDicitionary = new Dictionary<string, WorkoutDataForFirebase>();
        Dictionary<string, object> workoutHistoryDicitionary = new Dictionary<string, object>();
        int i = 1;
        workoutHistoryDicitionary.Add("voice-over", VOICE_OVER_ACCENT_TYPE);
        workoutHistoryDicitionary.Add("time-stamp", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString());
        if(intensity != null || intensity != "" || WORKOUT_TYPE != "stretch" || WORKOUT_TYPE != "light")
            workoutHistoryDicitionary.Add("intesity", intensity);
        workoutHistoryDicitionary.Add("duration", (int)duration);
        workoutHistoryDicitionary.Add("calories", (int)calories);
        workoutHistoryDicitionary.Add("fitness-point", (int)fpPoint);
        workoutHistoryDicitionary.Add("non-performing-count", (int)predefinedWorkout.nonPerformingCounter);
        workoutHistoryDicitionary.Add("pause-time-taken", (int)(pauseWorkout.pauseTimerDefined - pauseWorkout.pauseTime));
        workoutHistoryDicitionary.Add("is-workout-completed", workoutCompleted);
        workoutHistoryDicitionary.Add("workout-feedback", feedback);
        workoutHistoryDicitionary.Add("workout-type", WORKOUT_TYPE);
        if(unfinishedWorkoutDuration > 0)
            workoutHistoryDicitionary.Add("unfinished-workout-duration", unfinishedWorkoutDuration);
        foreach (SelectedActionsForWorkout item in runTimePredefinedWorkout.selectedActionsForWorkouts)
        {
            //dictionary.Add(i.ToString(), new WorkoutDataForFirebase(item.actionId, item.currentWorkoutActionDuration, item.intensityNumber));
            string str = JsonUtility.ToJson(new WorkoutDataForFirebase(item.actionId, item.currentWorkoutActionDuration, item.intensityNumber));
            Debug.LogError("Got Json Data : " + str);
            
            actionChronologyDicitionary.Add(i.ToString(), JsonUtility.FromJson<WorkoutDataForFirebase>(str));
            i++;
        }
        //dictionary.Add("")
        
        await StoreWorkoutHistory(currentYipliConfig.userId, playerProgress.playerId, currentYipliConfig.gameId, workoutHistoryDicitionary, actionChronologyDicitionary);
        //runTimePredefinedWorkout.ClearDataOfWorkout();
    }

    private async Task StoreWorkoutHistory(string userid, string playerid, string gameid, Dictionary<string, object> workoutHistory, Dictionary<string, WorkoutDataForFirebase> actionChronology)
    {
        Debug.LogError("trying to  Add data");

        await auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            guid = System.Guid.NewGuid().ToString();
            DatabaseReference pathRef = reference.Child("fgd").Child(userid).Child(playerid).Child("yipfit").Child("workout-history").Child(guid);
            await pathRef.SetValueAsync(workoutHistory);
            await pathRef.Child("action-chronology").SetRawJsonValueAsync(JsonConvert.SerializeObject(actionChronology, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })); ;
            Debug.LogError("Successfully Added data");
            DataTrackingCheck.trackingHasDone = true;
        });
        //return 0;
    }

    private async Task StoreWorkoutSummary(string userid, string playerid, string gameid, Dictionary<string, object> workoutSummary)
    {
        Debug.LogError("trying to  Add data");

        await auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            await reference.Child("fgd/").Child(userid).Child(playerid).Child("yipfit/").Child("/workout-summary/").SetRawJsonValueAsync(JsonConvert.SerializeObject(workoutSummary, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })); ;
            Debug.LogError("Successfully Added data");
            DataTrackingCheck.trackingHasDone = true;
        });
        //return 0;
    }

    public async void StoreWorkoutHistoryFeedBack(string userid, string playerid)
    {
        Dictionary<string, object> workoutHistoryDicitionary = new Dictionary<string, object>();
        workoutHistoryDicitionary.Add("workout-feedback", gotFeedback);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        await reference.Child("fgd/").Child(userid).Child(playerid).Child("yipfit/").Child("workout-history").Child(guid).UpdateChildrenAsync(workoutHistoryDicitionary);
    }

}

[System.Serializable]
public class WorkoutDataForFirebase
{
    public string id;
    public float duration;
    public int intensity;

    public WorkoutDataForFirebase(string actionId, float currentWorkoutActionDuration, int intensityNumber)
    {
        this.id = actionId;
        this.duration = currentWorkoutActionDuration;
        this.intensity = intensityNumber;
    }

}
