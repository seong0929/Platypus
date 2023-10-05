using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;

public class SpitGlider : Summon
{
    public GameObject Projectile;

    #region Settings
    public SpitGlider()
    {
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 5f, 0.5f, 80f, 1f, 0.8f };

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
        UpdateSkillCooldowns(Time.deltaTime);
        CreateBehaviorTree().Evaluate();
    }
    #endregion
    #region Skill
    public class Attack : Skill
    {
        private GameObject _projectile;
        private CC cc = new CC();

        public Attack()
        {
            float[] skillStats = { 5f, 1f, 2f };   // ��Ÿ�, ��Ÿ��, ������
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            _projectile = summon.GetComponent<SpitGlider>().Projectile;

            FlipSprite(summon, target);

            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Attack");

                // ���� ����� ���� ã��
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Summon");
                GameObject nearestEnemy = null;
                float nearestDistance = float.MaxValue;
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Summon>().MyTeam == false && enemy != summon)
                    {
                        float distance = Vector2.Distance(summon.transform.position, enemy.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestEnemy = enemy;
                            nearestDistance = distance;
                        }
                    }
                }
                // �߻�
                if (nearestEnemy != null)
                {
                    Vector3 projectilePosition = summon.transform.position;
                    GameObject projectile = Instantiate(_projectile, projectilePosition, Quaternion.identity, summon.transform);
                    Vector3 direction = (nearestEnemy.transform.position - projectilePosition).normalized;
                    projectile.GetComponent<Rigidbody2D>().velocity = direction * 2; // �ʿ信 ���� �ӵ� ����
                }

                StartSkillCooldown();
            }
        }
    }
    public class SeedSpitting : Skill
    {
        private CC cc = new CC();

        public SeedSpitting()
        {
            float[] skillStats = { 0.8f, 10f, 5f };   // ��Ÿ�, ��Ÿ��, ������
            base.stats = skillStats;

            HasCc = ECC.KnockBack;
            float[] ccStats = { 1f, 30f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: ���� ����
        {
            FlipSprite(summon, target);

            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Skill");
                target.GetComponent<Summon>().CurrentCCStats = cc.Stats;
                target.GetComponent<Summon>().CurrentCC = HasCc;
                cc.ApplyCC(summon, target, cc.Stats);
                StartSkillCooldown();
            }
        }
    }
    public class AerialBombardment : Skill
    {
        private CC cc = new CC();
        private GameObject _projectile;

        public AerialBombardment()
        {
            float[] skillStats = { 10f, 25f, 1f };   // ��Ÿ�, ��Ÿ��, ������
            base.stats = skillStats;
            
            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            FlipSprite(summon, target);
            _projectile = summon.GetComponent<SpitGlider>().Projectile;
            
            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Ult1");
                summon.tag = "NonTarget";

                animator.SetTrigger("Ult2");

                // �̵�
                summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)] *= 0.5f;

                // ���� ����� ���� ã��
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Summon");
                GameObject nearestEnemy = null;
                float nearestDistance = float.MaxValue;
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Summon>().MyTeam == false && enemy != summon)
                    {
                        float distance = Vector2.Distance(summon.transform.position, enemy.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestEnemy = enemy;
                            nearestDistance = distance;
                        }
                    }
                }
                // �߻�
                if (nearestEnemy != null)
                {
                    Vector3 projectilePosition = nearestEnemy.transform.position + new Vector3(0, 10, 0);
                    GameObject projectile = Instantiate(_projectile, projectilePosition, Quaternion.Euler(new Vector3(0f, 0f, 90f)), summon.transform);
                    Vector3 direction = (nearestEnemy.transform.position - projectilePosition).normalized;
                    projectile.GetComponent<Rigidbody2D>().velocity = direction * 2; // �ʿ信 ���� �ӵ� ����
                }

                StartSkillCooldown();
                animator.SetTrigger("Ult3");
                summon.tag = "Summon";
                summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)] *= 2f;
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
                        //�ñر� ������ á����, �ñر�
                        new Sequence(new List<Node>
                        {
                            new CheckUltGage(skills[((int)ESummonAction.Ult)]),
                            new TaskUlt(this.gameObject, skills[((int)ESummonAction.Ult)])
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
                            new CheckEnemyTooClose(this.gameObject),
                            new CheckSkill(skills[((int)ESummonAction.Skill)]),
                            new TaskSkill(this.gameObject, skills[((int)ESummonAction.Skill)])
                        }),
                        //���� ���� ���� �ȿ� �ִٸ�, ����
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(this.gameObject),
                            new TaskAttack(this.gameObject, skills[((int)ESummonAction.Attack)]),
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