using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SenorZorro : Summon
{
    [SerializeField] Animator _animator;  //�ִϸ��̼�

    public SenorZorro()
    {
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 0.8f, 1f, 150f, 8f, 2f, 0.3f, 5f, 15f };

        //Summon Ŭ������ �����ڸ� ȣ���ϸ鼭 �ʱ�ȭ�� ���� ����
        base.stats = summonStats;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        skills.Add(new FootworkSkill());
        skills.Add(new FlecheSkill());
    }
    private void Update()
    {
        CreateBehaviorTree().Evaluate();
    }
    public override void Attack(Summon target, float damage)
    {
        GiveDamage(target,stats[((int)Enums.ESummonStats.NormalDamage)]);
    }
    public class FootworkSkill : Skill
    {
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

            if (summon.transform.position.x < target.transform.position.x)
            {
                summon.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                summon.GetComponent<SpriteRenderer>().flipX = false;
            }
            animator.SetTrigger("Skill");

            if (distance > summon.GetComponent<Summon>().Stats[((int)Enums.ESummonStats.AttackRange)])
            {
                summon.transform.Translate(moveDirection * summon.GetComponent<Summon>().Stats[((int)Enums.ESummonStats.MoveSpeed)] * Time.deltaTime);
            }
            else
            {
                summon.transform.Translate(moveDirection * summon.GetComponent<Summon>().Stats[((int)Enums.ESummonStats.MoveSpeed)] * Time.deltaTime);
            }
        }
    }
    public class FlecheSkill : Skill
    {
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            float appearDistance = summon.GetComponent<Summon>().Stats[((int)Enums.ESummonStats.AttackRange)];

            animator.SetTrigger("UltIn");

            Vector3 direction = (target.transform.position - summon.transform.position).normalized;
            float distance = Vector3.Distance(summon.transform.position, target.transform.position);
            float teleportDistance = distance - appearDistance;

            Vector3 teleportPosition = summon.transform.position + direction * teleportDistance;
            summon.transform.position = teleportPosition;

            animator.SetTrigger("UltOut");

            Vector3 appearPosition = target.transform.position + direction * appearDistance;
            summon.transform.position = appearPosition;
            if (summon.transform.position.x < target.transform.position.x)
            {
                summon.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                summon.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
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
                    new TaskRespawn(this.transform)
                }),
                // ĳ���� ���� ���� Ȯ�� ��, ������
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive(IsDead())),
                    new TaskDie(this.transform),
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
                            new CheckEnemyOutOfAttackRange(this.transform, stats[((int)Enums.ESummonStats.AttackRange)]),
                            //��ų�� �ִٸ� ��ų ���, �ƴϸ� �̵�
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(skills[((int)Enums.ESummonAction.Skill)], stats[((int)Enums.ESummonStats.CoolTime)]),
                                    new TaskSkill(this.transform, skills[((int)Enums.ESummonAction.Skill)])
                                }),
                                new TaskMoveToEnemy(this.transform, stats[((int)Enums.ESummonStats.MoveSpeed)])
                            })

                        }),
                        //���� ���� ���� �ȿ� �ִٸ�, ����
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(this.transform , this.stats[((int)Enums.ESummonStats.AttackRange)]),
                            new Inverter(new CheckEnemyTooClose(this.transform,stats[((int)Enums.ESummonStats.PersonalDistance)])),
                            new Selector(new List<Node>{
                                new Sequence(new List<Node>
                                {
                                    new CheckUltGage(skills[((int)Enums.ESummonAction.Ult)], stats[((int)Enums.ESummonStats.UltGauge)]),
                                    new TaskUlt(this.transform, skills[1])
                                }),
                                new TaskAttack(this.transform)
                            })
                        }),
                        //���� �ʹ� ������, �̵�
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(this.transform,stats[((int)Enums.ESummonStats.PersonalDistance)]),
                            //��ų�� �ִٸ� ��ų ���, �ƴϸ� �̵�
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(skills[((int)Enums.ESummonAction.Skill)], stats[((int)Enums.ESummonStats.CoolTime)]),
                                    new TaskSkill(this.transform, skills[((int)Enums.ESummonAction.Skill)])
                                }),
                                new TaskMoveToEnemy(this.transform, stats[((int)Enums.ESummonStats.MoveSpeed)])
                            })

                        })
                    })
                }),
                //���� �� �ȿ� ���ٸ�, Idle
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckEnemyInScene()),
                    new TaskIdle(this.transform)
                })
            })
        });
        return root;
    }
}