using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;

public class PoToad : Summon
{
    public GameObject Area;

    #region Settings
    public PoToad()
    {
        //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
        float[] summonStats = { 1f, 0.3f, 800f, 3f };

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
        base.stats = summonStats;
    }

    private void Awake()
    {
        skills.Add(new Attack());
        skills.Add(new SlimeLick());
        skills.Add(new ElixirTorrent());

        foreach (Skill skill in skills)
        {
            skill.StartSkillCooldown();
        }
        CreateBehaviorTree();
    }
    private void Update()
    {
        UpdateSkillCooldowns(Time.deltaTime);
        _rootNode.Evaluate();
    }
    #endregion
    #region Skill
    // 해당 캐릭터는 일반 공격이 없다
    public class Attack : Skill
    {
        private Buffer _buffer = new Buffer();
        private float[] _skillStats = { -1f, -1f, -1f };   // 사거리, 쿨타임, 데미지

        public Attack()
        {
            base._stats = _skillStats;
            float[] bufferStats = { 0f, 0f };
            _buffer.Stats = bufferStats;
            _buffer.Type = EBufferType.None;
            HasCc = ECC.None;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            return false;
        }
    }
    public class SlimeLick : Skill
    {
        private Buffer _debuffer = new Buffer();
        private float[] _skillStats = { 1f, 1f, 10f };   // 사거리, 쿨타임, 데미지

        public SlimeLick()
        {
            base._stats = _skillStats;
            float[] debufferStats = { 2f, 2f, -1f };
            _debuffer.Stats = debufferStats;
            _debuffer.Type = EBufferType.Debuffer;
            HasCc = ECC.SlowDown;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (_isStart == false)
            {
                animator.SetTrigger("Skill");
                Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Skill"));
                FlipSprite(summon, target);
                _isStart = true;
                return true;
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    Debug.Log("In SlimeLick");
                    target.GetComponent<Summon>().CurrentCCStats = _debuffer.Stats;
                    target.GetComponent<Summon>().CurrentCC = HasCc;
                    _debuffer.ApplyCC(summon, target, _debuffer.Stats);

                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), _skillStats[((int)ESkillStats.Damage)]);
                    _isStart = false;
                    return false;
                }
                return true;
            }
        }
    }
    public class ElixirTorrent : Skill
    {
        private Buffer _buffer = new Buffer();
        private GameObject _area;
        private GameObject _areaPrfab;
        private float[] _skillStats = { 5f, 30f, 0f };   // 사거리, 쿨타임, 데미지
        // 사거리가 Elixir의 크기와 일치해야 함

        public ElixirTorrent()
        {
            base._stats = _skillStats;
            float[] bufferStats = { 5f, 2f, 1f };
            _buffer.Stats = bufferStats;
            _buffer.Type = EBufferType.Buffer;
            HasCc = ECC.None;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (_isStart == false)
            {
                _area = summon.GetComponent<PoToad>().Area;
                animator.SetTrigger("UltIn");
                FlipSprite(summon, target);
                _isStart = true;
                return true;
            }
            else
            {
                _areaPrfab = Instantiate(_area, summon.transform.position, Quaternion.identity, summon.transform);

                // 회복
                TickDown(_areaPrfab, _buffer, _buffer.Stats[((int)EBufferStats.Tick)]);
                
                // 궁극기 끝날 때
                if (IsDone(_buffer.Stats[((int)EBufferStats.Time)]))
                {
                    Debug.Log("End PoToad's Ult");
                    animator.SetTrigger("UltOut");
                    Destroy(_areaPrfab);
                    _isStart = false;
                    return false;
                }
                return true;
            }
        }
    }
    private static IEnumerator TickDown(GameObject area, Buffer buffer, float tick)
    {
        Debug.Log("In TickDoen Fn");
        yield return new WaitForSeconds(tick);
        Debug.Log("In TickDoen Fn next to yield");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(area.transform.position, area.transform.localScale.x);
        // ToDo: 팀 구분
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Summon"))
            {
                GameObject buffedTeam = collider.GetComponent<GameObject>();
                if (buffedTeam != null)
                {
                    buffer.Heling(buffedTeam, buffer.Stats[((int)EBufferStats.Power)]);
                    Debug.Log(buffedTeam + "&" + buffedTeam.GetComponent<Summon>().Stats[((int)ESummonStats.Health)]);
                }
            }
        }
    }
    private void UpdateSkillCooldowns(float deltaTime)
    {
        foreach (Skill skill in skills)
        {
            skill.UpdateSkillCooldown(deltaTime);
        }
    }
    #endregion
    #region BehaviorTree
    private Node _rootNode = null;

    protected override Node CreateBehaviorTree()
    {
        _rootNode = new Selector(new List<Node>
        {
            //행동 결정
            new Selector(new List<Node>{
                // 리스폰
                new Sequence(new List<Node>
                {
                    new CheckRespawn(this.gameObject),
                    new DutyRespawn()
                }),
                // 캐릭터 생존 여부 확인 후, 리스폰
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive()),
                    new DutyDie(),
                }),
                // CC 여부 확인
                new Sequence(new List<Node>
                {
                    new CheckCC(),
                    new DutyCC(),
                }),
                // 궁극기가 준비되었다면
                new Sequence(new List<Node>
                {
                    new CheckAnyone(Area),
                    new CheckUltGage(skills[((int)ESummonAction.Ult)]),
                    new TaskUlt(skills[((int)ESummonAction.Ult)])
                }),
                new Sequence(new List<Node>
                {
                    // 씬 안에 있는 지 확인
                    new CheckEnemyInScene(),
                    new Selector(new List<Node>
                    {
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(),
                            new DoMoveToEnemy()
                        }),
                        //공격 범위 안에 들어 왔다면 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(),
                            new CheckSkill(skills[((int)ESummonAction.Skill)]),
                            new TaskSkill(skills[((int)ESummonAction.Skill)])
                        }),
                    })
                }),
                //적이 씬 안에 없다면, Idle
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckEnemyInScene()),
                    new DoIdle()
                })
            })
        });
        return _rootNode;
    }
    #endregion
}