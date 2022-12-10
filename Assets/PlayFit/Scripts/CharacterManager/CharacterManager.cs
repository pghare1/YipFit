using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //Required Variables
    [Header("Character")]
    public GameObject characterModel = null;
    [SerializeField] private Animator characterAnimator = null;

    [SerializeField] public GameObject characterModelCanvas = null;
    [SerializeField] private Animator characterAnimatorCanvas = null;

    [Header("Required Scriptables")]
    [SerializeField] private CharacterAnimationHandler characterAnimationHandlerOBJ;
    [SerializeField] private CharacterAnimationHandler canvasCharacterAnimationHandlerOBJ;

    //Required Avatars
    [Header("Required Avatar")]
    [SerializeField] private Avatar[] idleAvatars;

    //Required Avatars
    [Header("Required OverrideController")]
    [SerializeField] private AnimatorOverrideController[] idleOverrideControllers;
    [SerializeField] private AnimatorOverrideController[] idleOverrideControllersCanvas;

    //private Variables
    private int randomNumber = -1;

    //Getters and Setters
    public CharacterAnimationHandler CharacterAnimationHandlerOBJ { get => characterAnimationHandlerOBJ; set => characterAnimationHandlerOBJ = value; }

    public CharacterAnimationHandler CharacterAnimationHandlerCanvasOBJ { get => canvasCharacterAnimationHandlerOBJ; set => canvasCharacterAnimationHandlerOBJ = value; }

    //Unity Methods
    private void Start()
    {
        HideMainCharacter();
    }

    public void ApplyIdleAnimation()
    {
        randomNumber = Random.Range(0, idleAvatars.Length);
        ApplyProperOverride(idleAvatars[randomNumber], idleOverrideControllers[randomNumber]);
    }

    public void ApplyIdleAnimationForCanvas()
    {
       randomNumber = Random.Range(0, idleAvatars.Length);
       
        ApplyProperOverrideCanvas(idleOverrideControllersCanvas[randomNumber]);
    }

    //Custom Methods
    public void ApplyProperOverride(Avatar providedAvatar, AnimatorOverrideController providedOverride, float animationSpeed = 1f)
    {
        //characterAnimator.avatar = providedAvatar;
        characterAnimator.runtimeAnimatorController = providedOverride;
        characterAnimator.speed = animationSpeed;    
        
    }

    public void ApplyProperOverrideCanvas(AnimatorOverrideController providedOverride, float animationSpeed = 1f)
    {
        characterAnimatorCanvas.runtimeAnimatorController = providedOverride;
        characterAnimatorCanvas.speed = animationSpeed;

    }

    public void DisplayMainCharacter()
    {
        characterModel.SetActive(true);
    }
    public void HideMainCharacter()
    {
        characterModel.SetActive(false);
    }
    
    

}
