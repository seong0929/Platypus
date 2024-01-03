// SummonController.cs -- Script that controls the movement and animation of the summons
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

public class SummonController : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody2D Rigidbody2D;
    private string[] _animationStates = new string[] { "Idle", "Move", "Attack", "Skill", "Dead", "Respawn", "Ult" };
    private float[] _animationMultiplier = new float[] { 1, 1, 1, 1, 1, 1, 1 };
    // Set animation state

    void Start()
    {
        Animator = GetComponent<Animator>();
        if (Animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }

        InitAnimationStatesandMultipliers();
    }

    // Set animation state and make it play in certain seconds
    public void SetAnimationState(string state, float seconds = 1)
    {
        if( _animationStates.Contains(state) == false)
        {
            Debug.LogError("Invalid animation state: " + state);
            return;
        }

        foreach (string animationState in _animationStates)
        {
            if (animationState.Equals(state)) continue;
            Animator.SetBool(animationState, false);
        }
        
        Animator.SetBool(state, true);
        Animator.SetTrigger(state);
        Animator.speed = _animationMultiplier[GetAnimationStateIndex(state)] * seconds;
    }

    #region initialization
    private void InitAnimationStatesandMultipliers()
    {
        AnimationClip[] clips = Animator.runtimeAnimatorController.animationClips;

        string[] possibleAnimationStates = new string[clips.Length];
        float[] animationMultiplier = new float[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            AnimationClip clip = clips[i];

            string clipName = clip.name;
            possibleAnimationStates[i] = clipName;

            float clipLength = clip.length;
            float multiplier = 1 / clipLength;
            animationMultiplier[i] = multiplier;
        }

        _animationStates = possibleAnimationStates;
        _animationMultiplier = animationMultiplier;
    }
    #endregion

    #region helper functions
    private int GetAnimationStateIndex(string state)
    {
        for (int i = 0; i < _animationStates.Length; i++)
        {
            if (_animationStates[i].Equals(state)) return i;
        }
        return -1;
    }
    #endregion
}
