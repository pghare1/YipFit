using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;

public class MatInputSystem : MonoBehaviour
{
        int FMResponseCount = -1;
    [SerializeField] private PredefinedWorkoutManager predefinedWorkoutManager = null;
    [SerializeField] private PauseWorkoutManager pauseWorkoutManager = null;

    [SerializeField] int currentButtonIndex;

    const string LEFT = "left";
    const string RIGHT = "right";
    const string ENTER = "enter";

    public Button currentB;
    [SerializeField] private List<Button> currentMenuButtons;

    YipliUtils.PlayerActions detectedAction;

    public List<Button> CurrentMenuButtons { get => currentMenuButtons; set => currentMenuButtons = value; }
    public YipliUtils.PlayerActions DetectedAction { get => detectedAction; set => detectedAction = value; }

    public Sprite borderButtonSprite; 

    public Sprite selectedButton;
    public Sprite defaultButton;
   // public Sprite optionsSelectedButton;
   // public Sprite optionsDefaultButton;

    bool isThisOptionsPanel = false;
    public bool isThisPausePanel = false;

    private int totalActionCount = 0;
    private int lastActionCount;

    private void Awake()
    {
    
    }

    

    void Update()
    {
         GetMatUIKeyboardInputs();
         ManageMatActions();
#if UNITY_EDITOR
        KeyBoardInputHandler();
#endif
    }

    public void SetProperClusterID(int clusterID)
    {
        try
        {
            Debug.LogError("provided clusterID : " + clusterID);
            YipliHelper.SetGameClusterId(clusterID);
        }
        catch (Exception e)
        {
            Debug.Log("Something went wrong with setting the cluster id : " + e.Message);
        }
    }

