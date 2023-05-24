using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : SummonBase
{
    public bool isMoving;
    public bool isAlive = true;

    //ToDo: BattleManager���� �� �Ǻ� �ʱ�ȭ
    public bool myTeam;
    float deadTime;

    protected float[] stats;  //�ӽ� ���� ��Ÿ�, �̵��ӵ�, ü��, ������, ����
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: ��ų, skillIndex == 1: ��

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

    // BehaviorTree���� ����� �޼���
    protected abstract Node CreateBehaviorTree();
    // ToDo: �ǰ� Ÿ�� ���ȭ
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