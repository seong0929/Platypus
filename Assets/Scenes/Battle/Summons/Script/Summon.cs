using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    public bool IsCC => _isCC;
    public Enums.ECC CurrentCC => _currentCC;
    //ToDo: BattleManager���� �� �Ǻ� �ʱ�ȭ
    public bool MyTeam;
    protected float[] stats;  //�ӽ� ���� ��Ÿ�, �̵��ӵ�, ü��, ������, ����
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: �Ϲ� ����, 1: ��ų, 2: ��
    protected List<Enums.ECC> cCs = new List<Enums.ECC>();

    private bool _isAlive = true;
    private bool _isCC = false;
    private Enums.ECC _currentCC;
    private float _deadTime;

    private void Start()
    {
        Enums.ECC[] cCs = new Enums.ECC[] { Enums.ECC.None };
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
    public float[] Stats
    {
        get { return stats; }
        set { stats = value; }
    }
    protected abstract Node CreateBehaviorTree();

    //�ǰ� Ÿ��
    #region
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
    // CC
    #region
    public void ApplyCC(Enums.ECC ccType)
    {
        _isCC = true;
        _currentCC = ccType;
        if (ccType != Enums.ECC.None)
        {
            // ToDo: CC�� ���� ���� ó�� �� ���� �ð� ����
            float ccDuration = GetCCDuration(ccType);
            // ccDuration�� ����Ͽ� ������ �����ϰų� �ٸ� ó���� �߰��ϼ���.
        }
    }
    public float GetCCDuration(Enums.ECC ccType)
    {
        // CC ������ ���� ���� �ð� ��ȯ
        switch (ccType)
        {
            case Enums.ECC.Stun:
                return 2f;
            case Enums.ECC.Silence:
                return 1f;
            default:
                return 0f;
        }
    }
    public void RemoveCC()
    {
        _isCC = false;
        _currentCC = Enums.ECC.None;
        // ToDo: CC ������ ���� ���� ó��
    }
    public void PerformCCAction(Enums.ECC ccType)
    {
        // Perform CC action here
        switch (ccType)
        {
            case Enums.ECC.Stun:
                break;
            case Enums.ECC.Silence:
                break;
            default:
                break;
        }
    }
    #endregion
}

public class Skill
{
    public float skiilCounter = 0;

    public virtual void Execute(GameObject summon, GameObject target, Animator animator)
    {
        // Implement the basic behavior of the skill here
    }
}