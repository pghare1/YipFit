using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using YipliFMDriverCommunication;
using UnityEngine.EventSystems;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject time10Highlighter, time15Highlighter, time20Highlighter, lowHighligher, mediumHighlighter, highHighlighter, stretchDefaultTimeButton_Highlighter, lightDefaultTimeButton_Highlighter, coreDefaultTime_HighLighter, coreDefaultIntensity_Highlighter;
    public GameObject[] timehiglighersForCore, intensityHiglighersForCore;
    public TextMeshProUGUI calories, fitnessPoints, duration;
    public float caloriesBurned, fps, totalDuration; 
    public playerProgress playerProgress;
    public WorkoutConfig workoutConfig;
    public PredefinedWorkoutManager predefinedWorkoutManager;
    [SerializeField] private RunTimePredefinedWorkout runTimePredefinedWorkout;
    public SoundManager soundManager = null;
    public YipliConfig currentYipliConfig;
    public const string defaultIntensityForStretching = "";
    public const string HIGH = "high";
    public const string MED = "medium";
    public const string LOW = "low";
    public const float TIME10 = 10f;
    public const float TIME15 = 1f;
    public const float TIME20 = 1f;
    public Button button10, button15, button20, lowButton, medButton, highButton, stretchDefaultTimeButton, lightDefaultTimeButton, coreDefaultTime, coreDefaultIntensity;
    public GameObject profilePanel,OfferingsPanel, stretchingTimeOptionsPanel, coreOptionsPanel, lightWorkoutOptionsPanel, optionsPanel, landingPage;
    public TextMeshProUGUI ageText, heightText, weightText, fpPointsText, calText, durationText, workoutCountText;
    public float timeOfFlip = 6f;
    public bool isOnCaloriesDisplayed = true;
    public bool isTimeOfFlipOver = false;
    public GameObject caloriesAndWorkoutHolder;
    public TextMeshProUGUI caloriesCount, workoutCount, headingText, avgIntensityOfWorkout, playerNameText, unfinishedWorkoutNumber;
    public int coreActionCount = 0;
    public float completeFitnessPoints = 0f;
    public Button backgroundMusicToggleButton;
    public Sprite bGMusicOn;
    public Sprite bGMusicOff;
    public bool isBgMusicOn = true, isDialogue = true;
    public GameObject settingsPanel;
    public GameObject handleMusic, handleDialogue;
    public GameObject buttonMusic, buttonDialogue;
    public GameObject quitConfirmationPanel;
    public GameObject Demopanel;
    bool isOpened = false, buttonExecuted = false;
    public List<Button> DemoPanelButtons;
    public MatInputSystem matInputSystem;
    public Button playButton;
    public Button keepGoingButton;
    public GameObject keepgoing_Quit, quitButton;
    public Sprite selected, notSelected;
    public TextMeshProUGUI appVersionText;
    public bool inStretchingWorkout = false;
    public bool inCoreWorkout = false;
    public bool inLightWorkout = false;

    public Button playStretching, playFull, playCore, playLight;

    GameObject time10Temp, time15Temp, time20Temp, intensityLowPrev, intensityMedPrev, intensityHighPrev;
     
    private async void Start()
    {
        //KeepgoinOnClick();

        AssignPrevRefForHighligther();

        //matInputSystem.UpdateButtonList(DemoPanelButtons, 0, true);
        handleMusic.transform.DOLocalMoveX(75f, 0.5f);
        buttonMusic.GetComponent<Image>().DOColor(new Color(0.43f, 0.42f, 0.83f), 0.5f);
        handleDialogue.transform.DOLocalMoveX(75f, 0.5f);
        buttonDialogue.GetComponent<Image>().DOColor(new Color(0.43f, 0.42f, 0.83f), 0.5f);

        if (!playerProgress.playerId.Equals(PlayerSession.Instance.GetCurrentPlayerId()))
        {
            playerProgress.playerId = PlayerSession.Instance.GetCurrentPlayerId();
            await BackendDataHandler.instance.GetData();
            

            //durationText.text = (playerProgress.totalDurationOfGame/60f).ToString("0");
            
        }
        if(playerProgress.totalFitnessPoints >= 1000f)
        {
            fpPointsText.text = (playerProgress.totalFitnessPoints / 1000f).ToString("0.0") + "K";
        }
        else
        {
            fpPointsText.text = playerProgress.totalFitnessPoints.ToString("0.0");
        }
        
        calText.text = playerProgress.totalCalories.ToString("0");
        workoutCountText.text = playerProgress.noOfWorkoutComplete.ToString();
        playerNameText.text = PlayerSession.Instance.GetCurrentPlayer();
        float min = playerProgress.totalDurationOfGame / 60f;
        float hr = playerProgress.totalDurationOfGame / 3600f;

        durationText.text = ConvertToTime(playerProgress.totalDurationOfGame);

        //if (hr < 1)
        //{
        //    if (min < 1)
        //        min = 0;

        //    if (min <= 60)
        //        durationText.text = "00" + " : " + min.ToString("0");
        //    else
        //        durationText.text = "00" + " : " + min.ToString("0");
        //}
        //else
        //{
        //    //if counter goes beyond 1 min, then show only seconds value.....
        //    durationText.text = hr.ToString("00") + " : " + hr.ToString("00");
        //}
        SetDefaultSelectedButtonForUser();
        ageText.text = currentYipliConfig.playerInfo.playerAge.ToString();//to start
        heightText.text = currentYipliConfig.playerInfo.playerHeight.ToString();
        weightText.text = currentYipliConfig.playerInfo.playerWeight.ToString();
        unfinishedWorkoutNumber.text = playerProgress.unfinishedWorkoutNumbers.ToString();
        //soundManager.EnableDisableDialogueMixer(false);
        FillCaloriesAndWorkoutPoints(Mathf.RoundToInt(playerProgress.totalCalories), (int)playerProgress.noOfWorkoutComplete);
        appVersionText.text = PlayerSession.Instance.GetDriverAndGameVersion();
        GetSettingsPlayerPref();
        //ButtonOnOff();
        //DialoguesOnOff();
    }

    public void AssignPrevRefForHighligther()
    {
        time10Temp = time10Highlighter;
        time15Temp = time15Highlighter;
        time20Temp = time20Highlighter;

        intensityLowPrev = lowHighligher;
        intensityMedPrev = mediumHighlighter;
        intensityHighPrev = highHighlighter;
    }

    public string ConvertToTime(float timeSeconds)

    {

        int mySeconds = System.Convert.ToInt32(timeSeconds);



        int myHours = mySeconds / 3600; //3600 Seconds in 1 hour

        mySeconds %= 3600;



        int myMinutes = mySeconds / 60; //60 Seconds in a minute

        mySeconds %= 60;





        string mySec = mySeconds.ToString(),

        myMin = myMinutes.ToString(),

        myHou = myHours.ToString();



        if (myHours < 10) { myHou = myHou.Insert(0, "0"); }

        if (myMinutes < 10) { myMin = myMin.Insert(0, "0"); }

        if (mySeconds < 10) { mySec = mySec.Insert(0, "0"); }





        return myHou + ":" + myMin;

    }

    public void FillCaloriesAndWorkoutPoints(int cals, int wokrouts)
    {
        caloriesCount.text = cals.ToString();
        workoutCount.text = wokrouts.ToString();
    }

    public void HighlighterEnableonButtonPress(string name)
    {

        switch (name)
        {
            case "10":
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                break;

            case "15":
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                break;

            case "20":
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                break;

            case "low":
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                break;

            case "medium":
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                break;

            case "high":
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);

                break;



        }

    }

    public void DisplayFitnessInfo()
    {
        float cals = predefinedWorkoutManager.displayCaloriesTillNow;
        if (FindObjectOfType<CurriculumGenerator>().is20Secs)
            calories.text = cals.ToString("0.0");
        else
            calories.text = cals.ToString("0.0");

        float fitnessPointsOfCoreSession = PlayerSession.Instance.GetFitnessPoints();
        completeFitnessPoints = fitnessPointsOfCoreSession + predefinedWorkoutManager.OverallFitnessPointsOfWorkout;
        fitnessPoints.text = completeFitnessPoints.ToString("0.0");
        float mins = Mathf.FloorToInt(workoutConfig.totalTimeWithBreakIncluded / 60f);//÷ 3,600
        float hour = Mathf.FloorToInt(workoutConfig.totalTimeWithBreakIncluded / 3600f);
        float secs = Mathf.FloorToInt(workoutConfig.totalTimeWithBreakIncluded % 60f);

       // totalDuration = predefinedWorkoutManager.timeSelectedByUser;
       // duration.text = totalDuration.ToString();
       duration.text = mins.ToString("00") + " : " + secs.ToString("00");
        ShowAverageIntensityLevelOfWorkout();
    }
    
    public void SetDefaultSelectedButtonForUser()
    {
        
        if (playerProgress.PreviousFeedback == "tooHard")
        {
            if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                
                Debug.Log("Button 15 Invoked");
                button15.GetComponent<Button>().onClick.Invoke();
                medButton.GetComponent<Button>().onClick.Invoke();
                Debug.Log("Medium button Invoked");
            }
            /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                button10.GetComponent<Button>().onClick.Invoke();
                medButton.GetComponent<Button>().onClick.Invoke();
            }*/

            else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                button10.GetComponent<Button>().onClick.Invoke();
                medButton.GetComponent<Button>().onClick.Invoke();
            }
            else if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button15.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
            }
            /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button10.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
            }*/

            else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button10.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
            }
            else if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button15.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
            }
            /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button10.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
            }*/

            else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button10.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
            }

        }
       else if (playerProgress.PreviousFeedback == "veryEasy")
        {
            if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button20.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }
            /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button20.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }*/

            else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button15.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }
            else if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button20.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }
            /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button15.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }*/

            else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button10.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }
            else if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                button15.GetComponent<Button>().onClick.Invoke();
                medButton.GetComponent<Button>().onClick.Invoke();
            }
            /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                button20.GetComponent<Button>().onClick.Invoke();
                medButton.GetComponent<Button>().onClick.Invoke();
            }*/

            else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                button20.GetComponent<Button>().onClick.Invoke();
                medButton.GetComponent<Button>().onClick.Invoke();
            }

        }
        else if (playerProgress.PreviousFeedback == "justRight")
        {
            if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button20.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }
            /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(true);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button15.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }
            }*/

            else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == HIGH)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(true);
                button10.GetComponent<Button>().onClick.Invoke();
                highButton.GetComponent<Button>().onClick.Invoke();
            }
        }
            else if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
                button20.GetComponent<Button>().onClick.Invoke();
                medButton.GetComponent<Button>().onClick.Invoke();
            }
        //
        /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == MED)
        {
            time10Highlighter.SetActive(false);
            time15Highlighter.SetActive(true);
            time20Highlighter.SetActive(false);
            lowHighligher.SetActive(false);
            mediumHighlighter.SetActive(true);
            highHighlighter.SetActive(false);
            button15.GetComponent<Button>().onClick.Invoke();
            highButton.GetComponent<Button>().onClick.Invoke();
        }
        }*/

        else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == MED)
            {
                time10Highlighter.SetActive(true);  
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(false);
                mediumHighlighter.SetActive(true);
                highHighlighter.SetActive(false);
               button10.GetComponent<Button>().onClick.Invoke();
               medButton.GetComponent<Button>().onClick.Invoke();
        }
            else if (playerProgress.privousTimeSelectedByUser == TIME20 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(false);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(true);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button20.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
        }
        /*else if (playerProgress.privousTimeSelectedByUser == TIME15 && playerProgress.privousIntensitySelectedByUser == LOW)
        {
            time10Highlighter.SetActive(false);
            time15Highlighter.SetActive(true);
            time20Highlighter.SetActive(false);
            lowHighligher.SetActive(true);
            mediumHighlighter.SetActive(false);
            highHighlighter.SetActive(false);
            button15.GetComponent<Button>().onClick.Invoke();
            lowButton.GetComponent<Button>().onClick.Invoke();
        }*/

        else if (playerProgress.privousTimeSelectedByUser == TIME10 && playerProgress.privousIntensitySelectedByUser == LOW)
            {
                time10Highlighter.SetActive(true);
                time15Highlighter.SetActive(false);
                time20Highlighter.SetActive(false);
                lowHighligher.SetActive(true);
                mediumHighlighter.SetActive(false);
                highHighlighter.SetActive(false);
                button10.GetComponent<Button>().onClick.Invoke();
                lowButton.GetComponent<Button>().onClick.Invoke();
        }
        else
        {
            time10Highlighter.SetActive(true);
            time15Highlighter.SetActive(false);
            time20Highlighter.SetActive(false);
            lowHighligher.SetActive(true);
            mediumHighlighter.SetActive(false);
            highHighlighter.SetActive(false);
            button10.GetComponent<Button>().onClick.Invoke();
            lowButton.GetComponent<Button>().onClick.Invoke();

        }
        

        }

    public void ShowProfilePanel()
    {
        profilePanel.SetActive(true);
    }

    public void DisableProfilePanel()
    {
        profilePanel.SetActive(false);
    }  
    
    public void FlipCaloriesAndWorkoutText()
    {
        StartCoroutine(ChangeCaloriesAndWorkoutText(isOnCaloriesDisplayed));
    }

    public IEnumerator ChangeCaloriesAndWorkoutText(bool isCaloriesDisplayed)
    {
        if (isCaloriesDisplayed)
        {
            workoutCount.gameObject.SetActive(false);
            caloriesCount.gameObject.SetActive(true);
            
            caloriesAndWorkoutHolder.gameObject.transform.DORotate(new Vector3(0f, 180f, 0f), 1f);
            headingText.text = "Calories (Kcal)";
            isOnCaloriesDisplayed = false;
        }
        else
        {
            workoutCount.gameObject.SetActive(true);
            caloriesCount.gameObject.SetActive(false);
            
            caloriesAndWorkoutHolder.gameObject.transform.DORotate(new Vector3(0f, 0f, 0f), 1f);
            headingText.text = "Total Workouts";
            isOnCaloriesDisplayed = true;
        }

        yield return new WaitForSeconds(2f);
        timeOfFlip = 6f;
        isTimeOfFlipOver = false;

    }

    private void Update()
    {
        timeOfFlip -= Time.deltaTime;
        if(timeOfFlip <= 0f && !isTimeOfFlipOver)
        {
            isTimeOfFlipOver = true;
            FlipCaloriesAndWorkoutText();
        }
        //if (!buttonExecuted)
        //    MatInputs();
    }


    
    public void ShowAverageIntensityLevelOfWorkout()
    {
        float totalWorkoutIntensity = 0f;
        float avgWorkoutIntensity = 0f;
        for (int i = 0; i < runTimePredefinedWorkout.selectedActionsForWorkouts.Count; i++)
        {
            if(runTimePredefinedWorkout.selectedActionsForWorkouts[i].workoutType == SelectedActionsForWorkout.WorkoutType.Core)
            {
                totalWorkoutIntensity += runTimePredefinedWorkout.selectedActionsForWorkouts[i].intensityNumber;
                Debug.Log("TotalIntensity:" + totalWorkoutIntensity);
                coreActionCount++;
            }
            avgWorkoutIntensity = totalWorkoutIntensity / runTimePredefinedWorkout.selectedActionsForWorkouts.Count;
            Debug.Log("AvgIntensity:" + avgWorkoutIntensity);
            avgIntensityOfWorkout.text = avgWorkoutIntensity.ToString("0.0");
            playerProgress.lastAvgWorkoutIntensity = avgWorkoutIntensity;
        }
    }


    public void ButtonOnOff()
    {
        if (workoutConfig.backgroundMusicSetting)
        {
            workoutConfig.backgroundMusicSetting = false;
            workoutConfig.SetBackgroundMusicSetting(workoutConfig.backgroundMusicSetting);
            BackgroundMusicButtonOnOff(isBgMusicOn);
            soundManager.EnableDisableBGMusicMixer(workoutConfig.backgroundMusicSetting);
            handleMusic.transform.DOLocalMoveX(-100f, 0.5f);
            buttonMusic.GetComponent<Image>().DOColor(Color.white, 0.5f);
        }
        else
        {
            workoutConfig.backgroundMusicSetting = true;
            workoutConfig.SetBackgroundMusicSetting(workoutConfig.backgroundMusicSetting);
            BackgroundMusicButtonOnOff(workoutConfig.backgroundMusicSetting);
            soundManager.EnableDisableBGMusicMixer(workoutConfig.backgroundMusicSetting);
            handleMusic.transform.DOLocalMoveX(75f, 0.5f);
            buttonMusic.GetComponent<Image>().DOColor(new Color(0.43f, 0.42f, 0.83f), 0.5f);
        }
           
    }

    public void DialoguesOnOff()
    {
        if (workoutConfig.dialoguesSettings)
        {
            workoutConfig.dialoguesSettings = false;
            workoutConfig.SetDialogueSetting(workoutConfig.dialoguesSettings);
            soundManager.EnableDisableDialogueMixer(workoutConfig.dialoguesSettings);
            handleDialogue.transform.DOLocalMoveX(-100f, 0.5f);
            buttonDialogue.GetComponent<Image>().DOColor(Color.white, 0.5f);
        }
        else
        {
            workoutConfig.dialoguesSettings = true;
            workoutConfig.SetDialogueSetting(workoutConfig.dialoguesSettings);
            soundManager.EnableDisableDialogueMixer(workoutConfig.dialoguesSettings);
            
            handleDialogue.transform.DOLocalMoveX(75f, 0.5f);
            buttonDialogue.GetComponent<Image>().DOColor(new Color(0.43f, 0.42f, 0.83f), 0.5f);
        }
    }

    private void GetSettingsPlayerPref()
    {
        workoutConfig.dialoguesSettings = workoutConfig.GetDialogueSetting();
        workoutConfig.backgroundMusicSetting = workoutConfig.GetBackgroundMusicSetting();

        if (!workoutConfig.backgroundMusicSetting)
        {
            workoutConfig.backgroundMusicSetting = false;
            workoutConfig.SetBackgroundMusicSetting(workoutConfig.backgroundMusicSetting);
            BackgroundMusicButtonOnOff(isBgMusicOn);
            soundManager.EnableDisableBGMusicMixer(workoutConfig.backgroundMusicSetting);
            handleMusic.transform.DOLocalMoveX(-100f, 0.5f);
            buttonMusic.GetComponent<Image>().DOColor(Color.white, 0.5f);
        }
        else
        {
            workoutConfig.backgroundMusicSetting = true;
            workoutConfig.SetBackgroundMusicSetting(workoutConfig.backgroundMusicSetting);
            BackgroundMusicButtonOnOff(workoutConfig.backgroundMusicSetting);
            soundManager.EnableDisableBGMusicMixer(workoutConfig.backgroundMusicSetting);
            handleMusic.transform.DOLocalMoveX(75f, 0.5f);
            buttonMusic.GetComponent<Image>().DOColor(new Color(0.43f, 0.42f, 0.83f), 0.5f);
        }

        if (!workoutConfig.dialoguesSettings)
        {
            workoutConfig.dialoguesSettings = false;
            workoutConfig.SetDialogueSetting(workoutConfig.dialoguesSettings);
            soundManager.EnableDisableDialogueMixer(workoutConfig.dialoguesSettings);
            handleDialogue.transform.DOLocalMoveX(-100f, 0.5f);
            buttonDialogue.GetComponent<Image>().DOColor(Color.white, 0.5f);
        }
        else
        {
            workoutConfig.dialoguesSettings = true;
            workoutConfig.SetDialogueSetting(workoutConfig.dialoguesSettings);
            soundManager.EnableDisableDialogueMixer(workoutConfig.dialoguesSettings);

            handleDialogue.transform.DOLocalMoveX(75f, 0.5f);
            buttonDialogue.GetComponent<Image>().DOColor(new Color(0.43f, 0.42f, 0.83f), 0.5f);
        }

    }

    public void BackgroundMusicButtonOnOff(bool isOn)
    {
        soundManager.EnableDisableBGMusicMixer(isOn);
    }

    public void DialoguesMixerOnOff(bool isOn)
    {
        
    }

    public void EnableSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void DisableSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void EnableQuitConfirmationPanel()
    {
        
        quitConfirmationPanel.SetActive(true);
        
    }
    public void DisableQuitConfirmationPanel()
    {
        quitConfirmationPanel.SetActive(false);
    }


    public void EnableDemoPanel()
    {
        Demopanel.SetActive(true);
        isOpened = true;
        //matInputSystem.UpdateButtonList(optionsButtons, 0, true);
        //matInputSystem.UpdateButtonList(DemoPanelButtons, 0, true);
    }

    public void DisableDemoPanel()
    {
        Demopanel.SetActive(false);
    }

    public void DemoButtonGameplay()
    {
        predefinedWorkoutManager.GetUserTime(2f);
        predefinedWorkoutManager.ChangeTimeButtonSpriteOnSelection("10");
        HighlighterEnableonButtonPress("10");
        predefinedWorkoutManager.GetUserIntensity("low");
        predefinedWorkoutManager.ChangeIntensityButtonSpriteOnSelection("low");
        HighlighterEnableonButtonPress("low");
        playButton.onClick.Invoke();
    }

    public void KeepgoinOnClick()
    {
        //quitButton.GetComponent<Image>().sprite = notSelected;
        //keepGoingButton.GetComponent<Image>().sprite = selected;
    }

    public void QuitOnClick()
    {
        quitButton.GetComponent<Image>().sprite = selected;
        keepGoingButton.GetComponent<Image>().sprite = notSelected;
    }



    //private void MatInputs()
    //{


    //    string fmActionData = InitBLE.GetFMResponse();//InitBLE.PluginClass.CallStatic<string>("_getFMResponse");
    //    Debug.Log("Json Data from fmdriver : " + fmActionData);
    //    FmDriverResponseInfo singlePlayerResponse = JsonUtility.FromJson<FmDriverResponseInfo>(fmActionData);
    //    if (fmActionData == null) //if mat is not connected
    //        return;
    //    if (PlayerSession.Instance.currentYipliConfig.oldFMResponseCount != singlePlayerResponse.count)
    //    {
    //        Debug.Log("FMResponse " + fmActionData);
    //        PlayerSession.Instance.currentYipliConfig.oldFMResponseCount = singlePlayerResponse.count;
    //        if (singlePlayerResponse.playerdata[0].fmresponse.action_id.Equals(ActionAndGameInfoManager.getActionIDFromActionName(YipliUtils.PlayerActions.RIGHT)))
    //        {
    //            Debug.Log("Inside Main Menu : Left Key");
    //            //MovePointerToNextButton();
    //        }
    //        else if (singlePlayerResponse.playerdata[0].fmresponse.action_id.Equals(ActionAndGameInfoManager.getActionIDFromActionName(YipliUtils.PlayerActions.LEFT)))
    //        {
    //            Debug.Log("Inside Main Menu : Right Key");
    //            //MovePointerToPrevButton();
    //        }
    //        else if (singlePlayerResponse.playerdata[0].fmresponse.action_id.Equals(ActionAndGameInfoManager.getActionIDFromActionName(YipliUtils.PlayerActions.ENTER)))
    //        {
    //            Debug.Log("Inside Main Menu : Enter Key");
    //            if (isOpened)
    //            {
    //                DemoButtonGameplay();
    //                isOpened = false;
    //                buttonExecuted = true;
    //            }
    //        }
    //    }
    //}

    //public void MatInputOnStart()
    //{
    //    matInputSystem.UpdateButtonList(DemoPanelButtons, 0, true);
    //}

    public void EnableStretchingOptionsPanel()
    {
        stretchingTimeOptionsPanel.SetActive(true);
        BackendDataHandler.instance.WORKOUT_TYPE = "stretch";
        matInputSystem.currentB = playStretching;
        List<Button> play = new List<Button>();
        play.Add(playStretching);
        predefinedWorkoutManager.optionsButtons = play;
        stretchDefaultTimeButton.GetComponent<Button>().onClick.Invoke();
        stretchDefaultTimeButton_Highlighter.GetComponent<Image>().enabled = true;
        workoutConfig.intensity = defaultIntensityForStretching;
        soundManager.PlayAudioDialogue(1);
    }

    public void DisableDefaultHighlighterStretching()
    {
        stretchDefaultTimeButton_Highlighter.GetComponent<Image>().enabled = false;
    }

    public void DisableDefaultHighlighterLight()
    {
        lightDefaultTimeButton_Highlighter.GetComponent<Image>().enabled = false;
    }




    public void DisableStretchingOptionsPanel()
    {
        stretchingTimeOptionsPanel.SetActive(false);
        
    }

    public void EnableOfferingsPanel()
    {
        OfferingsPanel.SetActive(true);
        soundManager.PlayAudioDialogue(0);
    }

    public void DisableOfferingsPanel()
    {
        OfferingsPanel.SetActive(false);
        landingPage.SetActive(true);
        soundManager.PauseDialogue();
    }

    public void IsStretchingWorkout()
    {
        inStretchingWorkout = true;
        
    }

    public void IsCoreWorkout()
    {
        inCoreWorkout = true;
    }

    public void InLightWorkout()
    {
        inLightWorkout = true;
    }

    public void EnabledFullWorkoutOptions()
    {
        matInputSystem.currentB = playFull;
        List<Button> play = new List<Button>();
        play.Add(playFull);
        predefinedWorkoutManager.optionsButtons = play;
        soundManager.PlayAudioDialogue(1);
    }


    public void EnableCoreOptionsPanel()
    {
        coreOptionsPanel.SetActive(true);
        BackendDataHandler.instance.WORKOUT_TYPE = "core";
        time10Highlighter = timehiglighersForCore[0];
        time15Highlighter = timehiglighersForCore[1];
        time20Highlighter = timehiglighersForCore[2];

        lowHighligher = intensityHiglighersForCore[0];
        mediumHighlighter = intensityHiglighersForCore[1];
        highHighlighter = intensityHiglighersForCore[2];
        matInputSystem.currentB = playCore;
        List<Button> play = new List<Button>();
        play.Add(playCore);
        predefinedWorkoutManager.optionsButtons = play;
        
        coreDefaultTime.GetComponent<Button>().onClick.Invoke();
        coreDefaultIntensity.GetComponent<Button>().onClick.Invoke();
        coreDefaultTime_HighLighter.SetActive(true);
        coreDefaultIntensity_Highlighter.SetActive(true);
        soundManager.PlayAudioDialogue(1);
    }
    
    public void DisableCoreOptionsPanel()
    {
        coreOptionsPanel.SetActive(false);
        
    }

    public void EnableLightWorkoutOptionsPanel()
    {
        lightWorkoutOptionsPanel.SetActive(true);
        BackendDataHandler.instance.WORKOUT_TYPE = "light";
        matInputSystem.currentB = playLight;
        List<Button> play = new List<Button>();
        play.Add(playLight);
        predefinedWorkoutManager.optionsButtons = play;
        lightDefaultTimeButton.GetComponent<Button>().onClick.Invoke();
        lightDefaultTimeButton_Highlighter.GetComponent<Image>().enabled = true;
        soundManager.PlayAudioDialogue(1);
    }

    public void DisableLightWorkoutOptionsPanel()
    {
        lightWorkoutOptionsPanel.SetActive(false);
    }

    public void SetFullWorkoutType()
    {
        BackendDataHandler.instance.WORKOUT_TYPE = "full";
    }

    public void GoToLandingPageFromFullWorkout()
    {
        optionsPanel.SetActive(false);
        OfferingsPanel.SetActive(true);
    }

    public void GoToOfferingsPanelFromStretching()
    {
        stretchingTimeOptionsPanel.SetActive(false);
        OfferingsPanel.SetActive(true);
    }

    public void GoToOfferingsPanelFromCore()
    {
        coreOptionsPanel.SetActive(false);
        OfferingsPanel.SetActive(true);
        time10Highlighter = time10Temp;
        time15Highlighter = time15Temp;
        time20Highlighter = time20Temp;

        lowHighligher = intensityLowPrev;
        mediumHighlighter = intensityMedPrev;
        highHighlighter = intensityHighPrev;
    }

    public void GoToOfferingsPanelFromLightWorkout()
    {
        lightWorkoutOptionsPanel.SetActive(false);
        OfferingsPanel.SetActive(true);
    }

    public void GoToHomeScreen()
    {
        optionsPanel.SetActive(false);
        lightWorkoutOptionsPanel.SetActive(false);
        stretchingTimeOptionsPanel.SetActive(false);
        coreOptionsPanel.SetActive(false);
        OfferingsPanel.SetActive(false);
        landingPage.SetActive(true);
    }






}


    


