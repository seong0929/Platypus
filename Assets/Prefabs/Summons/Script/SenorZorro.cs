using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class SenorZorro : Summon
{
    [SerializeField] Animator animator;  //�ִϸ��̼�

    public SenorZorro()
    {
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 0.8f, 1f, 150f, 8f, 2f, 0.3f, 5f, 100f };

        //Summon Ŭ������ �����ڸ� ȣ���ϸ鼭 �ʱ�ȭ�� ���� ����
        base.stats = summonStats;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        skills.Add(new FootworkSkill());
        skills.Add(new FlecheSkill());
    }
    public override void Attack(Summon target, float damage)
    {
        GiveDamage(target,stats[((int)Enums.ESummonStats.NormalDamage)]);
    }
    protected override Node CreateBehaviorTree()
    {
        Node root = new Selector(new List<Node>
        {
            //�ൿ ����
            new Selector(new List<Node>{
                //ĳ���� ���� ���� Ȯ�� ��, ������
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive(IsDead())),
                    new TaskDie(this.transform),
                    new TaskWait(),
                    new TaskRespawn(this.transform)
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
                                    new CheckSkill(skills[0], stats[((int)Enums.ESummonStats.CoolTime)]),
                                    new TaskSkill(skills[0])
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
                                    new CheckUltGage(skills[1], stats[((int)Enums.ESummonStats.UltGauge)]),
                                    new TaskUlt(skills[1])
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
                                    new CheckSkill(skills[0], stats[((int)Enums.ESummonStats.CoolTime)]),
                                    new TaskSkill(skills[0])
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
    public class FootworkSkill : Skill
    {
        public override void Execute(GameObject target)
        {
            // Implement the Footwork skill, �ִϸ��̼� �߰�
        }
    }

    public class FlecheSkill : Skill
    {
        public override void Execute(GameObject target)
        {
            // Implement the Fleche skill, �ִϸ��̼� �߰�
        }
    }
}