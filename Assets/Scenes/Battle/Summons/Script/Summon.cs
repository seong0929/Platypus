using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    public ECC CurrentCC;
    public float[] CurrentCCStats;
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
    protected abstract Node CreateBehaviorTree();
    #endregion
    #region Attack
    public bool IsDead()
    {
        if(stats[((int)ESummonStats.Health)] <= 0)
        {
            _isAlive = false;
            return _isAlive;
        }
        _isAlive = true;
        return _isAlive;
    }
    // ������ ���� �Լ�
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
    // ���� �ִ� �Լ�
    public void GiveDamage(Summon target, float damage)
    {
        // �ٰŸ��� ��ų��, ���Ÿ��� ���Ÿ� ���⿡ �߰�
        target.TakeDamage(damage);
    }
    #endregion
    #region CC
    // CC �ɷ��ִ� �� ���� �Ǵ�
    public bool HasCC()
    {
        if (CurrentCC == ECC.None) return false;
        else return true;
    }
    #endregion
}