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
        //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
        float[] summonStats = { 5f, 0.5f, 1000f, 1f, 0.8f };

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
        base.stats = summonStats;
    }

    private void Awake()
    {
        skills.Add(new Attack());
        skills.Add(new SeedSpitting());
        skills.Add(new AerialBombardment());
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

        public Attack()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            _projectile = summon.GetComponent<SpitGlider>().Projectile;
            _projectile.GetComponent<Projectile>().damageAmount = skillStats[((int)ESkillStats.Damage)];

            FlipSprite(summon, target);

            animator.SetBool("Idle", false);
            animator.SetBool("Move", false);
            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Attack");

                // 가장 가까운 적을 찾기
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
                // 발사
                if (nearestEnemy != null)
                {
                    Vector3 projectilePosition = summon.transform.position;
                    GameObject projectile = Instantiate(_projectile, projectilePosition, Quaternion.identity, summon.transform);
                    Vector3 direction = (nearestEnemy.transform.position - projectilePosition).normalized;
                    projectile.GetComponent<Rigidbody2D>().velocity = direction * 2; // 필요에 따라 속도 조정
                }

                StartSkillCooldown();
            }
        }
    }
    public class SeedSpitting : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 0.8f, 10f, 5f };   // 사거리, 쿨타임, 데미지

        public SeedSpitting()
        {
            base.stats = skillStats;

            HasCc = ECC.KnockBack;
            float[] ccStats = { 1f, 30f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            FlipSprite(summon, target);

            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Skill");
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    target.GetComponent<Summon>().CurrentCCStats = cc.Stats;
                    target.GetComponent<Summon>().CurrentCC = HasCc;
                    cc.ApplyCC(summon, target, cc.Stats);
                    StartSkillCooldown();

                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
                }
            }
        }
    }
    public class AerialBombardment : Skill
    {
        private CC cc = new CC();
        private GameObject _projectile;
        private float[] skillStats = { 10f, 25f, 1f };   // 사거리, 쿨타임, 데미지

        public AerialBombardment()
        {
            base.stats = skillStats;
            
            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)
        {
            FlipSprite(summon, target);
            _projectile = summon.GetComponent<SpitGlider>().Projectile;
            _projectile.GetComponent<Projectile>().damageAmount = skillStats[((int)ESkillStats.Damage)];

            if (!IsSkillCooldown())
            {
                animator.SetTrigger("Ult1");
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    summon.tag = "NonTarget";
                }

                animator.SetTrigger("Ult2");
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f)
                {
                    // 이동
                    summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)] *= 0.5f;

                    // 가장 가까운 적을 찾기
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
                    // 발사
                    if (nearestEnemy != null)
                    {
                        Vector3 projectilePosition = nearestEnemy.transform.position + new Vector3(0, 10, 0);
                        GameObject projectile = Instantiate(_projectile, projectilePosition, Quaternion.Euler(new Vector3(0f, 0f, 90f)), summon.transform);
                        Vector3 direction = (nearestEnemy.transform.position - projectilePosition).normalized;
                        projectile.GetComponent<Rigidbody2D>().velocity = direction * 2; // 필요에 따라 속도 조정
                    }
                }
                StartSkillCooldown();

                animator.SetTrigger("Ult3");
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    summon.tag = "Summon";
                    summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)] *= 2f;
                }
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
                            new TaskMoveToEnemy()
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
                            new TaskAttack(skills[((int)ESummonAction.Attack)]),
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
        return _rootNode;
    }
    #endregion
}