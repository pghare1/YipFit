using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class PredefinedWorkoutManager : MonoBehaviour
{
    const string LOW = "low";
    const string MEDIUM = "medium";
    const string HIGH = "high";
    [SerializeField] private CharacterManager characterManager = null;
    [SerializeField] private CharacterAnimationHandler characterAnimationHandler;

    public WorkoutConfig workoutConfig;
    public RunTimePredefinedWorkout predefinedWorkout;
    [SerializeField] private curriculumManager curriculumManager;
    [SerializeField] private RunTimePredefinedWorkout runTimePredefinedWorkout;
    
    [SerializeField] private WorkoutTimeAndIntensity workoutTimeAndIntensity;
    [SerializeField]
    private CheckPointManager checkPointManager;


    [SerializeField] private List<Button> startMenuButtons;
    public List<Button> optionsButtons;//all time and options and play button
    [SerializeField] private List<Button> suggestionPanelButtons;
    [SerializeField] private List<Button> feedbackPanelButtons;//all feedback buttons and  home button
    

    public actions currentWorkOutAction;
    public playerProgress playerProgress = null;
    public MatInputSystem matInputSystem = null;
    public UIManager uIManager = null;
    public PauseWorkoutManager pauseWorkoutManager = null;
    public CurriculumGenerator curriculumGenerator = null;
    public float timeSelectedByUser = 0f;
    private string intensitySelectedByUser = "";
  
    public GameObject landingPage;
    public GameObject optionsPanel;
    public GameObject gamePlayPanel;
    public GameObject loadingPanel;
    public GameObject resultPanel;
    public GameObject sugessionPanel;

    public TextMeshProUGUI totalCountDownTimer;
    public TextMeshProUGUI nameOfTheWorkout, caloriesCountText;//
    public TextMeshProUGUI totalNumberOfWorkoutCompleted;
    public TextMeshProUGUI totalCalories;
    public GameObject pauseButton;
    public GameObject previewPanel;
    public GameObject previewPanelMain;

    public TextMeshProUGUI nextActionName, nextActionIntenity, nextActionIntensityText, durationTextOnPreviewPanel;

    public bool workOutStarted = false;

    float timeInSec;
    float breakTimeInSec;

    public bool isActionWorkoutFetched = false;
    float currentActionWorkoutDuration = 0f;
    public int actionCounter = -1;
    public bool allWorkoutActionsCompleted = false;
    public bool isSessionCompleted = false;
    bool isBreakIdleAnimationApplied = false;
   

    public GameObject timeSelectedIs_10;
    public GameObject intensitySelectedIsLow;
    public GameObject warmupStrike, cooldowmStrike;
    public string getUserFeedback;
    public TextMeshProUGUI suggesionText, workoutSectionTextOnGamePlay;//
    public GameObject caloriesIcon;
    public GameObject buttonTime10;
    public GameObject buttonTime15;
    public GameObject buttonTime20;
    public GameObject intensityLow;
    public GameObject intensityMedium;
    public GameObject intensityHigh;

    public Sprite buttonSelected;
    public Sprite intensityButtonGrey;
    public Sprite timeButtonGrey;

    public Image progressBarForeGround;
    public float divideProgressBarValue = 25f;
    public float currentActionTime = 0f;
    public float perActionTimeCounter = 0f;

    [Header("Feedback Button Objects")]
    public GameObject veryEasyBtn, justRightBtn, tooHardBtn;

    public GameObject feedbackButtonPanel, feedbackThankYouPanel;

    private const float lowestWorkoutDuration = 10f;
    private const float highestWorkoutDuration = 10f;

    private const string lowestWorkoutIntensity = "low";
    private const string highestWorkoutIntensity = "high";

    bool lastSecondWorkoutCheck;

    public bool isBreakOn = false;
    public bool startWaitBetweenWorkout = false;
    public YipliConfig currentYipliConfig;
    float totalTransitionTime = 5f;

    public static bool isAppStartedForFirstTime = true;
    public GameObject caloriesCountOnCoreAction;
    public Animator progressBarBlinkingAnimator;

    public GameObject CaloriesObject, WorkoutObject;

    public float breakTime;
    public float triggerBreakAfterThisTime;
    public bool isbreakEligible;
    public bool isBreakCoroutineStarted = false;
    public bool isFlowInCurrentWorkout = false;
    public bool isBreakActivated = false;
    public bool isCoreActionInProgress = false;
    public float AddCaloriesForNotDevelopedActionTimer = 0f;
    public int countOfTimesAddCaloriesForNotDevelopedActions = 0;
    public float currentNotDevelopedActionTimer = 0f;
    public float totalWorkoutCalories = 0f, displayCaloriesTillNow = 0f;
    public float OverallFitnessPointsOfWorkout = 0f;
    public float totalCaloriesCountOfWarmup = 0f;
    public SoundManager soundManager;
    public bool audioEngagingVoicePlayed = false;

    bool isCalledWaitBetweenAction = false;

    bool isGoBackToIdleCalled = true;
    bool replayTheIdleAnimation = true;
    bool isBreakFunctionCalled = false;
    float calsPerSession = 0f;
    public AudioClip currentVoiceOverClip = null;
    public bool actionNameAudioPlayed = false;
    [SerializeField] private Animator characterAnimatorCanvas = null;
    [SerializeField] private AnimatorOverrideController[] idleOverrideControllersCanvas;
    [SerializeField] private Vector3 canvasModelRotationValues;

    public float previousCalories = 0f, newCaloriesValue = 0f;

    public bool playNonPerformingVoiceOvers = false;
    public bool inNonPerformingVoices = false;
    public bool dontPlayNonPerformVoices = false;
    private bool stoppedPlayNonPerfromingAtBreak = false;
    public int nonPerformingCounter = 0;
    public float rotationValue90Deg;
    public bool withoutDialogueBuild = true;

    public GameObject upComingWorkoutPanel;
    public TextMeshProUGUI currentWarmupText, currentCoreText, currentCooldownText;
    public GameObject currentWarmupObj, currentCoreObj, currentCooldownObj, stretchObj;
    public TextMeshProUGUI popUpText;
    public GameObject bleErrorPanel;
    public bool isMatDisconnected = false;
    public Button easyBtn, medBtn, hardBtn;
    public bool isNextSoundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        
        soundManager.MainMenuBg();
        //Time.timeScale = 2.5f;
        ShowNoOfWorkoutAndCalories();
        runTimePredefinedWorkout.ClearDataOfWorkout();
        curriculumGenerator.ResetRuntimePredefinedWorkouts();
        OnStartRefreshSprites();
        ChangeTimeButtonSpriteOnSelection("10");
        //GetPlayerData();
        PlayerSession.Instance.StartSPSession();
        workoutConfig.nonPerformingTime = 0f;
        previousCalories = newCaloriesValue;
        //canvasModelRotationValues = characterManager.characterModel.transform.eulerAngles;
        DataTrackingCheck.trackingHasDone = false;
        // matInputSystem.UpdateButtonList(startMenuButtons, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if (YipliHelper.GetMatConnectionStatus() != "Connected")
        //{
        //    Debug.LogError("Mat connection lost, pausing game");
        //    pauseWorkoutManager.PauseWorkout();
        //    return;
        //}



        Debug.LogError("Calling From Update");
        DisplayTotalTimeCountDown();
       
        if (isActionWorkoutFetched && workOutStarted)
        {
            if (isCoreActionInProgress)
            {
                breakCountDown();
               // timerForNotDevelopedAction();// For not developed Actions
            }
            
            TrackActionWorkout();
            
        }
        
        // ManageCurrentActionTimer();
        NonPerformingActionsVoiceOvers();
        PlayerSession.Instance.UpdateDuration();
    }


    //Get Time input and store it in Scriptable
    public void GetUserTime( float userTime)
    {
        timeSelectedByUser = userTime;
       
        Debug.Log("SelectedTime" + timeSelectedByUser);
        
        //if((userTime * 60) != workoutConfig.currentSelectedTime)
        //    ResetRunTimeWorkoutScriptable();

        StoreValuesInScriptable();
    }

    //Get Intensity input and store it in Scriptable
    public void GetUserIntensity(string userIntensity)
    {
        intensitySelectedByUser = userIntensity;
        Debug.Log("SelectedIntensity" + intensitySelectedByUser);
        //CurriculumGenerator.instance.selectedIntensity = "";

        //if (userIntensity != workoutConfig.currentSelectedIntensity)
        //    CurriculumGenerator.instance.selectedIntensity = "";

        StoreValuesInScriptable();
        workoutConfig.triggerBreakTimer = AllotBreakTimeAndBreakTriggerTime(intensitySelectedByUser);
    }

    public void GetDataFromFBBasedOnInPut()
    {
        FirebaseManager.GetWorkoutData(timeSelectedByUser, intensitySelectedByUser);
    }

    public void OpenOptionsPanel()
    {
        StartCoroutine(CallOptionsPanel());
        
        //characterManager.HideMainCharacter();
    }

    private IEnumerator CallOptionsPanel()
    {
        optionsPanel.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        soundManager.SelectDialgoue(DialoguesClassification.DialogueType.optionsPanelDialogues);
    }

  

    public void ResetWorkout()
    {
        Debug.LogError("Workout Start Time : Reset workout called");
    }


    //For storing values in Workout Config Scriptable
    public void StoreValuesInScriptable()
    {
        float timeInSec = timeSelectedByUser * 60;

        Debug.Log("Time in sec" + timeInSec);
        SetTime(timeInSec);
        workoutConfig.time = timeInSec;
        workoutConfig.intensity = intensitySelectedByUser;

        //if (timeInSec != workoutConfig.currentSelectedTime || intensitySelectedByUser != workoutConfig.currentSelectedIntensity)
        //{
        //    CurriculumGenerator.instance.ResetRuntimePredefinedWorkouts();
        //    
        //}


        workoutConfig.currentSelectedTime = timeInSec;
        workoutConfig.currentSelectedIntensity = intensitySelectedByUser;

    }
    

    public void EnableGamePlayScreen()
    {
        sugessionPanel.SetActive(false);
        StartCoroutine(EnableDisableResultScreen(false));
        optionsPanel.SetActive(false);
        uIManager.coreOptionsPanel.SetActive(false);
        uIManager.lightWorkoutOptionsPanel.SetActive(false);
        uIManager.OfferingsPanel.SetActive(false);
        loadingPanel.SetActive(false);
        StartCoroutine(ShowCinematics());
    }

    private IEnumerator ShowCinematics()
    {
        //TODO:Create Common Function For Text Manipulation
        upComingWorkoutPanel.SetActive(true);
        //if(CurriculumGenerator.instance.is20Secs)
        //{
        //    currentWarmupText.text = "     20 sec";
        //    currentCoreText.text = "     80 sec";
        //    currentCooldownText.text = "     20 sec";
        //}
        //else
        //{

        //}
       if(uIManager.inStretchingWorkout)
        {
            warmupStrike.SetActive(false);
            cooldowmStrike.SetActive(false);
            stretchObj.SetActive(true);
            currentWarmupObj.SetActive(false);
            currentCoreObj.SetActive(false);
            stretchObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "     " + ((predefinedWorkout.stretchTime) / 60f).ToString("0") + " Min";
            currentCooldownObj.SetActive(false);      
        }
       else if(uIManager.inCoreWorkout)
        {
            stretchObj.SetActive(false);
            currentWarmupText.text = "     " + (predefinedWorkout.warmupActionDuration / 60f).ToString("0") + " Min    ";
            warmupStrike.SetActive(true);
            currentCoreText.text = "     " + ((predefinedWorkout.coreActionDuration_30 + predefinedWorkout.coreActionDuration_70) / 60f).ToString("0") + " Min";
            currentCooldownText.text = "     " + (predefinedWorkout.coolDownActionDuration / 60f).ToString("0") + " Min    ";
            cooldowmStrike.SetActive(true);
        }
       else 
        {
            warmupStrike.SetActive(false);
            cooldowmStrike.SetActive(false);
            stretchObj.SetActive(false);
            currentWarmupText.text = "     " + (predefinedWorkout.warmupActionDuration / 60f).ToString("0") + " Min";
            currentCoreText.text = "     " + ((predefinedWorkout.coreActionDuration_30 + predefinedWorkout.coreActionDuration_70) / 60f).ToString("0") + " Min";
            currentCooldownText.text = "     " + (predefinedWorkout.coolDownActionDuration / 60f).ToString("0") + " Min";
        }
        

        characterManager.DisplayMainCharacter();
        characterManager.ApplyIdleAnimation();
        popUpText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(4.25f);
        soundManager.SelectDialgoue(DialoguesClassification.DialogueType.startCinematicsDialogues);
        popUpText.gameObject.SetActive(true);
        popUpText.text = "Let's";
        popUpText.transform.DOScale(new Vector2(3f, 2.25f), 0.45f).timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.65f);
        yield return new WaitForSecondsRealtime(0.1f);
        popUpText.text = "";
        popUpText.gameObject.GetComponent<RectTransform>().localScale = Vector2.one;
        yield return new WaitForSecondsRealtime(0.65f);
        popUpText.text = "Get";
        popUpText.transform.DOScale(new Vector2(3f, 2.25f), 0.45f).timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.65f);
        yield return new WaitForSecondsRealtime(0.1f);
        popUpText.text = "";
        popUpText.gameObject.GetComponent<RectTransform>().localScale = Vector2.one;
        yield return new WaitForSecondsRealtime(0.65f);
        popUpText.text = "Fit !";
        popUpText.transform.DOScale(new Vector2(3f, 2.25f), 0.45f).timeScale = 1f;
        yield return new WaitForSecondsRealtime(1f);
        yield return new WaitForSecondsRealtime(0.1f);
        popUpText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        upComingWorkoutPanel.transform.GetChild(0).transform.DOLocalMoveX(-1800f, 0.35f).timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.75f);
        upComingWorkoutPanel.SetActive(false);
        yield return new WaitForSecondsRealtime(0.75f);
        characterManager.ApplyProperOverrideCanvas(
                characterManager.CharacterAnimationHandlerCanvasOBJ.GetAnimationOverrideFromActionId(predefinedWorkout.selectedActionsForWorkouts[0].actionId),
                characterManager.CharacterAnimationHandlerCanvasOBJ.GetAnimationSpeed(predefinedWorkout.selectedActionsForWorkouts[0].actionId)

                );
        if (characterManager.CharacterAnimationHandlerCanvasOBJ.CheckCharacterAngleToBeSetOnCanvas(predefinedWorkout.selectedActionsForWorkouts[0].actionId))
        {
            characterManager.characterModelCanvas.transform.localEulerAngles
            = new Vector3(0f,
            270f, 0f);
        }
        else
        {
            characterManager.characterModelCanvas.transform.localEulerAngles
            = canvasModelRotationValues;
        }
        nextActionName.text = characterAnimationHandler.GetActionName(predefinedWorkout.selectedActionsForWorkouts[0].actionId);
        gamePlayPanel.SetActive(false);
        TurnOnOffPreviewPanel(true);
        soundManager.PlayActionNameAudio(predefinedWorkout.selectedActionsForWorkouts[0].actionId);
        yield return new WaitForSecondsRealtime(5f);
        GameplayStartProcesses();
    }

    private void GameplayStartProcesses()
    {
        //Suggesion panel is not shown on extreme conditions like 10 and low and feedback given is hard and same as of if 20 and High and feedback is it is was very easy 
        Debug.LogError("Workout Start Time : ENable gameplay called");

        //soundManager.WarmUpBg();
        //checkPointManager.TriggerAtStartOfWarmUp();

        //
        //if (Mathf.Approximately(playerProgress.privousTimeSelectedByUser, lowestWorkoutDuration) && playerProgress.privousIntensitySelectedByUser.Equals(lowestWorkoutIntensity))
        //{
        //    Debug.LogError("Low intensity start workout");
        //    OnWorkoutStart();
        //    return;
        //}
        //else if (Mathf.Approximately(playerProgress.privousTimeSelectedByUser, highestWorkoutDuration) && playerProgress.privousIntensitySelectedByUser.Equals(highestWorkoutIntensity))
        //{
        //    Debug.LogError("HIgh intensity start workout");
        //    OnWorkoutStart();
        //    return;
        //}else

        //TODO: Use suggesion panel before configuring workout
        //TODO: Instead Of If Else -> Switch Case
        //TODO: Use Text Box Instead Of Entire Suggesion Panel
        if (playerProgress.PreviousFeedback != null || playerProgress.PreviousFeedback != "" || playerProgress.PreviousFeedback != string.Empty)
        {
            //Depending on previous feedback if time and intensity matches with the previous workout Suggesion panel is shown according to previous feedback given
            if (timeSelectedByUser == playerProgress.PrivousTimeSelectedByUser && intensitySelectedByUser == playerProgress.PrivousIntensitySelectedByUser)
            {  //If time and intensity is same as of previous Workout and feedback is very easy
                if (playerProgress.PreviousFeedback == "veryEasy")
                {
                    //provide promt with very easy text
                    sugessionPanel.SetActive(true);
                    matInputSystem.UpdateButtonList(suggestionPanelButtons, 0);
                    suggesionText.text = "Previously on this selection, you found the workout was not very challenging! Would you like to try a higher intensity?";
                    Debug.LogError("player feedback hard higher");
                    return;
                }
                else if (playerProgress.PreviousFeedback == "tooHard")
                { //when previous given feedback is too hard
                    //provide promt with very hard text
                    sugessionPanel.SetActive(true);
                    matInputSystem.UpdateButtonList(suggestionPanelButtons, 0);
                    suggesionText.text = "Previously on this selection, you found the workout was quite the challenge! Would you like to choose a lower intensity?";
                    Debug.LogError("player feedback low higher");
                    return;
                }
            }


        }

        //if time and intensity doesnt match with the previous workout Start Workout without showing suggesion panel 
        Debug.LogError("Without and feedback");
        OnWorkoutStart();
    }

    public void SetTime(float time)
    {
       timeInSec = time;
    }
    public void DisplayTotalTimeCountDown()
    { 
        if (workoutConfig.time <= 0f || !gamePlayPanel.activeInHierarchy || !isActionWorkoutFetched || isBreakFunctionCalled)
            return;

        //coutdown for value which is set in workout Config 
        timeInSec = workoutConfig.time;
        // totalCountDownTimer.text = timeInSec.ToString();
        timeInSec -= Time.deltaTime;
        float countDown = timeInSec;
        
        float mins = Mathf.FloorToInt(countDown / 60f);
        float secs = Mathf.FloorToInt(countDown % 60f);

        if(mins < 1)
        {
            if (secs < 1)
                secs = 0;

            if(secs <= 9)
                totalCountDownTimer.text = "00" + ":" + "0" + secs.ToString("0");
            else
                totalCountDownTimer.text = "00" + ":" + secs.ToString("0");
        }
        else
        {
            //if counter goes beyond 1 min, then show only seconds value.....
            totalCountDownTimer.text = mins.ToString("00") + ":" + secs.ToString("00");
        }

        if(!isbreakEligible)
            isFlowInCurrentWorkout = true;

        workoutConfig.time = countDown;
        workoutConfig.currentSelectedTime = countDown;

    }

    //TODO: Convert Below Function in to parts
    //TODO: Implement Observer Pattern 
    //TODO: Remove ForEach loop 
    public void FetchWorkOutAction()
    { //checks whether breaks are eligible and is there any action working currently if not execute breaks part else follow the workout sequence
        if (isbreakEligible && !isFlowInCurrentWorkout)
        {
            //StartCoroutine(BreakIsActive());
            if (!isBreakActivated)
            {
                caloriesCountOnCoreAction.SetActive(false);
                // Calculate total time including breaks
                workoutConfig.totalTimeWithBreakIncluded += workoutConfig.breakTime;
                // On break is eligible and not called for break
                if(!isBreakFunctionCalled)
                    StartCoroutine(BreakIsActive()); 
            }
           
        }
        else if (!CheckIfAllWorkoutActionsAreCompleted())//Check if all actions are completed or not if not fetch another action
        {
            Debug.LogError("FetchWorkoutAction : All actions not completed");
            foreach (SelectedActionsForWorkout item in predefinedWorkout.selectedActionsForWorkouts)
            {
                //Check all actions from workout whether they all are completed, if true then the workout is completed.
                Debug.LogError("FetchWorkoutAction : Action selected " + item.actionId);
                if (!item.isActionCompleted)
                {
                    Debug.LogError("FetchWorkoutAction : Action completed");
                    //Shows which action is currently selected 
                    currentWorkOutAction.actionId = item.actionId;
                    // currentWorkOutAction.actionName = action.actionName;
                    currentWorkOutAction.actionTime = item.currentWorkoutActionDuration;
                    currentWorkOutAction.isCompleted = item.isActionCompleted;
                    Debug.LogError("FetchWorkoutAction : Action time " + item.currentWorkoutActionDuration);
                    Debug.LogError("FetchWorkoutAction TimeInSec " + timeInSec);
                    //Calculate current workout remaining time
                    //TODO: Use Int instead of Ceil and Abs
                    currentActionWorkoutDuration = Mathf.Ceil(Mathf.Abs(workoutConfig.currentSelectedTime - currentWorkOutAction.actionTime));
                    Debug.LogError("Current Action workout duration : " + currentActionWorkoutDuration);
                    currentActionTime = item.currentWorkoutActionDuration;
                    //Progress Bar 
                    StartCoroutine(ActionProgress());
                    StartCoroutine(CalculateTimingForActionVoice());
                    isActionWorkoutFetched = true;
                    workOutStarted = true;
                    workoutConfig.nonPerformingTime = 0f;//Timer set to 0 on start of every action
                    inNonPerformingVoices = false;
                    //Warmup called at start of workout
                    if(uIManager.inStretchingWorkout)
                    {
                        checkPointManager.workoutState = "S";
                    }
                    else if(uIManager.inCoreWorkout)
                    {
                        checkPointManager.workoutState = "C";
                    }
                    else
                    {
                        if (!checkPointManager.coreStarted)
                        {
                            checkPointManager.TriggerAtStartOfTheCoreSection();
                        }
                        //else if (!checkPointManager.midOfCore)
                        //{
                        //    checkPointManager.TriggerAtMidOfTheCore();
                        //}
                        //else if (!checkPointManager.endOfCore)
                        // {
                        //    checkPointManager.TriggerAtEndOfTheCore();
                        // }
                        else if (!checkPointManager.startOfCool)
                        {
                            checkPointManager.TriggerAtStartOfTheCooldown();
                        }
                    }
                    

                    

                    //Increment action counter to fetch next action 
                    //
                    actionCounter++;
                    


                    Debug.Log("Action Counter" + actionCounter);
                    Debug.LogError("FetchWorkoutAction Action Counter " + actionCounter);
                    lastSecondWorkoutCheck = IsItLastSecondWorkout();
                    // TODO: Change 999 -> Other Number(9999)
                    if (actionCounter >= 0)
                    {
                        if (characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId) == 999)
                        {

                            AddCaloriesForNonDetectableActionOnMat(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId,
                            predefinedWorkout.selectedActionsForWorkouts[actionCounter].currentWorkoutActionDuration);
                            AddFitnessPointsForNotDetectedActions(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId, predefinedWorkout.selectedActionsForWorkouts[actionCounter].currentWorkoutActionDuration);
                        }
                    }
                    break;
                }

            }
        }
        else
        {
            //here on workout Completion
            //Remove DisablePauseButtonFunction()
            allWorkoutActionsCompleted = true;
            TurnOnOffPreviewPanel(false);
            DisablePauseButton();
            Debug.LogError("FetchWorkoutAction : All actions completed ");
            //ShowResultScreen();
            //pauseWorkoutManager.EndWorkout();
            
        }

        if (!isGoBackToIdleCalled)
        {
            //Fetch animator only once 
            FindObjectOfType<CharacterAnimationEventHandler>().GoToIdleStart();
            FindObjectOfType<CharacterAnimationEventHandler>().ResetBoolForMainAction();
            isGoBackToIdleCalled = true;
        }

    }


    //Checks whether all workout actions are completed or not
    private bool CheckIfAllWorkoutActionsAreCompleted()
    {
        bool workoutsCompleted = false;

        for (int i = 0; i < predefinedWorkout.selectedActionsForWorkouts.Count; i++)
        {

            //Debug.Log("Workout Last Index " + predefinedWorkout.Workout[0].actions.LastIndexOf(predefinedWorkout.Workout[0].actions[actionCounter]));
            //Debug.Log("Workout Count " + predefinedWorkout.Workout[0].actions.Count);

            if (predefinedWorkout.selectedActionsForWorkouts[i].isActionCompleted)
            {
                workoutsCompleted = true;

                


            }
            else
            {
                workoutsCompleted = false;
            }
        }

        return workoutsCompleted;
    }

    private bool IsItLastSecondWorkout()
    {
        if (predefinedWorkout.selectedActionsForWorkouts.LastIndexOf(predefinedWorkout.selectedActionsForWorkouts[actionCounter]) < (predefinedWorkout.selectedActionsForWorkouts.Count-1))
        {
            return false;
        }
        else
        {
            if (!checkPointManager.endOfCore)
            {
                
            }
            return true;
        }


    }

    //It manages the animation of current action workout and input taken from the mat, Manages camera angle when required for 45 degrees
    //Convert BB to Enum
    //Combine Sound Managemnet and Animation management in similar Update Cycle 
    private void TrackActionWorkout()
    {
        Debug.LogError("Calling From In Track Workout Action");
        if(!allWorkoutActionsCompleted)
        {
            Debug.LogError("In allWorkoutActionsCompleted");
            // nameOfTheWorkout.text = predefinedWorkout.Workout[0].actions[actionCounter].actionName;

            if (!isBreakIdleAnimationApplied && isbreakEligible && isBreakCoroutineStarted)
            {
                Debug.LogError("In isBreakIdleAnimationApplied");
                //characterManager.ApplyIdleAnimationForCanvas();


                isBreakIdleAnimationApplied = true;
                nameOfTheWorkout.text = "Break";
                
                isBreakOn = true;
                Debug.LogError("IsbreakOn" + isBreakOn);
                gamePlayPanel.SetActive(true);
                workoutSectionTextOnGamePlay.gameObject.SetActive(true);
                workoutSectionTextOnGamePlay.text = "Core";
                //caloriesIcon.SetActive(false);


                //Camera change is required or not
                if (characterManager.CharacterAnimationHandlerOBJ.CheckCharacterAngleToBeSet(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId))
                {
                    characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        rotationValue90Deg, characterManager.characterModel.transform.rotation.z);
                }
                else
                {
                    characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                }


                    caloriesCountOnCoreAction.SetActive(false);




            }
            else if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId != "BB" && (!isbreakEligible && !isBreakCoroutineStarted))
            { 
                Debug.LogError("IsbreakOn" + isBreakOn);
                isBreakOn = false;
                Debug.LogError("IsbreakOn" + isBreakOn);
                isBreakIdleAnimationApplied = false;
                //CounterForNotDevelopedActions();
                //if (!checkPointManager.endOfCore)
                //{
                //    checkPointManager.TriggerAtEndOfTheCore();
                //}
                //else if(!checkPointManager.endOfCool)
                //{
                //    checkPointManager.TriggerAtEndOFCoolDown();
                //}
                //if(!checkPointManager.endOfWarm)
                //{
                //    checkPointManager.TriggerAtWarmUpEnd();
                //}



                characterManager.ApplyProperOverride(
                characterManager.CharacterAnimationHandlerOBJ.GetAvatarFromActionId(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId),
                characterManager.CharacterAnimationHandlerOBJ.GetAnimationOverrideFromActionId(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId),
                characterManager.CharacterAnimationHandlerOBJ.GetAnimationSpeed(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId)
                
                ) ;
                workoutSectionTextOnGamePlay.gameObject.SetActive(true);
                //caloriesIcon.SetActive(true);
                //Action Name to be shown At Progress bar
                if (isbreakEligible)
                {
                    nameOfTheWorkout.text = "Break";
                    
                }    
                else
                {
                    nameOfTheWorkout.text = characterManager.CharacterAnimationHandlerOBJ.GetActionName(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId);
                    
                }
                    
                //If pause not called
                if (!FindObjectOfType<PauseWorkoutManager>().pauseCalled)
                {
                    matInputSystem.SetProperClusterID(characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId));
                }

                //Change Character angle
                if (characterManager.CharacterAnimationHandlerOBJ.CheckCharacterAngleToBeSet(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId))
                {
                    characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        rotationValue90Deg, characterManager.characterModel.transform.rotation.z);
                }
                else
                {
                    characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                }

                //caloriesDisplayOnNonDevelopedActions();
                //Calories Update for core actions
                if (characterManager.CharacterAnimationHandlerOBJ.IsThisCoreAction(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId))
                {
                    Debug.LogError(" In isCoreActionInProgress");
                    isCoreActionInProgress = true;
                    caloriesCountOnCoreAction.SetActive(true);
                    
                    Debug.LogError("Calories Burned : " + PlayerSession.Instance.GetCaloriesBurned());
                    calsPerSession = PlayerSession.Instance.GetCaloriesBurned();

                    Debug.LogError("calsPerSession : " + calsPerSession);
                    Debug.LogError("totalWorkoutCalories : " + totalWorkoutCalories);
                    displayCaloriesTillNow = totalWorkoutCalories + calsPerSession;
                    
                    newCaloriesValue = displayCaloriesTillNow;

                    Debug.Log("developedActionCal: " + totalWorkoutCalories);
                    //caloriesCountText.text = totalWorkoutCalories.ToString("0.0");
                    caloriesCountText.text = displayCaloriesTillNow.ToString("0.00");

                }
                else
                {
                    Debug.LogError(" Not In isCoreActionInProgress");
                    isCoreActionInProgress = false;
                    

                }

                if (uIManager.inCoreWorkout)
                {
                    if (actionCounter >= predefinedWorkout.selectedActionsForWorkouts.Count - 2)
                        isCoreActionInProgress = false;
                }

                //Prevoius code
                //1totalWorkoutCalories = PlayerSession.Instance.GetCaloriesBurned();
                //2Debug.Log("developedActionCal: " + totalWorkoutCalories);
                //3caloriesCountText.text = totalWorkoutCalories.ToString("0.0");
                //caloriesCountText.text = PlayerSession.Instance.GetCaloriesBurned().ToString("0.0");
                workoutSectionTextOnGamePlay.text = predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType.ToString();

                
                
            }
        }
        else
        {
            Debug.LogError("Workout complete flow started!");
            if (!isSessionCompleted)
            {
                isSessionCompleted = true;
                //nameOfTheWorkout.text = "Workout Complete";
                characterManager.HideMainCharacter();
                TurnOnOffPreviewPanel(false);
                
                DoCalculationForStoringData(true);
                StartCoroutine(EnableDisableResultScreen(true));
                FindObjectOfType<PauseWorkoutManager>().StoreDataAfterWorkout();
                matInputSystem.SetProperClusterID(0);
                Debug.LogError("Complete Complete" + playerProgress.noOfWorkoutComplete);
            }
        }

        if (timeInSec <= (currentActionWorkoutDuration + 7))
        {
            //blink progress bar
            if(isBreakActivated || isbreakEligible)
                BlinkProgressBar(true);
            else
                BlinkProgressBar(true);
            // Last 7 sec Calories Display in any sec
            if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Cooldown ||
                predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Warmup || predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Stretch
                )
            {
                
                
                if(predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Cooldown)
                {

                    caloriesCountText.text = displayCaloriesTillNow.ToString("0.00");
                  
                }
                else
                {
                    caloriesCountText.text = totalWorkoutCalories.ToString("0.00");
                }
            }

        }
        else
            BlinkProgressBar(false);


        if (timeInSec <= (currentActionWorkoutDuration + 3f))//
        {

            try
            {
                FindObjectOfType<CharacterAnimationEventHandler>().SwitchToActionEnd();
                
            }
            catch (Exception e) { }
        }

        if (timeInSec <= (currentActionWorkoutDuration + 3))
        {
            try
            {
                FindObjectOfType<CharacterAnimationEventHandler>().ResetBoolForMainAction();
            }
            catch (Exception e) { }
        }

       

        if (timeInSec <= currentActionWorkoutDuration && !isBreakFunctionCalled)
        {

            
            isActionWorkoutFetched = false;
                currentWorkOutAction.isCompleted = true;
                predefinedWorkout.selectedActionsForWorkouts[actionCounter].isActionCompleted = currentWorkOutAction.isCompleted;
                isFlowInCurrentWorkout = false;

            try
            {
                FindObjectOfType<CharacterAnimationEventHandler>().GoToIdleStart();
            }
            catch (Exception e) { }
            if (!isCalledWaitBetweenAction)
                StartCoroutine(WaitBetweenWorkoutAction());

            //if (!startWaitBetweenWorkout)
            //{
            //    Debug.Log("Action workout completed, fetch new action workout");

            //    isActionWorkoutFetched = false;
            //    currentWorkOutAction.isCompleted = true;
            //    predefinedWorkout.Workout[0].actions[actionCounter].isCompleted = currentWorkOutAction.isCompleted;
            //    startWaitBetweenWorkout = true;
            //}
            //if(startWaitBetweenWorkout)
            //{
            //    if (totalTransitionTime > 0f)
            //    {
            //        Debug.Log("Transition TImer started");
            //        StartTransitionTimer();
            //    }
            //    else
            //    {
            //        Debug.Log("Transition Time is Over, resetting the transition time and fetching other action");
            //        FetchNewActionWorkOutFlow();
            //        startWaitBetweenWorkout = false;
            //    }
            //}
            //Time.timeScale = 1f;
            //FetchWorkOutAction();
            //isActionWorkoutFetched = true;

        }
    }

    public void DoCalculationForStoringData(bool isWorkoutCompleted = false)
    {
        if (isWorkoutCompleted)
        {
            playerProgress.noOfWorkoutComplete++;
            FindObjectOfType<PauseWorkoutManager>().isWorkoutCompleted = isWorkoutCompleted;
        }
        playerProgress.totalCalories += displayCaloriesTillNow;
        playerProgress.nonPerformingCount += nonPerformingCounter;
        playerProgress.totalDurationOfGame += PlayerSession.Instance.GetGameplayDuration;
        playerProgress.totalFitnessPoints += PlayerSession.Instance.GetFitnessPoints() + OverallFitnessPointsOfWorkout;
        
    }

    private void BlinkProgressBar(bool enableBlinking)
    {

        if (isbreakEligible)
        {
            progressBarBlinkingAnimator.gameObject.GetComponent<Image>().color = Color.white;
            progressBarBlinkingAnimator.enabled = false;//Blinking Paused For Breaks
        }
        else
        {
            if (enableBlinking)
            {
                progressBarBlinkingAnimator.enabled = true;
                if(!caloriesCountOnCoreAction.activeInHierarchy)
                    caloriesCountOnCoreAction.SetActive(true);
                caloriesCountOnCoreAction.GetComponent<Animator>().enabled = true;
                if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Cooldown ||
                predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Warmup
                )
                {


                    if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Cooldown)
                    {

                        caloriesCountText.text = displayCaloriesTillNow.ToString("0.00");

                    }
                    else
                    {
                        caloriesCountText.text = totalWorkoutCalories.ToString("0.00");
                    }
                }
            }
            else
            {
                progressBarBlinkingAnimator.gameObject.GetComponent<Image>().color = Color.white;
                progressBarBlinkingAnimator.enabled = false;
                if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Cooldown ||
                predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Warmup || predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Stretch
                )
                {
                    caloriesCountOnCoreAction.SetActive(false);
                }
            }
        }
    }
    

    //This is used for transiotion between 2 workouts
    public IEnumerator WaitBetweenWorkoutAction()
    {
        Debug.LogError("WaitBetweenCalled");
        isCalledWaitBetweenAction = true;
        float i = 0;
        if (!lastSecondWorkoutCheck)
        {
            // nextActionName.gameObject.transform.localScale = new Vector3(5f, 5f, 5f);

            //TurnOnOffPreviewPanel(true);
            //if (predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId == "BB")
            //    nextActionName.text = "Break";
            //else
            //    nextActionName.text = characterAnimationHandler.GetActionName(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);
            // upper part instead of  below one
            if(!isBreakActivated)
            {
                nameOfTheWorkout.gameObject.transform.DOScale(new Vector3(3f, 2f, 2f), 0f);
            }

            
            if (isBreakOn && isCalledWaitBetweenAction)
            {
                
                TurnOnOffPreviewPanel(true);
                characterManager.ApplyIdleAnimationForCanvas();
                gamePlayPanel.SetActive(true);
                
                
                nextActionName.text = "Break";
               // nextActionName.gameObject.transform.DOScale(1f, 1.5f).DOTimeScale(1f, 0f);
                //characterManager.ApplyIdleAnimation();
                
                //caloriesCountText.gameObject.SetActive(false);
                if (characterManager.CharacterAnimationHandlerOBJ.CheckCharacterAngleToBeSet(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId))
                {
                    characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        rotationValue90Deg, characterManager.characterModel.transform.rotation.z);
                }
                else if ((isbreakEligible && isBreakCoroutineStarted))
                {
                    characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                }
                else
                {
                    characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                }
                
                yield return new WaitForSeconds(5f);
                isCalledWaitBetweenAction = false;

            }
            else
            {
                Debug.LogError("Display Preview");
                gamePlayPanel.SetActive(false);
                //TurnOnOffPreviewPanel(true);
                //soundManager.SelectDialgoue(DialoguesClassification.DialogueType.nextActionDialogue);
                //yield return new WaitForSeconds(1f);
                //if (actionCounter != -1)
                //{
                //    soundManager.PlayActionNameAudio(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);

                //}


                //gamePlayPanel.SetActive(true);
                //nextActionIntenity.gameObject.SetActive(false);
                if (isbreakEligible && isCalledWaitBetweenAction)
                {
                    nextActionName.text = "Break";
                    // nextActionIntenity.gameObject.SetActive(false);
                    //nextActionIntensityText.gameObject.SetActive(false);
                    // durationTextOnPreviewPanel.gameObject.SetActive(false);
                    // nextActionName.gameObject.transform.DOScale(1f, 1.5f).DOTimeScale(1f, 0f);
                    //characterManager.ApplyIdleAnimation();
                    characterManager.ApplyIdleAnimationForCanvas();
                    if (characterManager.CharacterAnimationHandlerOBJ.CheckCharacterAngleToBeSet(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId))
                    {
                        characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        rotationValue90Deg, characterManager.characterModel.transform.rotation.z);
                    }
                    else if ((isbreakEligible && isBreakCoroutineStarted))
                    {
                        characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                    }
                    else
                    {
                        characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                    }
                    gamePlayPanel.SetActive(false);
                    TurnOnOffPreviewPanel(true);
                    yield return new WaitForSeconds(5f);
                    gamePlayPanel.SetActive(true);
                    isCalledWaitBetweenAction = false;
                }
                else
                {
                    
                    gamePlayPanel.SetActive(false);
                    //soundManager.SelectDialgoue(DialoguesClassification.DialogueType.nextActionDialogue);
                    //yield return new WaitForSeconds(1.5f);
                    // nextActionIntenity.gameObject.SetActive(true);
                    // nextActionIntensityText.gameObject.SetActive(true);
                    // durationTextOnPreviewPanel.gameObject.SetActive(true)

                    

                    characterManager.ApplyProperOverride(
                characterManager.CharacterAnimationHandlerOBJ.GetAvatarFromActionId(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId),
                characterManager.CharacterAnimationHandlerOBJ.GetAnimationOverrideFromActionId(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId),
                characterManager.CharacterAnimationHandlerOBJ.GetAnimationSpeed(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId)

                );

                    characterManager.ApplyProperOverrideCanvas(
                characterManager.CharacterAnimationHandlerCanvasOBJ.GetAnimationOverrideFromActionId(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId),
                characterManager.CharacterAnimationHandlerCanvasOBJ.GetAnimationSpeed(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId)

                );

                    nextActionName.text = characterAnimationHandler.GetActionName(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);

                    //nextActionIntenity.text = characterAnimationHandler.GetIntensityOfParticularAction(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId).ToString();
                    //nextActionName.gameObject.transform.DOScale(1f, 1.5f).DOTimeScale(1f, 0f);
                    if (characterManager.CharacterAnimationHandlerOBJ.CheckCharacterAngleToBeSet(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId))
                    {
                        characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                    }                                                                                        
                    else
                    {
                        characterManager.characterModel.transform.eulerAngles
                        = new Vector3(characterManager.characterModel.transform.rotation.x,
                        -90f, characterManager.characterModel.transform.rotation.z);
                    }

                    TurnOnOffPreviewPanel(true);
                    soundManager.SelectDialgoue(DialoguesClassification.DialogueType.nextActionDialogue);
                    Debug.LogError("Before PlayActionNameAudio");
                    yield return new WaitForSeconds(1.5f);
                    try {
                        
                        if (actionCounter != -1)
                        {
                            Debug.LogError("Trying PlayActionNameAudio");
                            soundManager.PlayActionNameAudio(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);

                        }
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("Wait between workout : " + e.StackTrace + "\n" + e.Message);
                    }
                    

                    yield return new WaitForSeconds(5f);
                    
                    isCalledWaitBetweenAction = false;
                    isNextSoundPlayed = false;

                }
                    
            }
                

            
            
        }
        Debug.LogError("Completed with preview panel, proceeding to fetch work out action");
        
        TurnOnOffPreviewPanel(false);
        try { gamePlayPanel.SetActive(true); } catch (Exception e) { }
        
        FetchWorkOutAction();
        isActionWorkoutFetched = true;
        nameOfTheWorkout.gameObject.transform.DOScale(1f, 1f);
        yield return new WaitForSeconds(2f);

        try
        {
            FindObjectOfType<CharacterAnimationEventHandler>().ResetBoolForMainAction();
            FindObjectOfType<CharacterAnimationEventHandler>().SwitchToMainActionWithoutEvent();
        }
        catch (Exception e) { }

        if (!replayTheIdleAnimation)
        {
            
            replayTheIdleAnimation = true;
        }
        isGoBackToIdleCalled = false;
        replayTheIdleAnimation = false;
        
    }

    public void StartTransitionTimer()
    {
    
        totalTransitionTime -= Time.deltaTime;
        Debug.Log("transition time : " + totalTransitionTime.ToString("0"));
        TurnOnOffPreviewPanel(true);
        if (!lastSecondWorkoutCheck)
        {

            if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId == "BB")
                nextActionName.text = "Break";
            else////590
                nextActionName.text = characterAnimationHandler.GetActionName(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);
        }
        
    }

    private void FetchNewActionWorkOutFlow()
    {


        TurnOnOffPreviewPanel(false);
        FetchWorkOutAction();
        isActionWorkoutFetched = true;
        totalTransitionTime = 5f;
    }

    public void DisablePauseButton()
    {
        if (allWorkoutActionsCompleted == true)
        {
            pauseButton.SetActive(false);
        }
    }


    public IEnumerator EnableDisableResultScreen(bool isEnabled)
    { //TODO: Trigger Sound with animation 
        TurnOnOffPreviewPanel(false);
        resultPanel.SetActive(isEnabled);
        soundManager.ResultScreenMusic();
        if (isEnabled)
        {
            yield return new WaitForSeconds(1.25f);
            soundManager.SelectDialgoue(DialoguesClassification.DialogueType.resultScreenDialogues);
        }
        //pauseWorkoutManager.EndWorkout();
        // previewPanel.SetActive(false);
        uIManager.DisplayFitnessInfo();
        if(isEnabled == true)
        {
           // matInputSystem.UpdateButtonList(feedbackPanelButtons, 1);
        }
    }

    public void GetUserFeedback(string userFeedback)
    {
        getUserFeedback = userFeedback;
        Debug.Log("FeedBack" + getUserFeedback);

            playerProgress.PreviousFeedback = getUserFeedback;
        
        WhichButtonIsSelected(userFeedback);
        easyBtn.interactable = false;
        medBtn.interactable = false;
        hardBtn.interactable = false;
        //TODO set button default selection after user feedback
        uIManager.SetDefaultSelectedButtonForUser();
        BackendDataHandler.instance.gotFeedback = userFeedback;
        try
        {
            //playerProgress.nonPerformingCount += nonPerformingCounter;
            //FindObjectOfType<PauseWorkoutManager>().StoreDataAfterWorkout();
            BackendDataHandler.instance.StoreWorkoutHistoryFeedBack(currentYipliConfig.userId, PlayerSession.Instance.GetCurrentPlayerId());

        }
        catch (Exception e)
        {

        }
    }

    private void WhichButtonIsSelected(string buttonName)
    {
        switch (buttonName)
        {
            case "veryEasy":
                FlipButtonObjectUsingAnimation(veryEasyBtn);
                break;
            case "justRight":
                FlipButtonObjectUsingAnimation(justRightBtn);
                break;
            case "tooHard":
                FlipButtonObjectUsingAnimation(tooHardBtn);
                break;
        }
    }

    private void FlipButtonObjectUsingAnimation(GameObject buttonObject)
    {
        buttonObject.transform.DORotate(new Vector3(0f, 0f, 0f), 1f);
        Debug.Log("msg");
        StartCoroutine(ShowThankYouMessage());
    }

    private IEnumerator ShowThankYouMessage()
    {
        //yield return new WaitForSeconds(1.5f);
        feedbackButtonPanel.SetActive(false);
        feedbackThankYouPanel.SetActive(true);
        feedbackThankYouPanel.transform.DORotate(new Vector3(0f, 0f, 0f), 1f);
        yield return new WaitForSeconds(2.5f);
        Debug.Log("gotomain");
        //StartCoroutine(GoBackToHomeAfterFeedback());
        FindObjectOfType<PauseWorkoutManager>().GoToHomeMenu();
    }

    public void ShowFeedBackPanelAgain()
    {
        Debug.LogError("Workout Start Time : feedback panel shown");
        feedbackButtonPanel.SetActive(true);
        feedbackThankYouPanel.SetActive(false);
    }

    //Setting timescale, storing time and intensity values and starting with workout gameplay panel
    public void OnWorkoutStart()
    {
        TurnOnOffPreviewPanel(false);
        Debug.LogError("Workout started");
        //TODO: Time Scale should be on EndWorkout()
        Time.timeScale = 1f;
        
        //Setting selected values in scriptable for future use
        playerProgress.privousTimeSelectedByUser = timeSelectedByUser;
        playerProgress.privousIntensitySelectedByUser = intensitySelectedByUser;
        //If workout is not generated, return 
        if(predefinedWorkout.selectedActionsForWorkouts.Count <= 0)
        {
            Debug.Log("Workout List is empty");
            return;
        }
         //If workout present start gameplay panel
        gamePlayPanel.SetActive(true);
       
        FetchWorkOutAction();
        DataTrackingCheck.trackingHasDone = false;
    }

    public void DisableSugessionPanel()
    {
        sugessionPanel.SetActive(false);
    }

    public void ChangeTimeButtonSpriteOnSelection(string timeInput)
    {
        switch(timeInput)
        {
            case "10":
                //GameObject ChildGameObject1 = ParentGameObject. transform. GetChild (0). gameObject;
                buttonTime10.transform.GetChild(1).gameObject.SetActive(true);
                buttonTime15.transform.GetChild(1).gameObject.SetActive(false);
                buttonTime20.transform.GetChild(1).gameObject.SetActive(false);
               // buttonTime10.GetComponent<Image>().sprite = buttonSelected;
               // buttonTime15.GetComponent<Image>().sprite = timeButtonGrey;
               // buttonTime20.GetComponent<Image>().sprite = timeButtonGrey;
                Debug.Log("Button 10");
                break;

            case "15":
                buttonTime10.transform.GetChild(1).gameObject.SetActive(false);
                buttonTime15.transform.GetChild(1).gameObject.SetActive(true);
                buttonTime20.transform.GetChild(1).gameObject.SetActive(false);
                //buttonTime10.GetComponent<Image>().sprite = timeButtonGrey;
              //  buttonTime15.GetComponent<Image>().sprite = buttonSelected;
               // buttonTime20.GetComponent<Image>().sprite = timeButtonGrey;
                Debug.Log("Button 15");
                break;

            case "20":
                buttonTime10.transform.GetChild(1).gameObject.SetActive(false);
                buttonTime15.transform.GetChild(1).gameObject.SetActive(false);
                buttonTime20.transform.GetChild(1).gameObject.SetActive(true);
               // buttonTime10.GetComponent<Image>().sprite = timeButtonGrey;
               // buttonTime15.GetComponent<Image>().sprite = timeButtonGrey;
               // buttonTime20.GetComponent<Image>().sprite = buttonSelected;
                Debug.Log("Button 20");
                break;
        }
        
    }

    public void ChangeIntensityButtonSpriteOnSelection(string intensityInput)
    {
        switch(intensityInput)
        {
            case "low":
                //intensityLow.transform.GetChild(1).gameObject.SetActive(true);
                //intensityMedium.transform.GetChild(1).gameObject.SetActive(false);
                //intensityHigh.transform.GetChild(1).gameObject.SetActive(false);
               // intensityLow.GetComponent<Image>().sprite = buttonSelected;
               // intensityMedium.GetComponent<Image>().sprite = intensityButtonGrey;
               // intensityHigh.GetComponent<Image>().sprite = intensityButtonGrey;
                Debug.Log("Button low");
                break;

            case "medium":
                //intensityLow.transform.GetChild(1).gameObject.SetActive(false);
                //intensityMedium.transform.GetChild(1).gameObject.SetActive(true);
                //intensityHigh.transform.GetChild(1).gameObject.SetActive(false);
                //intensityLow.GetComponent<Image>().sprite = intensityButtonGrey;
                //intensityMedium.GetComponent<Image>().sprite = buttonSelected;
                //intensityHigh.GetComponent<Image>().sprite = intensityButtonGrey;
                Debug.Log("Button med");
                break;

            case "high":
                //intensityLow.transform.GetChild(1).gameObject.SetActive(false);
                //intensityMedium.transform.GetChild(1).gameObject.SetActive(false);
                //intensityHigh.transform.GetChild(1).gameObject.SetActive(true);
               // intensityLow.GetComponent<Image>().sprite = intensityButtonGrey;
               // intensityMedium.GetComponent<Image>().sprite = intensityButtonGrey;
               // intensityHigh.GetComponent<Image>().sprite = buttonSelected;
                Debug.Log("Button high");
                break;

        }
    }

    public void ManageCurrentActionTimer()
    {
        if(perActionTimeCounter >= currentActionTime)
        {
            perActionTimeCounter = 0f;
        }
        perActionTimeCounter += Time.deltaTime;
        float fillAmount = ((perActionTimeCounter / currentActionTime));
        progressBarForeGround.fillAmount = Mathf.Clamp(fillAmount, 0f, 1f); 
        
        Debug.Log("progressBarForeGround fillAmount" + progressBarForeGround.fillAmount);

    }

    //As per action time Progress Bar is shown
    public IEnumerator ActionProgress()
    {
        perActionTimeCounter = 0f;
        while (perActionTimeCounter <= currentActionTime)
        {
            perActionTimeCounter += Time.deltaTime;
            float fillAmount = ((perActionTimeCounter / currentActionTime));
            progressBarForeGround.fillAmount = Mathf.Clamp(fillAmount, 0f, 1f);
            yield return null;
        }
    }

    private IEnumerator CalculateTimingForActionVoice()
    { //TODO: Instead return null -> break
        if (withoutDialogueBuild)
            yield return null;
        else
        {
            int i = 0;
            bool audioActionVoicePlayed = false;

            if (dontPlayNonPerformVoices)
                dontPlayNonPerformVoices = false;
            //TODO: Chnage all Static values to Const variables
            while (i < currentActionTime)
            {   //After First 4 secs check if ActionAudio or 
                if (i == 4 && !audioActionVoicePlayed && !actionNameAudioPlayed)
                {   
                    //Play action related voiceovers
                    //soundManager.PlayActionNameSound(currentWorkOutAction.actionId);
                    soundManager.PlayActionSound(currentWorkOutAction.actionId);
                    actionNameAudioPlayed = true;

                }
                if (currentVoiceOverClip != null)
                {
                    Debug.LogError("<Audio> I Count : " + i);
                    Debug.LogError("<Audio> Current Voice : " + Mathf.RoundToInt(currentVoiceOverClip.length + 4));
                    Debug.LogError("<Audio> Engaging Voice : " + audioEngagingVoicePlayed);
                    Debug.LogError("<Audio> Action Voice : " + audioEngagingVoicePlayed);
                    //Need 2 secs delay after every voiceover Hence 6 is added as first 4 secs + 2 secs
                    if ((i >= Mathf.RoundToInt(currentVoiceOverClip.length + 6)) && !audioEngagingVoicePlayed && !soundManager.isPlayingEngagingSound) // time, isEngagingAlreadyPlaying, if EngagingAudioIscompleted
                    {

                        soundManager.isPlayingEngagingSound = true;
                        //Check to play engaging or Non performing VoiceOvers
                        if (!playNonPerformingVoiceOvers)
                            soundManager.PlayEngagingSound(checkPointManager.workoutState);
                        else
                        {
                            //TODO: With nonPerforming Counter might add Action Name as well
                            nonPerformingCounter++;
                            soundManager.PlayNonPerformActionSound();
                        }
                        actionNameAudioPlayed = false;

                    }
                    else if (i >= (currentActionTime - 10))
                    { // Less than 10 secs are left from ActionTime Dont choose next action and let the previously selected action complete
                        soundManager.dialogueAudioSource.loop = false;
                        dontPlayNonPerformVoices = true;
                        // soundManager.dialogueAudioSource.Stop();
                        break;
                    }
                    //if (i == Mathf.RoundToInt(currentVoiceOverClip.length + 12) && audioEngagingVoicePlayed)
                    //{
                    //    soundManager.dialogueAudioSource.loop = false;
                    //    soundManager.dialogueAudioSource.Stop();
                    //    soundManager.PlayEngagingSound(checkPointManager.workoutState);
                    //}

                }
                else
                {
                    actionNameAudioPlayed = false;
                }
                i++;
                yield return new WaitForSecondsRealtime(1f);//TODO: Change to WaitForSec
            }
        }
    }

    public void OnStartRefreshSprites()
    {
        if (isAppStartedForFirstTime)
        {
            timeSelectedIs_10.GetComponent<Button>().onClick.Invoke();
            intensitySelectedIsLow.GetComponent<Button>().onClick.Invoke();
            isAppStartedForFirstTime = false;
        }
       // buttonTime15.GetComponent<Image>().sprite = timeButtonGrey;
       // buttonTime20.GetComponent<Image>().sprite = timeButtonGrey;
       // intensityMedium.GetComponent<Image>().sprite = intensityButtonGrey;
       // intensityHigh.GetComponent<Image>().sprite = intensityButtonGrey;
    }

    public void UpdateGameButtonList()
    {
       matInputSystem.UpdateButtonList(optionsButtons, 0, true);    
    }

    public void UpdateGameButtonListSprites()
    {
        //intensityLow.GetComponent<Image>().sprite = buttonSelected;
        //intensityMedium.GetComponent<Image>().sprite = buttonSelected;
        //intensityHigh.GetComponent<Image>().sprite = buttonSelected;
        //buttonTime10.GetComponent<Image>().sprite = buttonSelected;
        //buttonTime15.GetComponent<Image>().sprite = buttonSelected;
        //buttonTime20.GetComponent<Image>().sprite = buttonSelected;
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void TurnOnOffPreviewPanel(bool turnItOff)
    {

        if (isbreakEligible)
        {
            nextActionName.text = "Break";
        }
        Debug.LogError("Workout Start Time : preview panel disabled");
        if (isBreakActivated)
        {
            Debug.Log("isBreakActivated : " + isBreakActivated);
            previewPanel.SetActive(false);
            previewPanelMain.SetActive(false);
        }
        else
        {
            //PlayNextSoundAction();
            // soundManager.TransitionPanelBg();
            // soundManager.SelectDialgoue(DialoguesClassification.DialogueType.nextActionDialogue);

            try
            {
                Debug.Log("Action Counter : " + actionCounter);
                Debug.Log("predefinedWorkout.selectedActionsForWorkouts.Count : " + predefinedWorkout.selectedActionsForWorkouts.Count);
                if (characterManager.CharacterAnimationHandlerCanvasOBJ.CheckCharacterAngleToBeSetOnCanvas(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId))
                {
                    characterManager.characterModelCanvas.transform.localEulerAngles
                    = new Vector3(0f,
                    270f, 0f);
                }
                else
                {
                    characterManager.characterModelCanvas.transform.localEulerAngles
                    = canvasModelRotationValues;
                }
                Debug.Log("actionCounter : " + actionCounter);
               // if (actionCounter != -1)
               // { 
               //// soundManager.PlayActionNameAudio(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);
                
               // }
            }
            catch (Exception e) {
                Debug.LogError("Preview panel error : " + e.Message);
                Debug.LogError("Preview panel trace : " + e.StackTrace);
            }

            previewPanel.SetActive(turnItOff);
            previewPanelMain.SetActive(turnItOff);
            if (!turnItOff)
            {
                
                previewPanelMain.transform.DOLocalMove(new Vector3(-1950f, 0f, 11.8f), 0f);
            }
            else
            {
                //soundManager.TransitionPanelBg();
                
                previewPanelMain.transform.DOLocalMove(new Vector3(0f, 0f, 11.8f), 0.8f);
                
            }
                
        }

    }

    //public void PlayNextSoundAction()
    //{
        
    //    isNextSoundPlayed = true;
    //    PlayActionNameOnPreviewPanel();
    //    //if(actionCounter != -1)
    //    //soundManager.PlayActionNameAudio(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);
    //}

    //public void PlayActionNameOnPreviewPanel()
    //{
        
    //    if(actionCounter != -1 && isNextSoundPlayed)
    //    {
    //        //soundManager.PlayActionNameAudio(predefinedWorkout.selectedActionsForWorkouts[actionCounter + 1].actionId);
    //    }
        
    //    isNextSoundPlayed = true;
    //}



    

    public void ShowNoOfWorkoutAndCalories()
    {
        //totalCalories.text = playerProgress.totalCalories.ToString("0");
        //totalNumberOfWorkoutCompleted.text = playerProgress.noOfWorkoutComplete.ToString();
    }

    public float AllotBreakTimeAndBreakTriggerTime(string intensity)
    {
        switch(intensity)
        {
            case "low":
                breakTime = workoutTimeAndIntensity.IntensityWiseDuration[0].breakTime;
                return workoutTimeAndIntensity.IntensityWiseDuration[0].breakTriggerTime;
                

            case "medium":
                breakTime = workoutTimeAndIntensity.IntensityWiseDuration[1].breakTime;
                return workoutTimeAndIntensity.IntensityWiseDuration[1].breakTriggerTime;
                

            case "high":
                breakTime = workoutTimeAndIntensity.IntensityWiseDuration[2].breakTime;
                return workoutTimeAndIntensity.IntensityWiseDuration[2].breakTriggerTime;

            default:
                return 0f;
        }
    }
    

    public void breakCountDown()
    {
        if (FindObjectOfType<CurriculumGenerator>().isDemo)
            return;

        if (workoutConfig.triggerBreakTimer <= 0f)
        {
            if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Core)
            {
                isbreakEligible = true;
                Debug.Log("BreakIsEligible");
                return;
            }
            else
            { 
                workoutConfig.triggerBreakTimer = AllotBreakTimeAndBreakTriggerTime(intensitySelectedByUser);
                Debug.Log("BreakIsNotEligible");
                return;
            }
        }
            
        
        workoutConfig.triggerBreakTimer -= Time.unscaledDeltaTime;
        
    }

    public IEnumerator BreakIsActive()
    {
        //checks to see if brake is started
        //Fetch two timers 1)Trigger Timer 2)Brake Timer According to selected workout intensity
        int i = 0;
        progressBarForeGround.fillAmount = 0;
       
        isBreakCoroutineStarted = true;
        isBreakActivated = true;
        isBreakFunctionCalled = true;
        float x = workoutConfig.breakTime - 2;
        Debug.Log("Break is Completed");
        //int randomNumber = UnityEngine.Random.Range(0, idleOverrideControllersCanvas.Length);
        //characterAnimatorCanvas.runtimeAnimatorController = idleOverrideControllersCanvas[randomNumber];
        characterManager.ApplyIdleAnimation();
        //characterManager.ApplyIdleAnimationForCanvas();
        TurnOnOffPreviewPanel(true);
        //yield return new WaitForSecondsRealtime(3f);
        
        gamePlayPanel.SetActive(false);
        yield return new WaitForSecondsRealtime(2f);

        TurnOnOffPreviewPanel(false);
        if (!checkPointManager.inBreak)
        {
            checkPointManager.TriggerAtBreak();
        }
        BlinkProgressBar(false);
        gamePlayPanel.SetActive(true);
        matInputSystem.SetProperClusterID(0);
        while (i < x)
        {
            progressBarForeGround.fillAmount += 1 / x;
            i++;
            yield return new WaitForSecondsRealtime(1f);
        }
        
        Debug.LogError("Break logic completed");
        BlinkProgressBar(false);
        yield return new WaitForSecondsRealtime(1f);
        isbreakEligible = false;
        isBreakCoroutineStarted = false;
        //Assign brakeTriggerTimeAgain
        workoutConfig.triggerBreakTimer = AllotBreakTimeAndBreakTriggerTime(intensitySelectedByUser);
        isBreakActivated = false;
        isBreakFunctionCalled = false;
        stoppedPlayNonPerfromingAtBreak = false;
        soundManager.CoreBg();
        Debug.LogError("Break coroutine completed");
        //yield return new WaitForSeconds(workoutConfig.breakTime); 

    }

    //As new workout curriculum has range of time This function calculates the exact time of the workout 
    public void AssignTotalTimeToWorkout()
    {
        float newTotalTime = 0f;
        workoutConfig.totalTimeWithBreakIncluded = 0f;
        for (int i=0; i<predefinedWorkout.selectedActionsForWorkouts.Count; i++)
        {
            newTotalTime += predefinedWorkout.selectedActionsForWorkouts[i].currentWorkoutActionDuration;
        }
        Debug.Log("Current Time" + newTotalTime);
        workoutConfig.time = newTotalTime;
        workoutConfig.workoutTotalTime = newTotalTime;
        workoutConfig.totalTimeOfWorkoutIncludingBrakes = newTotalTime;
        workoutConfig.currentSelectedTime = newTotalTime;
        workoutConfig.totalTimeWithBreakIncluded = newTotalTime;
        totalWorkoutCalories = 0f;
        workoutConfig.nonPerformingTime = 0f;
        SetBreakTime(intensitySelectedByUser);
       
    }

    private void AddCaloriesForNonDetectableActionOnMat(string nameOfAction, float durationOfAction)
    {
        
       float numberToDecideCaloriesOfNotDevelopedActions = PerActionCaloriesToBeAddedForNotDevelopedActions(nameOfAction);
        float totalCalories = numberToDecideCaloriesOfNotDevelopedActions * durationOfAction;
        totalWorkoutCalories += totalCalories;
        displayCaloriesTillNow += totalCalories;
        Debug.Log("totalCaloriesOfWorkout" + totalWorkoutCalories);
        
    }

    private float PerActionCaloriesToBeAddedForNotDevelopedActions(string actionName)
    {  
        //TODO: Change to enum 
        switch (actionName)
        {
            case "WW":
                return 0.02f;
            case "AC":
                return 0.02f;
            case "SR":
                return 0.02f;
            case "BAR":
                return 0.02f;
            case "TS":
                return 0.02f;
            case "NS":
                return 0.02f;
            case "LC":
                return 0.02f;
            case "HFS":
                return 0.02f;
            case "STHS":
                return 0.02f;
            case "WS":
                return 0.02f;
            case "SS":
                return 0.02f;
            case "CAS":
                return 0.02f;
            case "QS":
                return 0.03f;
            case "LBS":
                return 0.02f;
            case "CCP":
                return 0.02f;
            case "CP":
                return 0.02f;
            case "SLHS":
                return 0.02f;//
            case "PK":
                return 0.08f;
            case "FF":
                return 0.05f;
            case "3LD":
                return 0.1f;
            case "MP":
                return 0.05f;
            case "AP":
                return 0.02f;
            default:
                return 0.02f;
               
        }

    }

    public void AddFitnessPointsForNotDetectedActions(string nameOfAction, float durationOfAction)
    {
            float fitnessPointNumberForNotDevelopedAction = PerActionFitnessPointsToBeAddedForNotDevelopedActions(nameOfAction);

            float totalFitnessPoints = fitnessPointNumberForNotDevelopedAction * durationOfAction;
            OverallFitnessPointsOfWorkout += totalFitnessPoints;
            

        
    }

    //Fitness Point Function
    private float PerActionFitnessPointsToBeAddedForNotDevelopedActions(string actionName)
    {
        //TODO: Change to enum 
        switch (actionName)
        {
            case "WW":
                return 0.2f;
            case "AC":
                return 0.2f;
            case "SR":
                return 0.2f;
            case "BAR":
                return 0.2f;
            case "TS":
                return 0.2f;
            case "NS":
                return 0.2f;
            case "LC":
                return 0.2f;
            case "HFS":
                return 0.2f;
            case "STHS":
                return 0.2f;
            case "WS":
                return 0.2f;
            case "SS":
                return 0.2f;
            case "CAS":
                return 0.2f;
            case "QS":
                return 0.3f;
            case "LBS":
                return 0.2f;
            case "CCP":
                return 0.2f;
            case "CP":
                return 0.2f;
            case "SLHS":
                return 0.2f;//
            case "PK":
                return 0.8f;
            case "FF":
                return 0.5f;
            case "3LD":
                return 1.0f;
            case "MP":
                return 0.5f;
            case "AP":
                return 0.2f;
            default:
                return 0.2f;

        }

    }


    // call when new action is fetched
    //public void CounterForNotDevelopedActions()
    //{
    //    int count = 0;
    //    currentNotDevelopedActionTimer = currentWorkOutAction.actionTime;
    //    count = (int)currentNotDevelopedActionTimer / 10;
    //    countOfTimesAddCaloriesForNotDevelopedActions = count;
    //}

    //public void timerForNotDevelopedAction()
    //{  
    //    float secCounter = 0f;
    //   if(predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Core && characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId) == 999)
    //    {
    //        for(int i=0;i< countOfTimesAddCaloriesForNotDevelopedActions;i++)
    //        {
    //            if(secCounter == 10f)
    //            {
    //                Debug.Log("Calories Added");
    //                secCounter = 0f;
    //            }
    //        }
    //    }


    //    secCounter += Time.unscaledDeltaTime;

    //}

   /* public void caloriesDisplayOnNonDevelopedActions()
    {
        if (characterManager.CharacterAnimationHandlerOBJ.IsThisCoreAction(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId))
        {
            isCoreActionInProgress = true;
            caloriesCountOnCoreAction.SetActive(true);
            totalWorkoutCalories += PlayerSession.Instance.GetCaloriesBurned();
            Debug.Log("developedActionCal: " + totalWorkoutCalories);
            caloriesCountText.text = totalWorkoutCalories.ToString("0.0");
            //if (characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId) == 999)
            //{
            //    AddCaloriesForNonDetectableActionOnMat();
            //}
        }
        else
        {
            isCoreActionInProgress = false;
            caloriesCountOnCoreAction.SetActive(false);
            if (characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkout.selectedActionsForWorkouts[actionCounter].actionId) == 999)
            {
                //call function that calculates calories for not developed actions
                

            }
        }
    }*/

    public void SetBreakTime(string intensity)
    {
        switch(intensity)
        {
            case LOW:
                workoutConfig.breakTime = workoutTimeAndIntensity.IntensityWiseDuration[0].breakTime;
                break;
            case MEDIUM:
                workoutConfig.breakTime = workoutTimeAndIntensity.IntensityWiseDuration[1].breakTime;
                break;
            case HIGH:
                workoutConfig.breakTime = workoutTimeAndIntensity.IntensityWiseDuration[2].breakTime;
                break;
        }

    }

    

    public void NonPerformingActionsVoiceOvers()
    {

        if (dontPlayNonPerformVoices)
        {
            //soundManager.dialogueAudioSource.Stop();
            return;
        }

        if (isBreakActivated && !stoppedPlayNonPerfromingAtBreak)
        {
            soundManager.dialogueAudioSource.Stop();
            stoppedPlayNonPerfromingAtBreak = true;
            return;
        }

        if (actionCounter > 0)
        {
            if (predefinedWorkout.selectedActionsForWorkouts[actionCounter].workoutType == SelectedActionsForWorkout.WorkoutType.Core)
            {



                if (newCaloriesValue <= previousCalories && !playNonPerformingVoiceOvers)
                {
                    workoutConfig.nonPerformingTime += Time.deltaTime;
                    if (workoutConfig.nonPerformingTime >= 20f && !playNonPerformingVoiceOvers)
                    {
                        playNonPerformingVoiceOvers = true;
                    }
                }
                else if (newCaloriesValue > previousCalories)
                {
                    previousCalories = newCaloriesValue;
                    workoutConfig.nonPerformingTime = 0f;
                    audioEngagingVoicePlayed = false;
                    playNonPerformingVoiceOvers = false;

                }
            }
        }

    }

    private IEnumerator PlayNonPerformingVoiceOversOrNot()
    {
        //workoutConfig.nonPerformingTime = 0f; 
        inNonPerformingVoices = true;
        soundManager.PlayNonPerformActionSound();
        audioEngagingVoicePlayed = false;
        yield return new WaitForSeconds(soundManager.audio.length);
        nonPerformingCounter = 0;
        playNonPerformingVoiceOvers = false;
        inNonPerformingVoices = false;
    }

   

    
}
