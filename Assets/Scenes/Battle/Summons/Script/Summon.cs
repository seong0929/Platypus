using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    public Enums.ECC CurrentCC;
    public float[] CurrrentCCStats;
    public bool MyTeam;     //ToDo: BattleManager에서 팀 판별 초기화

    protected float[] stats;  //임시 스탯: 사거리, 이동속도, 체력, 데미지, 방어력
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: 일반 공격, 1: 스킬, 2: 궁

    private bool _isAlive = true;
    private float _deadTime;

    #region settings
    public float[] Stats
    {
        get { return stats; }
        set { stats = value; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.name)
        {
            case "Summon":
                //TODO: 투사체 혹은 공격의 데미지 가져오기
                //TakeDamage(collision.);   // 누구의 스킬 데미지
                break;
            default:
                break;
        }
    }
    protected abstract Node CreateBehaviorTree();
    #endregion
    #region Attack
    public bool IsDead()
    {
        if(stats[((int)Enums.ESummonStats.Health)] <= 0)
        {
            _isAlive = false;
            return _isAlive;
        }
        _isAlive = true;
        return _isAlive;
    }
    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(damage - stats[((int)Enums.ESummonStats.Defence)], 0);
        stats[((int)Enums.ESummonStats.Health)] -= actualDamage;
        if (stats[((int)Enums.ESummonStats.Health)] <= 0) 
        {
            _deadTime = BattleManager.instance.GameTime;
            _isAlive = false;
        }
    }
    public void GiveDamage(Summon target, float damage)
    {
        target.TakeDamage(damage);
    }
    #endregion
    #region CC
    // CC 걸려있는 지 여부 판단
    public bool HasCC()
    {
        if (CurrentCC == Enums.ECC.None) return false;
        else return true;
    }
    #endregion
}