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
        float[] summonStats = { 5f, 0.5f, 80f, 1f, 0.8f };

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
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
            float[] skillStats = { 5f, 1f, 2f };   // 사거리, 쿨타임, 데미지
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

        public SeedSpitting()
        {
            float[] skillStats = { 0.8f, 10f, 5f };   // 사거리, 쿨타임, 데미지
            base.stats = skillStats;

            HasCc = ECC.KnockBack;
            float[] ccStats = { 1f, 30f };
            cc.Stats = ccStats;
        }
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: 내용 변경
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
            float[] skillStats = { 10f, 25f, 1f };   // 사거리, 쿨타임, 데미지
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
            //행동 결정
            new Selector(new List<Node>{
                // 리스폰
                new Sequence(new List<Node>
                {
                    new CheckRespawn(this.gameObject),
                    new TaskRespawn(this.gameObject)
                }),
                // 캐릭터 생존 여부 확인 후, 리스폰
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive(this.gameObject)),
                    new TaskDie(this.gameObject),
                }),
                // CC 여부 확인
                new Sequence(new List<Node>
                {
                    new CheckCC(this.gameObject),
                    new TaskCC(this.gameObject),
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
                            new TaskUlt(this.gameObject, skills[((int)ESummonAction.Ult)])
                        }),
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(this.gameObject),
                            new TaskMoveToEnemy(this.gameObject)
                        }),
                        //적이 너무 가까우면, 스킬
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(this.gameObject),
                            new CheckSkill(skills[((int)ESummonAction.Skill)]),
                            new TaskSkill(this.gameObject, skills[((int)ESummonAction.Skill)])
                        }),
                        //적이 공격 범위 안에 있다면, 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(this.gameObject),
                            new TaskAttack(this.gameObject, skills[((int)ESummonAction.Attack)]),
                        })
                    })
                }),
                //적이 씬 안에 없다면, Idle
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