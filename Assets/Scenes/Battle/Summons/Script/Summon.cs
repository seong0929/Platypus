using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    public bool IsMoving;
    public bool IsAlive = true;
    //ToDo: BattleManager���� �� �Ǻ� �ʱ�ȭ
    public bool MyTeam;
    protected float[] stats;  //�ӽ� ���� ��Ÿ�, �̵��ӵ�, ü��, ������, ����
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: ��ų, skillIndex == 1: ��
    private float _deadTime;

    public virtual bool IsDead()
    {
        if(stats[((int)Enums.ESummonStats.Health)] <= 0)
        {
            IsAlive = false;
            return IsAlive;
        }
        IsAlive = true;
        return IsAlive;
    }
    public abstract void Attack(Summon target, float damage);

    // BehaviorTree���� ����� �޼���
    protected abstract Node CreateBehaviorTree();
    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(damage - stats[((int)Enums.ESummonStats.Defence)], 0);
        stats[((int)Enums.ESummonStats.Health)] -= actualDamage;
        if (stats[((int)Enums.ESummonStats.Health)] <= 0) {
            _deadTime = BattleManager.instance.GameTime;
            IsAlive = false;
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