using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;

public class SenorZorro : Summon
{
    #region Settings
    public SenorZorro()
    {
        //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
        float[] summonStats = { 0.7f, 1f, 1500f, 5f, 0.5f };

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
        base.stats = summonStats;
    }

    private void Awake()
    {
        skills.Add(new Attack());
        skills.Add(new FootworkSkill());
        skills.Add(new FlecheSkill());
    }
    private void Update()
    {
        UpdateSkillCooldowns(Time.deltaTime);
        CreateBehaviorTree().Evaluate();
    }
    #endregion
    #region Skill
    public class Attack: Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 0.8f, 2f, 2f };   // 사거리, 쿨타임, 데미지

        public Attack()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            FlipSprite(summon, target);

            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Attack");
                if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 0.9f)
                {
                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
                }
                StartSkillCooldown();
            }
        }
    }
    public class FootworkSkill : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 0.8f, 7f, 0f };   // 사거리, 쿨타임, 데미지

        public FootworkSkill()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            Rigidbody2D rb = summon.GetComponent<Rigidbody2D>();
            Vector2 summonPosition = summon.transform.position;
            Vector2 targetPosition = target.transform.position;
            float distance = Vector2.Distance(summonPosition, targetPosition);

            Vector2 moveDirection;
            if (summonPosition.x < targetPosition.x)
            {
                moveDirection = Vector2.right;
            }
            else
            {
                moveDirection = Vector2.left;
            }

            FlipSprite(summon, target);

            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Skill");
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                Debug.Log(stateInfo.IsName("Skill"));
                if (stateInfo.IsName("Skill") && stateInfo.normalizedTime >= 0.25f)
                {
                    Debug.Log(stateInfo.IsName("Skill"));

                    if (distance > summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)])
                    {
                        rb.AddForce(moveDirection * summon.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)] * 2, ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(moveDirection * summon.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)] * 2, ForceMode2D.Impulse);
                    }
                }

                StartSkillCooldown();
            }

            summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
        }
    }
    public class FlecheSkill : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 20f, 19f, 10f };   // 사거리, 쿨타임, 데미지

        public FlecheSkill()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            float appearDistance = summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)];
            if (!IsSkillCooldown())
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                animator.SetTrigger("UltIn");

                if (stateInfo.normalizedTime >= 0.01f)
                {
                    Vector3 direction = (target.transform.position - summon.transform.position).normalized;
                    float distance = Vector3.Distance(summon.transform.position, target.transform.position);
                    float teleportDistance = distance - appearDistance;

                    Vector3 teleportPosition = summon.transform.position + direction * teleportDistance;
                    summon.transform.position = teleportPosition;

                    Vector3 appearPosition = target.transform.position + direction * appearDistance;
                    summon.transform.position = appearPosition;
                    FlipSprite(summon, target);

                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
                }
                animator.SetTrigger("UltOut");

                StartSkillCooldown();
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
    protected override Node CreateBehaviorTree()
    {
        Node root = new Selector(new List<Node>
        {
            //행동 결정
            new Selector(new List<Node>{
                // 리스폰
                new Sequence(new List<Node>
                {
                    new CheckRespawn(this.gameObject),
                    new TaskRespawn()
                }),
                // 캐릭터 생존 여부 확인 후, 리스폰
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive()),
                    new TaskDie(),
                }),
                // CC 여부 확인
                new Sequence(new List<Node>
                {
                    new CheckCC(),
                    new TaskCC(),
                }),
                //적이 씬 안에 있다면, 행동
                new Sequence(new List<Node>
                {
                    new CheckEnemyInScene(),
                    new Selector(new List<Node>
                    {
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(),
                            //스킬이 있다면 스킬 사용, 아니면 이동
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(skills[((int)ESummonAction.Skill)]),
                                    new TaskSkill(skills[((int)ESummonAction.Skill)])
                                }),
                                new TaskMoveToEnemy()
                            })
                        }),
                        //적이 공격 범위 안에 있다면, 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(),
                            new Inverter(new CheckEnemyTooClose()),
                            new Selector(new List<Node>{
                                new Sequence(new List<Node>
                                {
                                    new CheckUltGage(skills[((int)ESummonAction.Ult)]),
                                    new TaskUlt(skills[((int)ESummonAction.Ult)])
                                }),
                                new TaskAttack(skills[((int)ESummonAction.Attack)]),
                            })
                        }),
                        //적이 너무 가까우면, 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(),
                            //스킬이 있다면 스킬 사용, 아니면 이동
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(skills[((int)ESummonAction.Skill)]),
                                    new TaskSkill(skills[((int)ESummonAction.Skill)])
                                }),
                                new TaskMoveToEnemy()
                            })
                        })
                    })
                }),
                //적이 씬 안에 없다면, Idle
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckEnemyInScene()),
                    new TaskIdle()
                })
            })
        });
        return root;
    }
    #endregion
}