using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;
using static Constants;
using System.Collections;
using System;
using System.Data;

[System.Serializable]
public struct SummonStats
{
    public float Health;
    public float Defence;
    public float MoveSpeed;
    public float DamageCoefficient;
    public float ActionSpeedCoefficient;
    public float CriticalChanceCoefficient;
}

[System.Serializable]
public struct GivenSkillContainer
{
    public ActionStats Stun;
}

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : MonoBehaviour
{
    protected ESummon _eSummon;

    public SummonStats _baseStats;
    public SummonStats _currentStats;
    [SerializeField]
    private ESummonState _summonState;

    [SerializeField]
    private GivenSkillContainer _hardCCContainer;
    public Rigidbody2D Rigidbody2D;

    [SerializeField]
    protected Vector2 _spawnPosition;

    protected List<Summon> _teamMates = new List<Summon>();
    protected List<Summon> _enemies = new List<Summon>();

    protected Node _root = null;
    protected SummonController _summonController;    
    protected Dictionary<string, Skill> _skillDictionary = new Dictionary<string, Skill>();

    // Battle Area Boundaries
    private float _MaxAreaX;
    private float _MaxAreaY;
    private float _minAreaX;
    private float _minAreaY;

    private float _moveSeconds = 3f;
    private float _idleSeconds = 9f;
    private float _deadSeconds = 2f;
    private float _respawnSeconds = 1.5f;

    public bool _isMoving = false;
    private bool _isRespawnAnimationEventCalled = false;
    private bool _isDeadAnimationEventCalled = false;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        if (Rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
            return;
        }
        
        SetBattleArea();
    }

    private void Update()
    {
        UpdateSkillCooldowns(Time.deltaTime);
        UpdateHardCCContainer(Time.deltaTime);
    }
    public void SetSummon(SummonStats summonStats, List<Summon> team, List<Summon> enemies, Vector2 spawnPosition)
    {
        _baseStats.Health = summonStats.Health;
        _baseStats.Defence = summonStats.Defence;
        _baseStats.MoveSpeed = summonStats.MoveSpeed;
        _baseStats.DamageCoefficient = summonStats.DamageCoefficient;
        _baseStats.ActionSpeedCoefficient = summonStats.ActionSpeedCoefficient;
        _baseStats.CriticalChanceCoefficient = summonStats.CriticalChanceCoefficient;


        _teamMates = team;
        _enemies = enemies;
        _spawnPosition = spawnPosition;
        _summonState = ESummonState.Idle;
        SetCurrentToBase();
    }
    public void SetSummonTeamEnemies(List<Summon> team, List<Summon> enemies)
    {
        _teamMates = team;
        _enemies = enemies;

    }
    public void SetSummonBasic(ESummon eSummon, Animator animator)
    {
        _eSummon = eSummon;
        // set create summon controller
        _summonController = new SummonController();
        _summonController.InitializeController(animator, Rigidbody2D);
        if (_summonController == null)
        {
            Debug.LogError("SummonController component not found!");
            return;
        }
    }

    abstract public void InitSkills();
    abstract protected Node CreateBehaviorTree();
    public ESummonState GetSummonState() { return _summonState; }

    public void Idle()
    {
        Rigidbody2D.velocity = Vector2.zero;
        _summonController.SetAnimationState("Idle", _idleSeconds);
    }

    #region Move
    public void Move(Vector2 vector2)
    {
        _isMoving = true;
        Rigidbody2D.velocity = vector2.normalized * _currentStats.MoveSpeed;
        _summonController.SetAnimationState("Move", _moveSeconds);
    }
    public void MoveTo(Summon target)
    {
        Vector2 direction = target.transform.position - this.transform.position;
        Move(direction);
    }
    public void Teleport(Vector2 transform) 
    {
        // check if the summon is out of the battle area
        if (transform.x > _MaxAreaX) transform.x = _MaxAreaX;
        if (transform.x < _minAreaX) transform.x = _minAreaX;
        if (transform.y > _MaxAreaY) transform.y = _MaxAreaY;
        if (transform.y < _minAreaY) transform.y = _minAreaY;

        Rigidbody2D.transform.position = transform;

        _isMoving = false;
    }
    #endregion

    #region Battle
    public void TakeDamage(float damageAmount)
    {
        _currentStats.Health -= damageAmount* GetDefenceCoefficient();
        if (_currentStats.Health <= 0) 
            _currentStats.Health = 0;
    }
    public void GiveDamage(List<Summon> targets, float damageAmount)
    {
        foreach (Summon target in targets)
        {
            target.TakeDamage(damageAmount*_currentStats.DamageCoefficient);
        }
    }
    public void GiveDamage(Summon target, float damageAmount)
    {
        target.TakeDamage(damageAmount*_currentStats.DamageCoefficient);
    }
    public void TakeHeal(float healAmount)
    {
        _currentStats.Health += healAmount;
        if (_currentStats.Health > _baseStats.Health)
            _currentStats.Health = _baseStats.Health;
    }
    public void GiveHeal(List<Summon> targets, float healAmount)
    {
        foreach (Summon target in targets)
        {
            target.TakeHeal(healAmount);
        }
    }
    
    public void GiveStun(List<Summon> targets, float stunTime)
    {
        foreach (Summon target in targets)
        {
            target.TakeStun(stunTime);
        }
    }

    public void TakeStun(float stunTime)
    {
        _summonState = ESummonState.HardCC;
        Rigidbody2D.velocity = Vector2.zero;
        
        if (_hardCCContainer.Stun.Duration > stunTime)
            return;
        _hardCCContainer.Stun.Duration = stunTime;

        _summonController.SetAnimationState("Stun");
        
        _isMoving = false;
    }
    public void TakeKnockBack(Vector2 direction, float knockBackAmount)
    { 
        Rigidbody2D.AddForce(direction.normalized * knockBackAmount, ForceMode2D.Impulse);
    }
    
    public void TakeSlow(float slowAmount, float slowTime)
    { 
        _currentStats.MoveSpeed *= slowAmount;
        StartCoroutine(ReleaseSlow(slowTime, slowAmount));
    }
    private IEnumerator ReleaseSlow(float slowTime, float slowAmount)
    {
        yield return new WaitForSeconds(slowTime);
        _currentStats.MoveSpeed /= slowAmount;
    }

    public void TakeBuff(EBuffType buffType,float buffAmount, float buffTime)
    {
        switch (buffType)
        {
            case EBuffType.MoveSpeed:
                _currentStats.MoveSpeed *= buffAmount;
                StartCoroutine(ReleaseBuff(buffTime, buffAmount, buffType));
                break;
            case EBuffType.Defence:
                _currentStats.Defence *= buffAmount;
                StartCoroutine(ReleaseBuff(buffTime, buffAmount, buffType));
                break;
            case EBuffType.Damage:
                _currentStats.DamageCoefficient *= buffAmount;
                StartCoroutine(ReleaseBuff(buffTime, buffAmount, buffType));
                break;
            case EBuffType.ActionSpeed:
                _currentStats.ActionSpeedCoefficient *= buffAmount;
                StartCoroutine(ReleaseBuff(buffTime, buffAmount, buffType));
                break;
            case EBuffType.CriticalChance:
                _currentStats.CriticalChanceCoefficient *= buffAmount;
                StartCoroutine(ReleaseBuff(buffTime, buffAmount, buffType));
                break;
            default:
                Debug.LogError("Invalid buff type: " + buffType);
                break;
        }
    }
    private IEnumerator ReleaseBuff(float buffTime, float buffAmount, EBuffType buffType)
    {
        yield return new WaitForSeconds(buffTime);
        switch (buffType)
        {
            case EBuffType.MoveSpeed:
                _currentStats.MoveSpeed /= buffAmount;
                break;
            case EBuffType.Defence:
                _currentStats.Defence /= buffAmount;
                break;
            case EBuffType.Damage:
                _currentStats.DamageCoefficient /= buffAmount;
                break;
            case EBuffType.ActionSpeed:
                _currentStats.ActionSpeedCoefficient /= buffAmount;
                break;
            case EBuffType.CriticalChance:
                _currentStats.CriticalChanceCoefficient /= buffAmount;
                break;
            default:
                Debug.LogError("Invalid buff type: " + buffType);
                break;
        }
    }
    
    public void GiveBuff(List<Summon> targets, BuffDebuffStats buffDebuffStats)
    {
        foreach (Summon target in targets)
        {
            if (buffDebuffStats.MoveSpeed.Duration > 0)
                target.TakeBuff(EBuffType.MoveSpeed, buffDebuffStats.MoveSpeed.Amount, buffDebuffStats.MoveSpeed.Duration);
            if (buffDebuffStats.Defence.Duration > 0)
                target.TakeBuff(EBuffType.Defence, buffDebuffStats.Defence.Amount, buffDebuffStats.Defence.Duration);
            if (buffDebuffStats.AttackDamage.Duration > 0)
                target.TakeBuff(EBuffType.Damage, buffDebuffStats.AttackDamage.Amount, buffDebuffStats.AttackDamage.Duration);
            if (buffDebuffStats.ActionSpeed.Duration > 0)
                target.TakeBuff(EBuffType.ActionSpeed, buffDebuffStats.ActionSpeed.Amount, buffDebuffStats.ActionSpeed.Duration);
            if (buffDebuffStats.CriticalChance.Duration > 0)
                target.TakeBuff(EBuffType.CriticalChance, buffDebuffStats.CriticalChance.Amount, buffDebuffStats.CriticalChance.Duration);
        }
    }
    #endregion

    public bool Die()
    {
        _isMoving = false;
        
        if(_summonState != ESummonState.Dead)
        {
            _isDeadAnimationEventCalled = false;
            _summonState = ESummonState.Dead;
            _summonController.SetAnimationState("Dead", _deadSeconds);
            return true;
        }

        if(_summonState == ESummonState.Dead && _isDeadAnimationEventCalled)
        {
            _isDeadAnimationEventCalled = false;
            return false;
        }
        return true;
    }
    public void Respawn()
    {
        _isMoving = false;
        if (_summonState != ESummonState.Respawn)
        {
            this.transform.position = _spawnPosition;

            _isRespawnAnimationEventCalled = false;

            _summonState = ESummonState.Respawn;
            _summonController.SetAnimationState("Respawn", _respawnSeconds);

            // reset stats
            SetCurrentToBase();
        }
    }
    protected void SetCurrentToBase()
    {
        _currentStats.Health = _baseStats.Health;
        _currentStats.Defence = _baseStats.Defence;
        _currentStats.MoveSpeed = _baseStats.MoveSpeed;
        _currentStats.DamageCoefficient = _baseStats.DamageCoefficient;
        _currentStats.ActionSpeedCoefficient = _baseStats.ActionSpeedCoefficient;
        _currentStats.CriticalChanceCoefficient = _baseStats.CriticalChanceCoefficient;
    }
    public void SetAnimationState(string v, float duration)
    {
        _summonController.SetAnimationState(v, duration);
    }
    public void FlipSpriteTo(Summon target)
    {
        GameObject targetObject = target.gameObject;
        _summonController.FlipSpriteTo(targetObject);
    }

    public bool CheckCriticalEvent()
    {
        if (_summonState == ESummonState.HardCC || _summonState == ESummonState.Dead)
            return false;
        return false;
    }

    protected void UpdateRootNodeData()
    {
        _root.SetData("self", this);
    }

    #region HelperFuncton
    private float GetDefenceCoefficient()
    {
        if(_baseStats.Defence > MAX_DEFENSE || _baseStats.Defence < 0.0f)
        {
            Debug.LogError("Defence is out of range");
            return 1;
        }
        return (1 - _baseStats.Defence/MAX_DEFENSE);
    }

    private void SetBattleArea()
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
        _MaxAreaX = battleAreaCollider.bounds.max.x;
        _MaxAreaY = battleAreaCollider.bounds.max.y;
        _minAreaX = battleAreaCollider.bounds.min.x;
        _minAreaY = battleAreaCollider.bounds.min.y;
    }

    protected void UpdateSkillCooldowns(float deltaTime)
    {
        foreach ((string key, Skill skill) in _skillDictionary)
        {
            skill.UpdateSkillCooldown(deltaTime * _baseStats.ActionSpeedCoefficient);
        }
    }

    protected void UpdateHardCCContainer(float deltaTime)
    {
        if (_hardCCContainer.Stun.Duration > 0)
            _hardCCContainer.Stun.Duration -= deltaTime;
    }

    #endregion

    #region Getter
    public float GetHealth() { return _currentStats.Health; }
    public List<Summon> GetTeamMates() { return _teamMates; }
    public List<Summon> GetEnemies() { return _enemies; }
    public GivenSkillContainer GetHardCCContainer() { return _hardCCContainer; }
    #endregion


    public void CallDeadAnimationEvent()
    {
        _isDeadAnimationEventCalled = true;
    }

    public void CallRespawnAnimationEvent()
    {
        _isRespawnAnimationEventCalled = true;
        _currentStats.Defence = _baseStats.Defence;
    }
}