    private void ManageMatActions()
    {
        if (predefinedWorkoutManager.isBreakOn)
            return;


        string fmActionData = InitBLE.GetFMResponse();
            Debug.LogError("Json Data from Fmdriver : " + fmActionData);

            /* New FmDriver Response Format
               {
                  "count": 1,                 # Updates every time new action is detected
                  "timestamp": 1597237057689, # Time at which response was packaged/created by Driver
                  "playerdata": [                      # Array containing player data
                    {
                      "id": 1,                         # Player ID (For Single-player-1 , Multiplayer it could be 1 or 2 )
                      "fmresponse": {
                        "action_id": "9D6O",           # Action ID-Unique ID for each action. Refer below table for all action IDs
                        "action_name": "Jump",         # Action Name for debugging (Gamers should strictly check action ID)
                        "properties": "null"           # Any properties action has - ex. Running could have Step Count, Speed
                      }
                    },
                    {null}
                  ]
                }
            */

            FmDriverResponseInfo singlePlayerResponse = null;

            try
            {
                singlePlayerResponse = JsonUtility.FromJson<FmDriverResponseInfo>(fmActionData);
            }
            catch (System.Exception e)
            {
                Debug.Log("singlePlayerResponse is having problem : " + e.Message);
            }

            if (singlePlayerResponse == null) return;

            if (singlePlayerResponse.playerdata[0].fmresponse.action_id.Equals(ActionAndGameInfoManager.getActionIDFromActionName(YipliUtils.PlayerActions.PAUSE)))
            {
                pauseWorkoutManager.PauseWorkout();
            }

        if (PlayerSession.Instance.currentYipliConfig.oldFMResponseCount < singlePlayerResponse.count)
            {
                PlayerSession.Instance.currentYipliConfig.oldFMResponseCount = singlePlayerResponse.count;

                YipliUtils.PlayerActions providedAction = ActionAndGameInfoManager.GetActionEnumFromActionID(singlePlayerResponse.playerdata[0].fmresponse.action_id);
                DetectedAction = providedAction;

                switch (providedAction)
                {
                    case YipliUtils.PlayerActions.PAUSE:
                        Debug.LogError("PausePanelActive");
                        //Time.timeScale = 0;
                        //pauseWorkoutManager.pausePanel.SetActive(true);
                        pauseWorkoutManager.PauseWorkout();
                        SetProperClusterID(0);

                        break;

                    case YipliUtils.PlayerActions.RUNNINGSTOPPED:

                        break;

                    case YipliUtils.PlayerActions.STOP:
                        //pauseWorkoutManager.EndWorkout();
                        //SetProperClusterID(0);

                        break;

                    // other actions
                    case YipliUtils.PlayerActions.JUMP:
                        Debug.LogError("Jump Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMP);
                        break;

                    case YipliUtils.PlayerActions.RUNNING:
                        Debug.LogError("Running Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.RUNNING);
                        break;

                    case YipliUtils.PlayerActions.JUMPING_JACK:

                        Debug.LogError("Jumping Jack Added :" + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMPING_JACK);
                        break;

                    case YipliUtils.PlayerActions.SKIER_JACK:

                        Debug.LogError("Skier Jack Added : " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SKIER_JACK);
                        break;

                    case YipliUtils.PlayerActions.CROSSOVER_JUMPING_JACK:
                        Debug.LogError("CrossOverJack Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.CROSSOVER_JUMPING_JACK);

                        break;

                    case YipliUtils.PlayerActions.LUNGES_RUN:
                        Debug.LogError("Lunges Run Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGES_RUN);
                        break;

                    case YipliUtils.PlayerActions.MOUNTAIN_CLIMBING:
                        Debug.LogError("MountainClimbing Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MOUNTAIN_CLIMBING);
                        break;

                    case YipliUtils.PlayerActions.PLANK_STARTED:
                        break;

                    case YipliUtils.PlayerActions.PLANK_STOPPED:
                        break;

                    case YipliUtils.PlayerActions.MULE_KICK:
                        Debug.LogError("Mule Kick Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MULE_KICK);
                        break;

                    case YipliUtils.PlayerActions.R_LEG_HOPPING:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.R_LEG_HOPPING);
                        break;

                    case YipliUtils.PlayerActions.L_LEG_HOPPING:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.L_LEG_HOPPING);
                        break;

                    case YipliUtils.PlayerActions.BURPEE:
                        Debug.LogError("Burpee Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.BURPEE);//
                        break;

                    case YipliUtils.PlayerActions.JUMPS_180:

                        Debug.LogError("180 Jump Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMPS_180);
                        break;

                    case YipliUtils.PlayerActions.DIAGONAL_JUMP:
                        Debug.LogError("Diagonal Jump Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.DIAGONAL_JUMP);//
                        break;

                    case YipliUtils.PlayerActions.FORWARD_JUMP:
                        Debug.LogError("Forward Jump: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.FORWARD_JUMP);
                        break;

                    case YipliUtils.PlayerActions.BACKWARD_JUMP:
                        Debug.LogError("Backward Jump: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.BACKWARD_JUMP);
                        break;

                    case YipliUtils.PlayerActions.RIGHT_JUMP:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.RIGHT_JUMP);
                        break;

                    case YipliUtils.PlayerActions.LEFT_JUMP:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LEFT_JUMP);
                        break;

                    case YipliUtils.PlayerActions.STAR_JUMP:
                        Debug.LogError("Star Jump Jump: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.STAR_JUMP);//
                        break;

                    case YipliUtils.PlayerActions.CHEST_JUMP:

                        Debug.LogError("Chest Jump: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.CHEST_JUMP);
                        break;

                    case YipliUtils.PlayerActions.HOPSCOTCH:
                        Debug.LogError("Chest Jump Added");
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.CHEST_JUMP);
                        break;

                    case YipliUtils.PlayerActions.BALANCE_STARTED:
                        break;

                    case YipliUtils.PlayerActions.BALANCE_STOPPED:
                        break;

                    case YipliUtils.PlayerActions.ARM_STARTED_1:
                        break;

                    case YipliUtils.PlayerActions.ARM_STOPPED_1:
                        break;

                    case YipliUtils.PlayerActions.NINJA_KICK:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.NINJA_KICK);
                        break;

                    case YipliUtils.PlayerActions.HIGH_KNEE:
                        Debug.LogError("High Knee: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.HIGH_KNEE);
                        break;

                    case YipliUtils.PlayerActions.SQUATS_180:
                        Debug.LogError("180 Squats: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUATS_180);
                        break;

                    case YipliUtils.PlayerActions.SQUAT_AND_JUMP:
                        Debug.LogError("Squat And Jump Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_JUMP);
                        break;

                    case YipliUtils.PlayerActions.SQUAT_AND_KICK:
                        Debug.LogError("Squat and Kick Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_KICK);
                        break;

                    case YipliUtils.PlayerActions.SQUATS:
                        break;

                    case YipliUtils.PlayerActions.SQUAT_AND_JUMPING_JACK:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_JUMPING_JACK);
                        break;

                    case YipliUtils.PlayerActions.LATERAL_SQUATS:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LATERAL_SQUATS);
                        break;

                    case YipliUtils.PlayerActions.PLANK_JUMP_INS:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.PLANK_JUMP_INS);
                        break;

                    case YipliUtils.PlayerActions.LEG_DOG_3:
                        Debug.LogError("Leg Dog 3 Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LEG_DOG_3);
                        break;

                    case YipliUtils.PlayerActions.BANARSANA:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.BANARSANA);
                        break;

                    case YipliUtils.PlayerActions.AEROPLANE_POSE:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.AEROPLANE_POSE);
                        break;

                    case YipliUtils.PlayerActions.VIKRASANA:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.VIKRASANA);
                        break;

                    case YipliUtils.PlayerActions.ARDHA_CHANDRASANA:

                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.ARDHA_CHANDRASANA);
                        break;

                    //case YipliUtils.PlayerActions.BASIC_PLANK:

                    //    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.BASIC_PLANK);
                    //    break;

                    case YipliUtils.PlayerActions.MALASANA:
                        Debug.LogError("Malasana Added: " + YipliHelper.GetGameClusterId());
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MALASANA);
                        break;

                    // UI input executions
                    case YipliUtils.PlayerActions.LEFT:
                        ProcessMatInputs(LEFT);
                        break;

                    case YipliUtils.PlayerActions.RIGHT:
                        ProcessMatInputs(RIGHT);
                        break;

                    case YipliUtils.PlayerActions.ENTER:
                        ProcessMatInputs(ENTER);
                        break;
                case YipliUtils.PlayerActions.OBLIQUE_JACK:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.OBLIQUE_JACK);
                    break;
                case YipliUtils.PlayerActions.CURTSEY_LUNGE:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.CURTSEY_LUNGE);
                    break;
                case YipliUtils.PlayerActions.FORWARD_LUNGE:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.FORWARD_LUNGE);
                    break;
                case YipliUtils.PlayerActions.LUNGE_PILE:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGE_PILE);
                    break;
                case YipliUtils.PlayerActions.QUICK_STEP_LUNGE_JUMP:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.QUICK_STEP_LUNGE_JUMP);
                    break;
                case YipliUtils.PlayerActions.REAR_LUNGE_CHEST_KNEE:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.REAR_LUNGE_CHEST_KNEE);
                    break;
                case YipliUtils.PlayerActions.REAR_LUNGE:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.REAR_LUNGE);
                    break;
                case YipliUtils.PlayerActions.RUNNING_LUNGE:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.RUNNING_LUNGE);
                    break;
                case YipliUtils.PlayerActions.SIDEWISE_JUMPS:
                    PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SIDEWISE_JUMPS);
                    break;

                default:
                        Debug.LogError("Wrong Action is detected : " + providedAction.ToString());
                        break;
                }
                //ActionAdderToActionCount(providedAction);
            }
        
    }

    private void KeyBoardInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetActionEnumByClusterId();
        }
    }

    

    private void GetActionEnumByClusterId()
    {
        
        int clusterId = YipliHelper.GetGameClusterId();

        switch (clusterId)
        {
            case 206:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMPING_JACK);
                break;
            case 205:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMP);
                break;
            case 217:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMPS_180);
                break;
            case 226:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.BURPEE);
                break;
            case 214:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.CHEST_JUMP);
                break;
            case 202:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.RUNNING);
                break;
            case 204:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SKIER_JACK);
                break;
            case 220:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUATS_180);
                break;
            case 231:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.AEROPLANE_POSE);
                break;
            case 215:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.CROSSOVER_JUMPING_JACK);
                break;
            case 240:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGES_RUN);
                break;
            case 203:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.HIGH_KNEE);
                break;
            case 223:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LATERAL_SQUATS);
                break;
            case 209:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.HOPSCOTCH);
                break;
            case 211:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.L_LEG_HOPPING);
                break;
            case 241:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGES_RUN);
                break;
            case 236:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGES_RUN);
                break;
            case 239:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGES_RUN);
                break;
            case 237:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGES_RUN);
                break;
            case 210:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMP);
                break;
            case 213:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.STAR_JUMP);
                break;
            case 219:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MOUNTAIN_CLIMBING);
                break;
            case 221:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MULE_KICK);
                break;
            case 216:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_KICK);
                break;
            case 218:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_JUMP);
                break;
            case 222:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_JUMPING_JACK);
                break;
            case 229:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LEG_DOG_3);
                break;
            default:
                PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMP);
                break;
        }
        totalActionCount++;
        lastActionCount = totalActionCount;
    }

    public void UpdateButtonList(List<Button> newButtons, int newCurrentButtonIndex, bool isThisOptionsPanel = false)
    {
        /*if (currentMenuButtons != null)
        {
            currentMenuButtons.Clear();
        }*/
        currentButtonIndex = newCurrentButtonIndex;
        currentMenuButtons = newButtons;
        this.isThisOptionsPanel = isThisOptionsPanel;

        ManageCurrentButton();
    }

    private void ProcessMatInputs(string matInput)
    {
        switch (matInput)
        {
            case LEFT:
                currentButtonIndex = GetPreviousButton();
                ManageCurrentButton();
                break;

            case RIGHT:
                currentButtonIndex = GetNextButton();
                ManageCurrentButton();
                break;

            case ENTER:
                Debug.LogError("Button Clicked : " + currentB.gameObject.name);
                currentB.onClick.Invoke();
                break;

            default:
                Debug.Log("Wrong Input");
                break;
        }
    }

    private int GetNextButton()
    {
        if ((currentButtonIndex + 1) == currentMenuButtons.Count)
        {
            return 0;
        }
        else
        {
            return currentButtonIndex + 1;
        }
    }

    private int GetPreviousButton()
    {
        if (currentButtonIndex == 0)
        {
            return currentMenuButtons.Count - 1;
        }
        else
        {
            return currentButtonIndex - 1;
        }
    }

    private void ManageCurrentButton()
    {
        if (isThisOptionsPanel)
        {
            for (int i = 0; i < currentMenuButtons.Count; i++)
            {
                if (i == currentButtonIndex)
                {
                    currentMenuButtons[i].GetComponent<Image>().sprite = borderButtonSprite;
                    currentB = currentMenuButtons[i];
                    Debug.Log("button tag : " + currentMenuButtons[i].gameObject.tag);
                    
                }
                else
                {
                    // do nothing
                    
                    if (currentMenuButtons[i].gameObject.tag == "optionbuttons" && currentMenuButtons[i].transform.GetChild(1).gameObject.activeInHierarchy)
                    {
                        currentMenuButtons[i].GetComponent<Image>().sprite = selectedButton;
                    }
                    else
                    {
                        currentMenuButtons[i].GetComponent<Image>().sprite = defaultButton;
                    }
                    
                }
            }
        }
        else if (isThisPausePanel)
        {
            for (int i = 0; i < currentMenuButtons.Count; i++)
            {
                if (i == currentButtonIndex)
                {
                    Color color = currentMenuButtons[i].transform.Find("Image").GetComponent<Image>().color;
                    color = new Color(1f, 0.22f, 0f, 1f);
                    currentMenuButtons[i].transform.Find("Image").GetComponent<Image>().color = color;
                    currentB = currentMenuButtons[i];
                    if (currentMenuButtons[i].gameObject.tag == "pauseButton")
                    {
                        currentMenuButtons[i].GetComponent<RectTransform>().localScale = new Vector2(1.15f, 1.15f);
                    }
                }
                else
                {
                    Color color = currentMenuButtons[i].transform.Find("Image").GetComponent<Image>().color;
                    color = new Color(0.27f, 0.27f, 0.27f, 1f);
                    currentMenuButtons[i].transform.Find("Image").GetComponent<Image>().color = color;
                    //currentB = currentMenuButtons[i];
                    if (currentMenuButtons[i].gameObject.tag == "pauseButton")
                    {
                        currentMenuButtons[i].GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < currentMenuButtons.Count; i++)
            {
                if (i == currentButtonIndex)
                {
                    currentMenuButtons[i].GetComponent<Image>().sprite = selectedButton;
                    if (currentMenuButtons[i].gameObject.tag == "pauseButton")
                    {
                        currentMenuButtons[i].GetComponent<RectTransform>().localScale = new Vector2(1.15f, 1.15f);
                    }
                    currentB = currentMenuButtons[i];
                }
                else
                {
                    currentMenuButtons[i].GetComponent<Image>().sprite = defaultButton;
                    if (currentMenuButtons[i].gameObject.tag == "pauseButton")
                    {
                        currentMenuButtons[i].GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
                    }
                }
            }
        }
    }

    private void GetMatUIKeyboardInputs()
    {
        // left to right play, changeplayer, gotoyipli, exit
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ProcessMatInputs(LEFT);
        }

        // left to right play, changeplayer, gotoyipli, exit
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ProcessMatInputs(RIGHT);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ProcessMatInputs(ENTER);
        }
    }
}
