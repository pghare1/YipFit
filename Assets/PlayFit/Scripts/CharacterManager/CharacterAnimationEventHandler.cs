using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEventHandler : MonoBehaviour
{

    [SerializeField] private Animator characterAnimator = null;

    bool calledFromEvent = false;

    public void SwitchToMainAction()
    {
        if (!calledFromEvent)
        {
            Debug.LogError("Switching To main animation");
            characterAnimator.SetBool("startMainAction", true);
            calledFromEvent = true;
        }
    }

    public void SwitchToMainActionWithoutEvent()
    {
        Debug.LogError("Switching To main animation");
        characterAnimator.SetBool("startMainAction", true);
    }

    public void SwitchToActionEnd()
    {
        characterAnimator.SetBool("endMainAction", true);
    }

    public void GoToIdleStart()
    {
        characterAnimator.SetBool("gotoIdle", true);
    }

    public void ResetBoolForMainAction()
    {
        characterAnimator.SetBool("startMainAction", false);
        characterAnimator.SetBool("endMainAction", false);
        characterAnimator.SetBool("gotoIdle", false);
    }

    public void RePlayFirstStateOfAnimation()
    {
        characterAnimator.Play("IdleStart");
    }

}
