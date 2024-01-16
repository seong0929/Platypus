// SummonController.cs -- Script that controls the movement and animation of the summons
using System.Linq;
using UnityEngine;

public class SummonController //: MonoBehaviour
{
    public Animator Animator;
    public Rigidbody2D Rigidbody2D;

    private string[] _animationStates = new string[] { "Idle", "Move", "Attack", "Skill", "Dead", "Respawn", "Ult" };
    private float[] _animationMultiplier = new float[] { 1, 1, 1, 1, 1, 1, 1 };

    public void InitializeController(Animator animator, Rigidbody2D rigidbody2D)
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }

        Animator = animator;
        Rigidbody2D = rigidbody2D;
        InitAnimationStatesandMultipliers();
    }

    // Set animation state and make it play in certain seconds
    public void SetAnimationState(string state, float second = 1)
    {
        if(Animator.GetBool(state) == false)
        {
            Debug.Log(state + "Animation is called");
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
        }
        
        Animator.speed = _animationMultiplier[GetAnimationStateIndex(state)] * Mathf.Max(0.001f, 1/second);
        if(state.Equals("Move"))
            Animator.speed = 0.3f;
    }

    public void FlipSpriteTo(GameObject target)
    {
        if (Rigidbody2D.position.x < target.transform.position.x)
        {
            Rigidbody2D.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            Rigidbody2D.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
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
