using System.Collections.Generic;
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
        float[] summonStats = { 1f, 0.7f, 800f, 3f };

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
        private CC cc = new CC();
        private float[] skillStats = { -1f, -1f, -1f };   // 사거리, 쿨타임, 데미지

        public Attack()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            return false;
        }
    }
    public class SlimeLick : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 1f, 1f, 10f };   // 사거리, 쿨타임, 데미지

        public SlimeLick()
        {
            base.stats = skillStats;

            HasCc = ECC.SlowDown;
            float[] ccStats = { 2f, 2f };
            cc.Stats = ccStats;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (isStart == false)
            {
                animator.SetTrigger("Skill");
                FlipSprite(summon, target);
                isStart = true;
                return true;
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    target.GetComponent<Summon>().CurrentCCStats = cc.Stats;
                    target.GetComponent<Summon>().CurrentCC = HasCc;
                    cc.ApplyCC(summon, target, cc.Stats);

                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
                    isStart = false;
                    return false;
                }
                return true;
            }
        }
    }
    public class ElixirTorrent : Skill
    {
        private CC cc = new CC();
        private GameObject _area;
        private float[] skillStats = { 5f, 30f, 0f };   // 사거리, 쿨타임, 데미지

        public ElixirTorrent()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (isStart == false)
            {
                animator.SetTrigger("UltIn");
                FlipSprite(summon, target);
                isStart = true;
                return true;
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                _area = summon.GetComponent<PoToad>().Area;
                _area.transform.localScale *= skillStats[0];
                GameObject area = Instantiate(_area, summon.transform.position, Quaternion.identity, summon.transform);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(area.transform.position, skillStats[0]);
                
                // 회복
                // ToDo: 팀 구분, 회복 시간
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Summon"))
                    {
                        Summon buffedTeam = collider.GetComponent<Summon>();
                        if (buffedTeam != null)
                        {
                            buffedTeam.Stats[((int)ESummonStats.Health)] += 2; // 힐량
                        }
                    }
                }
                // 궁극기 끝날 때
                if (stateInfo.IsName("Ult") && stateInfo.normalizedTime >= 0.9f) //ToDo: 유지 시간
                {
                    animator.SetTrigger("UltOut");
                    Destroy(area);
                    isStart = false;
                    return false;
                }
                return true;
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
                    //new CheckAnyne(),
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