using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;

public class PoToad : Summon
{
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
    public class Attack : Skill
    {
        private GameObject _projectile;
        private CC cc = new CC();
        private float[] skillStats = { 5f, 1f, 2f };   // 사거리, 쿨타임, 데미지
        private int count;

        public Attack()
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
                count = 0;
                isStart = true;
                FlipSprite(summon, target);

                animator.SetBool("Idle", false);
                animator.SetBool("Move", false);
                animator.SetTrigger("Attack");

                return true;
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (count > 0)
                {
                    isStart = false;
                    return false;
                }

                if (stateInfo.IsName("Attack") && (stateInfo.normalizedTime >= 0.9f) && (count == 0))
                {
                    if (target != null)
                    {
                        return true;
                    }
                }
                return true;
            }
        }
    }
    public class SlimeLick : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 0.8f, 10f, 5f };   // 사거리, 쿨타임, 데미지

        public SlimeLick()
        {
            base.stats = skillStats;

            HasCc = ECC.None;   // 둔화
            float[] ccStats = { 1f, 3f };
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
        private GameObject _projectile;
        private float[] skillStats = { 10f, 25f, 1f };   // 사거리, 쿨타임, 데미지

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
                animator.SetTrigger("Ult1");
                FlipSprite(summon, target);
                isStart = true;
                return true;
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Ult") && stateInfo.normalizedTime >= 0.9f)
                {
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
                //적이 씬 안에 있다면, 행동
                new Sequence(new List<Node>
                {
                    new CheckEnemyInScene(),
                    new Selector(new List<Node>
                    {
                        //궁극기 게이지 찼으면, 궁극기
                        new Sequence(new List<Node>
                        {
                            new CheckUltGage(skills[((int)ESummonAction.Ult)]),
                            new TaskUlt(skills[((int)ESummonAction.Ult)])
                        }),
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(),
                            new DoMoveToEnemy()
                        }),
                        //적이 너무 가까우면, 스킬
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(),
                            new CheckSkill(skills[((int)ESummonAction.Skill)]),
                            new TaskSkill(skills[((int)ESummonAction.Skill)])
                        }),
                        //적이 공격 범위 안에 있다면, 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(),
                            new CheckSkill(skills[((int)ESummonAction.Attack)]),
                            new TaskAttack(skills[((int)ESummonAction.Attack)]),
                        })
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