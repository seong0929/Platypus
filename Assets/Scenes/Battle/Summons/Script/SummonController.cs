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

    public float MoveSpeed = 1;
    public float AttackSpeed = 1;
    public float SkillSpeed = 1;
    public float UltSpeed = 1;

    private string[] _animationStates = new string[] { "Idle", "Move", "Attack", "Skill", "Dead", "Respawn", "Ult" };
    private float[] _animationMultiplier = new float[] { 1, 1, 1, 1, 1, 1, 1 };

    private float MaxAreaX;
    private float MaxAreaY;
    private float minAreaX;
    private float minAreaY;

    void Start()
    {
        Animator = GetComponent<Animator>();
        if (Animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }

        InitAnimationStatesandMultipliers();
        SetArea();
    }

    // Set animation state and make it play in certain seconds
    public void SetAnimationState(string state, float speed = 1)
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
        Animator.speed = _animationMultiplier[GetAnimationStateIndex(state)] * Mathf.Max(0.1f, speed);
    }

    public void Move(Vector2 direction)
    {   
        Rigidbody2D.velocity = direction;
    }
    public void Transport(Vector3 transform)
    {
        // make sure the summon doesn't go out of the map
        if (transform.x > MaxAreaX) transform.x = MaxAreaX;
        if (transform.x < minAreaX) transform.x = minAreaX;
        if (transform.y > MaxAreaY) transform.y = MaxAreaY;
        if (transform.y < minAreaY) transform.y = minAreaY;

        Rigidbody2D.transform.position = transform;
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

    public void SetArea()
    {
        // find "BattleArea" object
        GameObject battleArea = GameObject.Find("BattleArea");
        if (battleArea == null)
        {
            Debug.LogError("BattleArea object not found!");
            return;
        }

        // get the size of the battle area
        BoxCollider2D battleAreaCollider = battleArea.GetComponent<BoxCollider2D>();
        if (battleAreaCollider == null)
        {
            Debug.LogError("BattleArea collider not found!");
            return;
        }

        // set the size of the battle area
        MaxAreaX = battleAreaCollider.bounds.max.x;
        MaxAreaY = battleAreaCollider.bounds.max.y;
        minAreaX = battleAreaCollider.bounds.min.x;
        minAreaY = battleAreaCollider.bounds.min.y;

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
