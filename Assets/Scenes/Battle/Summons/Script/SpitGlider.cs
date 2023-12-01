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
        float[] summonStats = { 5f, 0.5f, 40f, 1f};

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
        base.BaseStats = summonStats;

    }

    private void Awake()
    {
        skills.Add(new Attack());
        skills.Add(new AirStrike());
        skills.Add(new AerialBombardment());
        
        foreach (Skill skill in skills)
        {
            skill.StartSkillCooldown();
        }
        CreateBehaviorTree();
        _rootNode.SetData("State", ESummonState.Respawn);
    }
    private void Update()
    {
        UpdateSkillCooldowns(Time.deltaTime);
        _rootNode.Evaluate();
        //Transform targetT = (Transform)_rootNode.GetData("target");
        //base.Target = targetT.gameObject;

    }
    #endregion
    #region Skill
    public class Attack : Skill
    {
        private GameObject _projectile;
        private Buffer _buffer = new Buffer();
        private float[] _skillStats = { 5f, 1f, 2f };   // 사거리, 쿨타임, 데미지
        private int _count;

        public Attack()
        {
            base._stats = _skillStats;
            float[] bufferStats = { -1f, -1f, -1f };
            _buffer.Stats = bufferStats;
            _buffer.Type = EBufferType.None;
            HasCc = ECC.None;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (_isStart == false)
            {
                _count = 0;
                _isStart = true;
                FlipSprite(summon, target);

                animator.SetBool("Idle", false);
                animator.SetBool("Move", false);
                animator.SetTrigger("Attack");

                return true;
            }
            else 
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if(_count > 0)
                {
                    _isStart = false;
                    return false;
                }
                
                if (stateInfo.IsName("Attack") && (stateInfo.normalizedTime >= 0.9f) && (_count==0))
                {
                    // 발사
                    if (target != null)
                    {
                        _projectile = summon.GetComponent<SpitGlider>().Projectile;
                        _projectile.GetComponent<Projectile>().damageAmount = _skillStats[((int)ESkillStats.Damage)];
                        Vector3 projectilePosition = summon.transform.position;
                        GameObject projectile = Instantiate(_projectile, projectilePosition, Quaternion.identity, summon.transform);
                        Vector3 direction = (target.transform.position - projectilePosition).normalized;
                        projectile.GetComponent<Rigidbody2D>().velocity = direction * 2; // 필요에 따라 속도 조정

                        _count = 1;
                        return true;
                    }
                }
                return true;
            }
        }
    }
    public class AirStrike : Skill
    {
        private Buffer _cc = new Buffer();
        private float[] _skillStats = { 0.8f, 10f, 5f };   // 사거리, 쿨타임, 데미지

        public AirStrike()
        {
            base._stats = _skillStats;
            float[] ccStats = { 1f, 3f, -1f };
            _cc.Stats = ccStats;
            _cc.Type = EBufferType.CC;
            HasCc = ECC.KnockBack;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (_isStart == false)
            {
                animator.SetTrigger("Skill");
                FlipSprite(summon, target);
                _isStart = true; 
                return true;
            }
            else
            {                
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    target.GetComponent<Summon>().CurrentCCStats = _cc.Stats;
                    target.GetComponent<Summon>().CurrentCC = HasCc;
                    _cc.ApplyCC(summon, target, _cc.Stats);

                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), _skillStats[((int)ESkillStats.Damage)]);
                    _isStart = false;
                    return false;
                }
                return true;
            }
        }
    }
    public class AerialBombardment : Skill
    {
        private Buffer _buffer = new Buffer();
        private GameObject _projectile;
        private float[] _skillStats = { 10f, 25f, 1f };   // 사거리, 쿨타임, 데미지

        public AerialBombardment()
        {
            base._stats = _skillStats;
            float[] bufferStats = { -1f, -1f, -1f };
            _buffer.Stats = bufferStats;
            _buffer.Type = EBufferType.None;
            HasCc = ECC.None;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (_isStart == false)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Move", false);
                animator.SetTrigger("Ult1");
                FlipSprite(summon, target);
                _isStart = true;
                return true;
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Ult1") && stateInfo.normalizedTime >= 1f)
                {
                    _projectile = summon.GetComponent<SpitGlider>().Projectile;
                    _projectile.GetComponent<Projectile>().damageAmount = _skillStats[((int)ESkillStats.Damage)];

                    if (stateInfo.normalizedTime >= 1f)
                    {
                        summon.tag = "NonTarget";
                    }

                    animator.SetTrigger("Ult2");
                    if (stateInfo.IsName("Ult2"))
                    {
                        // 이동
                        summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)] *= 0.5f;

                        // 가장 가까운 적을 찾기
                        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Summon");
                        //GameObject nearestEnemy = null;
                        //float nearestDistance = float.MaxValue;
                        //foreach (GameObject enemy in enemies)
                        //{
                        //    if (enemy.GetComponent<Summon>().MyTeam == false && enemy != summon)
                        //    {
                        //        float distance = Vector2.Distance(summon.transform.position, enemy.transform.position);
                        //        if (distance < nearestDistance)
                        //        {
                        //            nearestEnemy = enemy;
                        //            nearestDistance = distance;
                        //        }
                        //    }
                        //}
                        // 발사
//                        if (nearestEnemy != null)
                        if (target != null)
                        {
                            Vector3 projectilePosition = target.transform.position + new Vector3(0, 10, 0);
                            GameObject projectile = Instantiate(_projectile, projectilePosition, Quaternion.Euler(new Vector3(0f, 0f, 90f)), summon.transform);
                            Vector3 direction = (target.transform.position - projectilePosition).normalized;
                            projectile.GetComponent<Rigidbody2D>().velocity = direction * 2; // 필요에 따라 속도 조정
                        }
                    }

                    animator.SetTrigger("Ult3");
                    if (stateInfo.IsName("Ult3") && stateInfo.normalizedTime >= 1f)
                    {
                        summon.tag = "Summon";
                        summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)] *= 2f;
                        _isStart = false;
                    }
                    return false;
                }
                return true;
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
                    new DutyRespawn()
                }),
                // 캐릭터 생존 여부 확인 후, 리스폰
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive()),
                    new DutyDie(),
                }),
                // CC 여부 확인
                new Sequence(new List<Node>
                {
                    new CheckCC(),
                    new DutyCC(),
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
                            new DoMoveToEnemy()
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
                            new CheckSkill(skills[((int)ESummonAction.Attack)]),
                            new TaskAttack(skills[((int)ESummonAction.Attack)]),
                        })
                    })
                }),
                //적이 씬 안에 없다면, Idle
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckEnemyInScene()),
                    new DoIdle()
                })
            })
        });
        return _rootNode;
    }
    #endregion
}