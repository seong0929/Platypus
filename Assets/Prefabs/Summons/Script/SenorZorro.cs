using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class SenorZorro : Summon
{
    [SerializeField] Animator animator;  //�ִϸ��̼�

    public SenorZorro()
    {
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 0.8f, 1f, 150f, 8f, 2f };

        //Summon Ŭ������ �����ڸ� ȣ���ϸ鼭 �ʱ�ȭ�� ���� ����
        base.stats = summonStats;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        skills.Add(new FootworkSkill());
        skills.Add(new FlecheSkill());
    }

    public override void MoveForOpponent()
    {
        base.MoveForOpponent();
        animator.SetBool("Move", base.isMoving);
    }


    public override void UseSkill(int skillIndex, Summon target)
    {
        if (skillIndex >= 0 && skillIndex < skills.Count)
        {
            skills[skillIndex].Execute(this, target);
        }
    }
    public override void Attack(Summon target, float damage)
    {
        animator.SetTrigger("Attack");
        TakeDamage(stats[((int)Enums.ESummonStats.NormalDamage)]);
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
                    new Inverter(new CheckIfAlive(isAlive)),
                    new TaskDie(),
                    new TaskWait(),
                    new TaskRespawn()
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
                            new CheckEnemyOutOfRange(),
                            //��ų�� �ִٸ� ��ų ���, �ƴϸ� �̵�
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(),
                                    new TaskSkill()
                                }),
                                new TaskMoveToEnemy()
                            })
                            
                        }),
                        //���� ���� ���� �ȿ� �ִٸ�, ����
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(),
                            new CheckEnemyNotToClose(),
                            new TaskAttack()
                        }),
                        //���� �ʹ� ������, �̵�
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(),
                            //��ų�� �ִٸ� ��ų ���, �ƴϸ� �̵�
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(),
                                    new TaskSkill()
                                }),
                                new TaskMoveToEnemy()
                            })

                        })
                    })
                }),
                //���� �� �ȿ� ���ٸ�, Idle
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckEnemyInScene()),
                    new TaskIdle()
                })
            })
        });

        return root;
    }
    public class FootworkSkill : Skill
    {
        public override void Execute(Summon user, Summon target)
        {
            // Implement the Footwork skill
        }
    }

    public class FlecheSkill : Skill
    {
        public override void Execute(Summon user, Summon target)
        {
            // Implement the Fleche skill
        }
    }
}