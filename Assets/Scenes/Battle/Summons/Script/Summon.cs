using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    public Enums.ECC CurrentCC;
    public float[] CurrrentCCStats;
    public bool MyTeam;     //ToDo: BattleManager���� �� �Ǻ� �ʱ�ȭ

    protected float[] stats;  //�ӽ� ����: ��Ÿ�, �̵��ӵ�, ü��, ������, ����
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: �Ϲ� ����, 1: ��ų, 2: ��

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
                //TODO: ����ü Ȥ�� ������ ������ ��������
                //TakeDamage(collision.);   // ������ ��ų ������
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
    // CC �ɷ��ִ� �� ���� �Ǵ�
    public bool HasCC()
    {
        if (CurrentCC == Enums.ECC.None) return false;
        else return true;
    }
    #endregion
}