using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;

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

            HasCc = Enums.ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            _projectile = summon.GetComponent<SpitGlider>().Projectile;

            FlipSprite(summon, target);

            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
            if (!IsCooldown())
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

                StartCooldown();
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

            HasCc = Enums.ECC.KnockBack;
            float[] ccStats = { 0.1f, 5f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: ���� ����
        {
            FlipSprite(summon, target);

            if (!IsCooldown())
            {
                animator.SetTrigger("Skill");
                cc.ApplyCC(summon, target, cc.Stats);
                target.GetComponent<Summon>().CurrrentCCStats = cc.Stats;
                StartCooldown();
            }
        }
    }
    public class AerialBombardment : Skill
    {
        private CC cc = new CC();

        public AerialBombardment()
        {
            float[] skillStats = { 10f, 25f, 1f };   // ��Ÿ�, ��Ÿ��, ������
            base.stats = skillStats;
            
            HasCc = Enums.ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: ���� ����
        {
            FlipSprite(summon, target);

            if (!IsCooldown())
            {
                animator.SetTrigger("Ult1");
                StartCooldown();
            }
        }
    }
    private void UpdateSkillCooldowns(float deltaTime)
    {
        foreach (Skill skill in skills)
        {
            skill.UpdateCooldown(deltaTime);
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
                            new CheckEnemyTooClose(this.gameObject),
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
    #endregion
}