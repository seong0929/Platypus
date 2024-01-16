using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;
using static Constants;
using System.Collections;
using System;
using System.Data;

public struct SummonStats
{
    public float Health;
    public float Defence;
    public float MoveSpeed;
    public float DamageCoefficient;
    public float ActionSpeedCoefficient;
    public float CriticalChanceCoefficient;
}
public struct GivenSkillContainer
{
    public ActionStats Stun;
}

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : MonoBehaviour
{
    protected ESummon _eSummon;
    protected SummonStats _baseStats;
    protected SummonStats _currentStats;
    protected List<Summon> _teamMates = new List<Summon>();
    protected List<Summon> _enemies = new List<Summon>();
    private ESummonState _summonState;
    protected Vector2 _spawnPosition;
    protected Node _root = null;
    protected SummonController _summonController;    
    protected Dictionary<string, Skill> _skillDictionary = new Dictionary<string, Skill>();

    private GivenSkillContainer _hardCCContainer;

    // Battle Area Boundaries
    private float _MaxAreaX;
    private float _MaxAreaY;
    private float _minAreaX;
    private float _minAreaY;

    public Rigidbody2D Rigidbody2D;

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
        update_for_inspectator();
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

    #region For prospective use

    //Current stats
    [Header("Current Stats")]
    public float p_Health;
    public float p_Defence;
    public float p_MoveSpeed;
    public float p_DamageCoefficient;
    public float p_ActionSpeedCoefficient;
    public float p_CriticalChanceCoefficient;

    [Header("Base Stats")]
    //base stats
    public float b_Health;
    public float b_Defence;
    public float b_MoveSpeed;
    public float b_DamageCoefficient;
    public float b_ActionSpeedCoefficient;
    public float b_CriticalChanceCoefficient;

    [Header("Summon")]
    public ESummon p_eSummon;
    public List<Summon> p_teamMates = new List<Summon>();
    public List<Summon> p_enemies = new List<Summon>();

    [Header("Summon info")]
    public ESummonState p_summonState;
    public Vector2 p_spawnPosition;
    public GivenSkillContainer p_hardCCContainer;

    [Header("normal Attack/Action Info")]
    public float a_CoolTime;
    public float a_Damage;
    public float a_Range;
    public float a_Duration;
    public float a_Heal;
    public float a_CriticalChance;
    public float a_CriticalCoefficient;

    [Header("Skill Info")]
    public float sk_CoolTime;
    public float sk_Damage;
    public float sk_Range;
    public float sk_Duration;
    public float sk_Heal;
    public float sk_CriticalChance;
    public float sk_CriticalCoefficient;

    [Header("ULT Info")]
    public float u_CoolTime;
    public float u_Damage;
    public float u_Range;
    public float u_Duration;
    public float u_Heal;
    public float u_CriticalChance;
    public float u_CriticalCoefficient;

    protected void update_for_inspectator()
    {
        Debug.Log("updating inspectator infos");

        p_Health = _currentStats.Health;
        p_Defence = _currentStats.Defence;
        p_MoveSpeed = _currentStats.MoveSpeed;
        p_DamageCoefficient = _currentStats.DamageCoefficient;
        p_ActionSpeedCoefficient = _currentStats.ActionSpeedCoefficient;
        p_CriticalChanceCoefficient = _currentStats.CriticalChanceCoefficient;

        b_Health = _baseStats.Health;
        b_Defence = _baseStats.Defence;
        b_MoveSpeed = _baseStats.MoveSpeed;
        b_DamageCoefficient = _baseStats.DamageCoefficient;
        b_ActionSpeedCoefficient = _baseStats.ActionSpeedCoefficient;
        b_CriticalChanceCoefficient = _baseStats.CriticalChanceCoefficient;

        p_eSummon = _eSummon;

        p_teamMates = _teamMates;
        p_enemies = _enemies;

        p_summonState = _summonState;
        p_spawnPosition = _spawnPosition;
        p_hardCCContainer = _hardCCContainer;

        //attack from dictionary
        a_CoolTime = _skillDictionary["attack"].GetCoolTime();
        a_Damage = _skillDictionary["attack"].GetDamage();
        a_Range = _skillDictionary["attack"].GetRange();
        a_Duration = _skillDictionary["attack"].GetDuration();
        a_Heal = _skillDictionary["attack"].GetHeal();
        a_CriticalChance = _skillDictionary["attack"].GetCriticalChance();
        a_CriticalCoefficient = _skillDictionary["attack"].GetCriticalCoefficient();

        //skill from dictionary
        sk_CoolTime = _skillDictionary["skill"].GetCoolTime();
        sk_Damage = _skillDictionary["skill"].GetDamage();
        sk_Range = _skillDictionary["skill"].GetRange();
        sk_Duration = _skillDictionary["skill"].GetDuration();
        sk_Heal = _skillDictionary["skill"].GetHeal();
        sk_CriticalChance = _skillDictionary["skill"].GetCriticalChance();
        sk_CriticalCoefficient = _skillDictionary["skill"].GetCriticalCoefficient();

        //ult
        u_CoolTime = _skillDictionary["ult"].GetCoolTime();
        u_Damage = _skillDictionary["ult"].GetDamage();
        u_Range = _skillDictionary["ult"].GetRange();
        u_Duration = _skillDictionary["ult"].GetDuration();
        u_Heal = _skillDictionary["ult"].GetHeal();
        u_CriticalChance = _skillDictionary["ult"].GetCriticalChance();
        u_CriticalCoefficient = _skillDictionary["ult"].GetCriticalCoefficient();
    }

    public void Make_Inspectator_Changes_update_real_value()
    {
        //base stats
        _baseStats.Health = b_Health;
        _baseStats.Defence = b_Defence;
        _baseStats.MoveSpeed = b_MoveSpeed;
        _baseStats.DamageCoefficient = b_DamageCoefficient;
        _baseStats.ActionSpeedCoefficient = b_ActionSpeedCoefficient;
        _baseStats.CriticalChanceCoefficient = b_CriticalChanceCoefficient;

        //current stats
        _currentStats.Health = p_Health;
        _currentStats.Defence = p_Defence;            
        _currentStats.MoveSpeed = p_MoveSpeed;
        _currentStats.DamageCoefficient = p_DamageCoefficient;
        _currentStats.ActionSpeedCoefficient = p_ActionSpeedCoefficient;
        _currentStats.CriticalChanceCoefficient = p_CriticalChanceCoefficient;

        //summon info
        //_eSummon = p_eSummon;
        //_teamMates = p_teamMates;
        //_enemies = p_enemies;
        //_summonState = p_summonState;
        _spawnPosition = p_spawnPosition;
        _hardCCContainer = p_hardCCContainer;

        //attack from dictionary
        _skillDictionary["attack"].SetCoolTime(a_CoolTime);
        _skillDictionary["attack"].SetDamage(a_Damage);
        _skillDictionary["attack"].SetRange(a_Range);
        _skillDictionary["attack"].SetDuration(a_Duration);
        _skillDictionary["attack"].SetHeal(a_Heal);
        _skillDictionary["attack"].SetCriticalChance(a_CriticalChance);
        _skillDictionary["attack"].SetCriticalCoefficient(a_CriticalCoefficient);
        
        //skill from dictionary
        _skillDictionary["skill"].SetCoolTime(sk_CoolTime);
        _skillDictionary["skill"].SetDamage(sk_Damage);
        _skillDictionary["skill"].SetRange(sk_Range);
        _skillDictionary["skill"].SetDuration(sk_Duration);
        _skillDictionary["skill"].SetHeal(sk_Heal);
        _skillDictionary["skill"].SetCriticalChance(sk_CriticalChance);
        _skillDictionary["skill"].SetCriticalCoefficient(sk_CriticalCoefficient);
        
        //ult
        _skillDictionary["ult"].SetCoolTime(u_CoolTime);
        _skillDictionary["ult"].SetDamage(u_Damage);
        _skillDictionary["ult"].SetRange(u_Range);
        _skillDictionary["ult"].SetDuration(u_Duration);
        _skillDictionary["ult"].SetHeal(u_Heal);
        _skillDictionary["ult"].SetCriticalChance(u_CriticalChance);
        _skillDictionary["ult"].SetCriticalCoefficient(u_CriticalCoefficient);
    }
    #endregion
}