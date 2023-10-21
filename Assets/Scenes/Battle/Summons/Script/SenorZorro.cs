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
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 0.7f, 1f, 150f, 5f, 0.5f };

        //Summon Ŭ������ �����ڸ� ȣ���ϸ鼭 �ʱ�ȭ�� ���� ����
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
        private float[] skillStats = { 0.8f, 1f, 2f };   // ��Ÿ�, ��Ÿ��, ������

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
            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Attack");
                StartSkillCooldown();
            }

            summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
        }
    }
    public class FootworkSkill : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 0.8f, 5f, 0f };   // ��Ÿ�, ��Ÿ��, ������

        public FootworkSkill()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
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
                // ToDo: ���� �ʿ�(������ �ڷ� �������� ����)
                if (distance > summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)])
                {
                    summon.transform.Translate(moveDirection * summon.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)] * Time.deltaTime);
                }
                else
                {
                    summon.transform.Translate(moveDirection * summon.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)] * Time.deltaTime);
                }
                StartSkillCooldown();
            }

            summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
        }
    }
    public class FlecheSkill : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 20f, 20f, 10f };   // ��Ÿ�, ��Ÿ��, ������

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
                animator.SetTrigger("UltIn");

                Vector3 direction = (target.transform.position - summon.transform.position).normalized;
                float distance = Vector3.Distance(summon.transform.position, target.transform.position);
                float teleportDistance = distance - appearDistance;

                Vector3 teleportPosition = summon.transform.position + direction * teleportDistance;
                summon.transform.position = teleportPosition;

                animator.SetTrigger("UltOut");

                Vector3 appearPosition = target.transform.position + direction * appearDistance;
                summon.transform.position = appearPosition;
                FlipSprite(summon, target);
                StartSkillCooldown();

                summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
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
            //�ൿ ����
            new Selector(new List<Node>{
                // ������
                new Sequence(new List<Node>
                {
                    new CheckRespawn(this.gameObject),
                    new TaskRespawn(this.gameObject)
                }),
                // ĳ���� ���� ���� Ȯ�� ��, ������
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive(this.gameObject)),
                    new TaskDie(this.gameObject),
                }),
                // CC ���� Ȯ��
                new Sequence(new List<Node>
                {
                    new CheckCC(this.gameObject),
                    new TaskCC(this.gameObject),
                }),
                //���� �� �ȿ� �ִٸ�, �ൿ
                new Sequence(new List<Node>
                {
                    new CheckEnemyInScene(),
                    new Selector(new List<Node>
                    {
                        //���� �ָ� �ִٸ�, ������ �̵�
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(this.gameObject),
                            //��ų�� �ִٸ� ��ų ���, �ƴϸ� �̵�
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(skills[((int)ESummonAction.Skill)]),
                                    new TaskSkill(this.gameObject, skills[((int)ESummonAction.Skill)])
                                }),
                                new TaskMoveToEnemy(this.gameObject)
                            })
                        }),
                        //���� ���� ���� �ȿ� �ִٸ�, ����
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(this.gameObject),
                            new Inverter(new CheckEnemyTooClose(this.gameObject)),
                            new Selector(new List<Node>{
                                new Sequence(new List<Node>
                                {
                                    new CheckUltGage(skills[((int)ESummonAction.Ult)]),
                                    new TaskUlt(this.gameObject, skills[((int)ESummonAction.Ult)])
                                }),
                                new TaskAttack(this.gameObject, skills[((int)ESummonAction.Attack)]),
                            })
                        }),
                        //���� �ʹ� ������, �̵�
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(this.gameObject),
                            //��ų�� �ִٸ� ��ų ���, �ƴϸ� �̵�
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(skills[((int)ESummonAction.Skill)]),
                                    new TaskSkill(this.gameObject, skills[((int)ESummonAction.Skill)])
                                }),
                                new TaskMoveToEnemy(this.gameObject)
                            })
                        })
                    })
                }),
                //���� �� �ȿ� ���ٸ�, Idle
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckEnemyInScene()),
                    new TaskIdle(this.gameObject)
                })
            })
        });
        return root;
    }
    #endregion
}