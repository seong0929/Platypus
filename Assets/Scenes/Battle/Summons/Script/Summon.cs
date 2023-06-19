using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    //ToDo: BattleManager���� �� �Ǻ� �ʱ�ȭ
    public bool MyTeam;
    protected float[] stats;  //�ӽ� ���� ��Ÿ�, �̵��ӵ�, ü��, ������, ����
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: ��ų, skillIndex == 1: ��
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
                //TODO: ����ü Ȥ�� ������ ������ ��������
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