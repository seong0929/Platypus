using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : MonoBehaviour
{
    public string SummonName;
    public GameObject Opponent;
    public bool IsCC => _isCC;
    public Enums.ECC CurrentCC => _currentCC;
    public bool MyTeam;     //ToDo: BattleManager에서 팀 판별 초기화

    protected float[] stats;  //임시 스탯: 사거리, 이동속도, 체력, 데미지, 방어력
    protected List<Skill> skills = new List<Skill>();   //skillIndex == 0: 일반 공격, 1: 스킬, 2: 궁
    protected List<Enums.ECC> ccs = new List<Enums.ECC>();

    private bool _isAlive = true;
    private bool _isCC = false;
    private Enums.ECC _currentCC;
    private float _deadTime;

    #region settings
    public float[] Stats
    {
        get { return stats; }
        set { stats = value; }
    }
    private void Start()
    {
        Enums.ECC[] ccs = new Enums.ECC[] { Enums.ECC.None };
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
    public void ApplyCC(Enums.ECC ccType)
    {
        _isCC = true;
        _currentCC = ccType;
        if (ccType != Enums.ECC.None)
        {
            // ToDo: CC에 따른 동작 처리 및 지속 시간 설정
            float ccDuration = GetCCDuration(ccType);
            // ccDuration을 사용하여 동작을 수행하거나 다른 처리를 추가
        }
    }
    public float GetCCDuration(Enums.ECC ccType)
    {
        // CC 유형에 따른 지속 시간 반환
        switch (ccType)
        {
            case Enums.ECC.Stun:
                return 2f;
            case Enums.ECC.KnockBack:
                return 1f;
            default:
                return 0f;
        }
    }
    public void RemoveCC()
    {
        _isCC = false;
        _currentCC = Enums.ECC.None;
        // ToDo: CC 해제에 따른 동작 처리
    }
    public void PerformCCAction(Enums.ECC ccType)
    {
        switch (ccType)
        {
            case Enums.ECC.Stun:
                break;
            case Enums.ECC.KnockBack:
                //StartCoroutine(KnockBack());
                break;
            default:
                break;
        }
    }
    //private IEnumerator KnockBack()
    //{
       // yield return mWait;
       // Vector3 playerPos = GameManager.instance.Player.transform.position;
       // Vector3 dirVec = transform.position - playerPos;
       // mRb.AddForce(dirVec.normalized * (3 + mKnockbackpower), ForceMode2D.Impulse);
    // }
    #endregion
}