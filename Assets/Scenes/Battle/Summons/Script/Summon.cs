using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    //ToDo: BattleManager에서 팀 판별 초기화
    public bool MyTeam;
    protected float[] stats;  //임시 스탯 사거리, 이동속도, 체력, 데미지, 방어력
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: 스킬, skillIndex == 1: 궁
    private bool _isAlive = true;
    private bool _isCC = false;
    private float _deadTime;

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
    public abstract void Attack(Summon target, float damage);
    protected abstract Node CreateBehaviorTree();
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
    public bool IsCC
    {
        get { return _isCC; }
        set { IsCC = value; }
    }
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
                TakeDamage(collision.GetComponent<Summon>().Stats[((int)Enums.ESummonStats.NormalDamage)]);
                break;
            default:
                break;
        }
    }
}
public class Skill
{
    public float skiilCounter = 0;
    public virtual void Execute(GameObject summon, GameObject target, Animator animator)
    {
        // Implement the basic behavior of the skill here
    }
}