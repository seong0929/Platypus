using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpitGlider : Summon
{
    #region Settings
    public SpitGlider()
    {
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 5f, 0.5f, 80f, 8f, 2f, 0.3f, 5f, 15f };

        //Summon Ŭ������ �����ڸ� ȣ���ϸ鼭 �ʱ�ȭ�� ���� ����
        base.stats = summonStats;
    }

    private void Awake()
    {
        skills.Add(new Attack());
        skills.Add(new SeedSpitting());
        skills.Add(new AerialBombardment());
    }
    private void Update()
    {
        CreateBehaviorTree().Evaluate();
    }
    #endregion
    #region Skill
    public class Attack : Skill
    {
        public Attack()
        {
            float[] skillStats = { 5f, 1f, 2f };   // ��Ÿ�, ��Ÿ��, ������
            base.stats = skillStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (summon.transform.position.x < target.transform.position.x)
            {
                summon.transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                summon.transform.GetComponent<SpriteRenderer>().flipX = false;
            }
            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
            animator.SetBool("Attack", true);
            //ToDo: �߻�ü ����
        }
    }
    public class SeedSpitting : Skill
    {
        public SeedSpitting()
        {
            float[] skillStats = { 0.8f, 10f, 5f };   // ��Ÿ�, ��Ÿ��, ������
            base.stats = skillStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: ���� ����
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
    public class AerialBombardment : Skill
    {
        public AerialBombardment()
        {
            float[] skillStats = { 10f, 25f, 1f };   // ��Ÿ�, ��Ÿ��, ������
            base.stats = skillStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: ���� ����
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
    #endregion
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
                        //�ñر� ������ á����, �ñر�
                        new Sequence(new List<Node>
                        {
                            new CheckUltGage(skills[((int)Enums.ESummonAction.Ult)]),
                            new TaskUlt(this.gameObject, skills[((int)Enums.ESummonAction.Ult)])
                        }),
                        //���� �ָ� �ִٸ�, ������ �̵�
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(this.gameObject),
                            new TaskMoveToEnemy(this.gameObject)
                        }),
                        //���� �ʹ� ������, ��ų
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(this.gameObject,stats[((int)Enums.ESummonStats.PersonalDistance)]),
                            new CheckSkill(skills[((int)Enums.ESummonAction.Skill)]),
                            new TaskSkill(this.gameObject, skills[((int)Enums.ESummonAction.Skill)])
                        }),
                        //���� ���� ���� �ȿ� �ִٸ�, ����
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(this.gameObject),
                            new TaskAttack(this.gameObject, skills[((int)Enums.ESummonAction.Attack)]),
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
}