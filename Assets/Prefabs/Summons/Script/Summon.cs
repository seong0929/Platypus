using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : SummonBase
{
    public bool isMoving;
    public bool isAlive = true;

    //ToDo: GameManager에서 팀 판별 초기화
    public List<GameObject> myTeam;
    public List<GameObject> theirTeam;
    float deadTime;

    protected float[] stats;  //임시 스탯 사거리, 이동속도, 체력, 데미지, 방어력
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: 스킬, skillIndex == 1: 궁

    public virtual bool IsEnemy() {
        if (theirTeam.Count != 0){ return true;}
        else { return false; }
    }
    public virtual bool IsDead()
    {
        if(stats[((int)Enums.ESummonStats.Health)] <= 0)
        {
            isAlive = false;
            return isAlive;
        }
        isAlive = true;
        return isAlive;
    }
    public abstract void Attack(Summon target, float damage);

    // BehaviorTree에서 사용할 메서드
    protected abstract Node CreateBehaviorTree();

    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(damage - stats[((int)Enums.ESummonStats.Defence)], 0);
        stats[((int)Enums.ESummonStats.Health)] -= actualDamage;
        if (stats[((int)Enums.ESummonStats.Health)] <= 0) {
            deadTime = BattleManager.instance.GameTime;
            isAlive = false;
        }
    }

    public void GiveDamage(Summon target, float damage)
    {
        target.TakeDamage(damage);
    }
    public float[] Stats
    {
        get { return stats; }
        set { stats = value; }
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