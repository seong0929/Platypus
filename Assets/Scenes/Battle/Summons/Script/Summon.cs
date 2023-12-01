using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;
using System.Collections;

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : MonoBehaviour
{
    #region for the Inspector
    public GameObject Target; 
    public ESummonState State;
    #endregion

    public string SummonName;
    public GameObject[] Opponents;
    public ECC CurrentCC;
    public float[] CurrentCCStats;
    public ETeamSide TeamSide;
    public int SpawnPositionOrder;
    public float _deadTime;
    [SerializeField]
    public float[] BaseStats;

    [SerializeField]
    public float[] stats;  //임시 스탯: 사거리, 이동속도, 체력, 데미지, 방어력
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: 일반 공격, 1: 스킬, 2: 궁
//    [SerializeField]
    private bool _isAlive = true;

    #region settings
    public float[] Stats
    {
        get { return stats; }
        set { stats = value; }
    }
    protected abstract Node CreateBehaviorTree();
    #endregion
    #region Attack
    public bool IsDead()
    {
        if(stats[((int)ESummonStats.Health)] <= 0)
        {
            _isAlive = false;
            return !_isAlive;
        }
        _isAlive = true;
        return !_isAlive;
    }
    // 데미지 받은 함수
    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(damage - stats[((int)ESummonStats.Defence)], 0);
        stats[((int)ESummonStats.Health)] -= actualDamage;
        if (stats[((int)ESummonStats.Health)] <= 0) 
        {
            _deadTime = BattleManager.instance.GameTime;
            _isAlive = false;
        }
    }
    // 데미지 주는 함수
    public void GiveDamage(Summon target, float damage)
    {
        // 근거리는 스킬에, 원거리는 원거리 무기에 추가
        target.TakeDamage(damage);
    }
    #endregion
    #region CC
    // CC 걸려있는 지 여부 판단
    public bool HasCC()
    {
        if (CurrentCC == ECC.None) return false;
        else return true;
    }
    // CC 혹은 죽었는지 확인
    // - 일반 task node를 interrupt하기 위해 사용
    public bool CheckCriticalEvent()
    {
        if(HasCC() || IsDead()) return true;
        return false;
    }
    #endregion

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void ResetStats()
    {
        stats = (float[])BaseStats.Clone();
        Debug.Log("ResetStats");
    }
}