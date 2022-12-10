using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimationHandler", menuName = "Create CharacterAnimationHandler")]
public class CharacterAnimationHandler : ScriptableObject
{
    [SerializeField] private List<ActionAnimation> actionAnimations;
    [SerializeField] private ActionMappedTargetAreaAndIntensity actionMappedTargetAreaAndIntensities;



    public Avatar GetAvatarFromActionId(string providedActionId)
    {
        Avatar avatarToBeSent= null;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if(actionAnimation.actionId == providedActionId)
            {
                avatarToBeSent = actionAnimation.actionAvatar;
                break;
            }
        }
        return avatarToBeSent;
    }

    public AnimatorOverrideController GetAnimationOverrideFromActionId(string providedActionId)
    {
        AnimatorOverrideController animationOverrideToBeSent = null;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                animationOverrideToBeSent = actionAnimation.actionOverrideController;
                break;
            }
        }
        return animationOverrideToBeSent;
    }


    public float GetAnimationSpeed(string providedActionId)
    {
        float animationSpeed = 1f;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                animationSpeed = actionAnimation.animationSpeed;
                break;
            }
        }
        return animationSpeed;
    }

    public int GetActionClusterID(string providedActionId)
    {
        int currentActionClusterId = 0;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                currentActionClusterId = actionAnimation.clusterId;
                Debug.LogError("Cluster ID" + currentActionClusterId);
                break;
            }
        }
        return currentActionClusterId;
    }

    public string GetActionName(string providedActionId)
    {
        string currentActionName = "";

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                currentActionName = actionAnimation.actionName;
                Debug.Log("Action Name" + currentActionName);
                break;
            }
        }
        return currentActionName;
    }

    public AudioClip GetVoiceOverClip(string providedActionId)
    {
        AudioClip currentActionAudio = null;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                currentActionAudio = actionAnimation.voiceOverClip;
                break;
            }
        }
        return currentActionAudio;
    }

    public bool CheckCharacterAngleToBeSet(string providedActionId)
    {
        bool changed = false;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                changed = actionAnimation.is90DegreeAngleRequired;   
            break;
            }
        }
        return changed;
    }

    public bool CheckCharacterAngleToBeSetOnCanvas(string providedActionId)
    {
        bool changed = false;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                changed = actionAnimation.is90DegreeAngleRequired;
                break;
            }
        }
        return changed;
    }

    public bool IsThisCoreAction(string providedActionId)
    {
        bool isCoreAction = false;

        foreach (ActionAnimation actionAnimation in actionAnimations)
        {
            if (actionAnimation.actionId == providedActionId)
            {
                isCoreAction = actionAnimation.isCoreWorkout;
                break;
            }
        }
        return isCoreAction;
    }

    public int GetIntensityOfParticularAction(string actionId)
    {
        int IntensityFound = 0;
        foreach (ActionsInfo item in actionMappedTargetAreaAndIntensities.ActionList)
        {
            if (item.actionId == actionId)
            {
                IntensityFound = item.intensityScale;
                break;
            }
        }
        return IntensityFound;
    }

}

[System.Serializable]
public class ActionAnimation
{
    public string actionId;
    public string actionName;
    public Avatar actionAvatar;
    public AnimatorOverrideController actionOverrideController;
    public float animationSpeed = 1f;
    public int clusterId;
    public bool is90DegreeAngleRequired = false;
    public bool isCoreWorkout = false;
    public AudioClip voiceOverClip;
}