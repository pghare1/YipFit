using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PauseWorkoutManager : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject workoutPanel;
    public GameObject previewPanel;
    public GameObject optionPanel;
    public GameObject resultpanel;
    public GameObject confirmationPanel;
    public MatInputSystem matInputSystem = null;
    public playerProgress playerProgress;
    public UIManager uIManager;
    [SerializeField] private CharacterManager characterManager = null;
    [SerializeField] private PredefinedWorkoutManager predefinedWorkoutManager = null;
    [SerializeField] private RunTimePredefinedWorkout runTimePredefinedWorkout = null;
    [SerializeField] private List<Button> pausePanelButtons;
    [SerializeField] private List<Button> endWorkoutConfirmationPanelButtons;
    [SerializeField] private CheckPointManager checkPointManager = null;
    [SerializeField] private CurriculumGenerator curriculumGenerator = null;
    public SoundManager soundManager;
    //[SerializeField] AudioMixer master;

    // public PredefinedWorkout predefinedWorkout;
    public WorkoutConfig workoutConfig;
    public TextMeshProUGUI pauseTimerText;
    public bool pauseCalled = false;
   public float pauseTimerDefined = 301f;
    public float a;
    public int timedPausedCalled = 0;
    public bool isWorkoutCompleted = true;
    private bool storeUnfinishedWorkout = false;

    public float pauseTime;

    // Start is called before the first frame update
    void Start()
    {
        //predefinedWorkoutManager.OnStartRefreshSprites();
        //  ondefaultTimeAndIntensity();
        curriculumGenerator.ResetRuntimePredefinedWorkouts();
        pauseTime = pauseTimerDefined;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayPauseTimer();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseWorkout();
#endif
    }

    public void PauseWorkout()
    {

        PlayerSession.Instance.PauseSPSession();
        pauseCalled = true;
        pausePanel.SetActive(true);
        FindObjectOfType<SoundManager>().EnableDisableDialogueMixer(false);
        
        matInputSystem.isThisPausePanel = true;
        try
        {
            matInputSystem.SetProperClusterID(0);
        }
        catch (Exception e) { }
        matInputSystem.UpdateButtonList(pausePanelButtons, 0, false);
        confirmationPanel.SetActive(false);
        workoutPanel.SetActive(false);
        //characterManager.HideMainCharacter();
        soundManager.BreakBg();
        Time.timeScale = 0;
        timedPausedCalled++;
        Debug.LogError("Before OnPause Cluster id : " + characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkoutManager.predefinedWorkout.selectedActionsForWorkouts[predefinedWorkoutManager.actionCounter].actionId));
        
    }
    public void EndWorkout(bool isWorkoutInComplete = false)
    {
        if (isWorkoutInComplete)
        {
            storeUnfinishedWorkout = true;
            predefinedWorkoutManager.DoCalculationForStoringData();
            isWorkoutCompleted = false;
            StoreDataAfterWorkout();
        }
        
        FindObjectOfType<SoundManager>().EnableDisableMasterMixer(false);
        pauseTimerDefined = 300f;
        Debug.Log("ActionCounter Before End" + predefinedWorkoutManager.actionCounter);
        predefinedWorkoutManager.actionCounter = -2; //TODO: Why -2 is set for this variable.
       // predefinedWorkoutManager.allWorkoutActionsCompleted = false;
        Debug.Log("Action Counter" + predefinedWorkoutManager.actionCounter);
       // predefinedWorkoutManager.predefinedWorkout.ResetWorkout();
        StopCoroutine(predefinedWorkoutManager.WaitBetweenWorkoutAction());
        matInputSystem.isThisPausePanel = false;

        //confirmationPanel.SetActive(false);
        //StoreDataAfterWorkout();

        matInputSystem.SetProperClusterID(0);
        ShowTotalStatistics();
        if (!isWorkoutInComplete)
            GoToHomeMenu();

    }

    //Manages resume flow of resume workout and sets proper cluster id according to the workout
    public void ResumeWorkout()
    {
        //pauseTimerDefined = 45f;
        Time.timeScale = 1;
        //  predefinedWorkoutManager.actionCounter = -1;
        confirmationPanel.SetActive(false);
        //characterManager.DisplayMainCharacter();
        if(!previewPanel.activeInHierarchy)
            workoutPanel.SetActive(true);

        matInputSystem.isThisPausePanel = false;
        pausePanel.SetActive(false);
        Debug.LogError("OnResume Cluster id : " + characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkoutManager.predefinedWorkout.selectedActionsForWorkouts[predefinedWorkoutManager.actionCounter].actionId));
        matInputSystem.SetProperClusterID(characterManager.CharacterAnimationHandlerOBJ.GetActionClusterID(predefinedWorkoutManager.predefinedWorkout.selectedActionsForWorkouts[predefinedWorkoutManager.actionCounter].actionId));
        pauseCalled = false;
        FindObjectOfType<SoundManager>().EnableDisableDialogueMixer(true);
        
        predefinedWorkoutManager.isMatDisconnected = false;
        PlayerSession.Instance.ResumeSPSession();


    }
    
    public void StoreDataAfterWorkout()
    {
        //playerProgress.totalCalories += predefinedWorkoutManager.totalWorkoutCalories;
        //playerProgress.totalFitnessPoints += PlayerSession.Instance.GetFitnessPoints();
       
        try
        {
            PlayerSession.Instance.StoreSPSession(playerProgress.noOfWorkoutComplete);
        }
        catch (Exception e) { }

        if (predefinedWorkoutManager.actionCounter >= 1)
        {
            int unfinishedWorkoutDuration = 0;
            if (storeUnfinishedWorkout)
            {
                playerProgress.unfinishedWorkoutNumbers++;
                unfinishedWorkoutDuration = Mathf.Abs(Mathf.CeilToInt((workoutConfig.workoutTotalTime + 1) - workoutConfig.time));
            }
            BackendDataHandler.instance.StoreData(playerProgress.noOfWorkoutComplete, playerProgress.totalCalories,
                playerProgress.totalFitnessPoints, playerProgress.totalDurationOfGame, playerProgress.unfinishedWorkoutNumbers,
                playerProgress.PreviousFeedback, playerProgress.nonPerformingCount, timedPausedCalled, isWorkoutCompleted, unfinishedWorkoutDuration);
        }
        //PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMP);

       
    }

    //Go to main menu on finishing workout and on End Session 
    public void GoToHomeMenu()
    {
        
        pauseTimerDefined = 300f;
        predefinedWorkoutManager.actionCounter = -1;
        soundManager.MainMenuBg();
        //predefinedWorkoutManager.predefinedWorkout.ResetWorkout();
        FindObjectOfType<SoundManager>().EnableDisableMasterMixer(true);//
        predefinedWorkoutManager.predefinedWorkout = null;
        //predefinedWorkoutManager.workOutStarted = false;
        predefinedWorkoutManager.currentActionTime = 0f;
        predefinedWorkoutManager.allWorkoutActionsCompleted = false;
        predefinedWorkoutManager.isSessionCompleted = false;
        predefinedWorkoutManager.pauseButton.SetActive(true);
        predefinedWorkoutManager.currentWorkOutAction.actionId = "";
        predefinedWorkoutManager.currentWorkOutAction.actionTime = 0f;
        predefinedWorkoutManager.currentWorkOutAction.isCompleted = false;
        predefinedWorkoutManager.currentActionTime = 0f;
        predefinedWorkoutManager.isActionWorkoutFetched = false;
        predefinedWorkoutManager.workOutStarted = false;
        predefinedWorkoutManager.caloriesCountText.text = "0";
        
        uIManager.inStretchingWorkout = false;
        uIManager.inCoreWorkout = false;
        uIManager.inLightWorkout = false;
        //uIManager.calories.text = "0";
        //uIManager.fitnessPoints.text = "0";
        //uIManager.duration.text = "0";
        ShowTotalStatistics();
        predefinedWorkoutManager.ShowNoOfWorkoutAndCalories();
        //curriculumGenerator.ResetRuntimePredefinedWorkouts();

        //pausePanel.SetActive(false);
        //optionPanel.SetActive(true);
        //resultpanel.SetActive(false);
        // predefinedWorkoutManager.OnStartRefreshSprites();

        try
        {
            predefinedWorkoutManager.UpdateGameButtonList();
            matInputSystem.SetProperClusterID(0);
        }
        catch (Exception e) { }
        
        StartCoroutine(ReloadTheCurrentScene());
    }

    private IEnumerator ReloadTheCurrentScene()
    {
        //runTimePredefinedWorkout.ClearDataOfWorkout();
        yield return new WaitForSecondsRealtime(0.00001f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //pausePanel.SetActive(false);
        Time.timeScale = 1f;
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!asyncOperation.isDone)
        //{
        //    yield return null;
        //}
    }

    //Display Pause Timer on clicking the pause button, pause timer if fixed for the workout
    public void DisplayPauseTimer()
    {
        if (predefinedWorkoutManager.isMatDisconnected)
        {
            pauseTimerText.gameObject.SetActive(false);
            return;
        }
        else
            pauseTimerText.gameObject.SetActive(true);

        if (!pausePanel.activeInHierarchy)
            return;

        if (pauseTime <= 15f)
            checkPointManager.TriggerWhenBreakISAboutTOOVer();

        if(pauseTime <= 0f)
        {
            GoToHomeMenu();
        }

        float pauseTimer = pauseTime;
        pauseTimer -= Time.unscaledDeltaTime;

        float mins = Mathf.FloorToInt(pauseTimer / 60f);
        float secs = Mathf.FloorToInt(pauseTimer % 60f);

        if (mins < 1)
        {
            if (secs < 1)
                secs = 0;

            if (secs <= 9)
                pauseTimerText.text = "00" + " : " + "0" + secs.ToString("0");
            else
                pauseTimerText.text = "00" + " : " + secs.ToString("0");
        }
        else
        {

            pauseTimerText.text = mins.ToString("00") + " : " + secs.ToString("00");
        }

        pauseTime = pauseTimer;
      //pauseTimerText.text = pauseTimerDefined.ToString();
        
        


    }

    public void ShowEndWorkoutConfirmation()
    {
        confirmationPanel.SetActive(true);
        matInputSystem.isThisPausePanel = false;
        matInputSystem.UpdateButtonList(endWorkoutConfirmationPanelButtons, 1);
    }

    public void ResetTimeOnWorkoutComplete()
    {
        Debug.Log("Time Reset To 0");
        workoutConfig.time = 0f;
        //runTimePredefinedWorkout.ClearDataOfWorkout();
    }

    public void DisableEndWorkoutpanels()
    {
        confirmationPanel.SetActive(false);
    }

    public void SetFeedbackToBeDefalut()
    {
       
        playerProgress.PreviousFeedback = null;
        
    }
    public void ondefaultTimeAndIntensity()
    {
        if(playerProgress.PreviousFeedback == null)
        {
            uIManager.button10.GetComponent<Button>().onClick.Invoke();
            uIManager.lowButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    //public void DefaultValues()
    //{
    //    uIManager.button10.GetComponent<Button>().onClick.Invoke();
    //    uIManager.lowButton.GetComponent<Button>().onClick.Invoke();
    //}

    public void ShowTotalStatistics()
    {
        //playerProgress.totalCaloriesCount += PlayerSession.Instance.GetCaloriesBurned();
        //playerProgress.totalDurationOfGame += PlayerSession.Instance.GetGameplayDuration;
        //playerProgress.totalFitnessPoints += PlayerSession.Instance.GetFitnessPoints();

    }
}
