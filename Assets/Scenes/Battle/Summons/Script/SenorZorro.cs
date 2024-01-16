using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;
using UnityEngine.Rendering;

public class SenorZorro : Summon
{
    [SerializeField]
    Animator _animator;

    private Attack attack = new Attack();
    private FootworkSkill footworkSkill = new FootworkSkill(0.1f);
    private FlecheSkill flecheSkill = new FlecheSkill();
    private float _desirableDistance = 0.1f;

    #region Settings
    public SenorZorro()
    {   
    }
    private void Awake()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }
    }
    private void Start()
    {
        base.SetSummonBasic(ESummon.SenorZorro, _animator);

        // setting up skills
        InitSkills();

        // dictionary skill cooldowns
        foreach( (string key, Skill skill) in _skillDictionary)
        {
            skill.StartSkillCooldown();
        }

        base._root = CreateBehaviorTree();
        UpdateRootNodeData();
        update_for_inspectator();
    }
    private void Update()
    {
        if (_root == null)
        {
            _root = CreateBehaviorTree();
        }
        UpdateRootNodeData();
        UpdateSkillCooldowns(Time.deltaTime);
        //Make_Inspectator_Changes_update_real_value();
        update_for_inspectator();
        _root.Evaluate();
        if(!_isMoving)
        {
            Rigidbody2D.velocity = Vector2.zero;
        }
        UpdateHardCCContainer(Time.deltaTime);
    }

    #endregion
    #region Skill
    public override void InitSkills()
    {
        // setting up skills
        _skillDictionary.Add("attack", attack);
        _skillDictionary.Add("skill", footworkSkill);
        _skillDictionary.Add("ult", flecheSkill);
    }
    public class Attack : Skill
    {
        public SkillStats skillStats = new SkillStats() {
            CoolTime = 7f,
            Damage = 10f,
            Range = 0.5f,
            Duration = 3f,
            Heal = 0f,
            CriticalChance = 0.3f,
            CriticalCoefficient = 1.3f
        };
        public bool IsAnimationEventCalled = false;

        public Attack()
        {
            base.Stats = skillStats;
        }
        public override bool Execute(Summon summon, Summon target)
        {
            summon._isMoving = false;
            if(State == ESkillState.Available)
            {
                Debug.Log("Attack is available");
                IsAnimationEventCalled = false;
                base.SetInUse();
                summon.SetAnimationState("Attack", skillStats.Duration);
                return true;
            }
            if(State == ESkillState.InUse && IsAnimationEventCalled)
            {
                Debug.Log("Attack is called");
                IsAnimationEventCalled = false;
                summon.FlipSpriteTo(target);
                summon.GiveDamage(target, skillStats.Damage);
                StartSkillCooldown();
                return true;
            }
            return false;
        }
    }
    public class FootworkSkill : Skill
    {
        public SkillStats skillStats = new SkillStats()
        {
            CoolTime = 8f,
            Damage = 0f,
            Range = 0.5f,
            Duration = 3f,
            Heal = 0f,
            CriticalChance = 0f,
            CriticalCoefficient = 0f
        };

        public float DesirableDistance = 5f;
        public bool IsAnimationEventCalled = false;

        public FootworkSkill(float desirableDistance = 5f)
        {
            base.Stats = skillStats;
            DesirableDistance = desirableDistance;
        }
        public override bool Execute(Summon summon, Summon target)
        {
            summon._isMoving = false;
            if(base.State == ESkillState.Available)
            {
                IsAnimationEventCalled = false;
                base.SetInUse();
                summon.SetAnimationState("Skill", skillStats.Duration);
                summon.FlipSpriteTo(target);

                return true;
            }
            if(base.State == ESkillState.InUse && IsAnimationEventCalled)
            {
                IsAnimationEventCalled = false;

                Vector2 summonPosition = summon.transform.position;
                Vector2 targetPosition = target.transform.position;
                float distance = Vector2.Distance(summonPosition, targetPosition);
                float movableDistance = skillStats.Range;
                // 멀리 있을 때
                if (distance > DesirableDistance)
                {
                    // 멀지만, 최대 이동시 공격 범위 보다 가까워질 위험이 있는 경우
                    if ((distance - movableDistance) < DesirableDistance)
                    {
                        Vector2 teleportPosition= (summonPosition - targetPosition).normalized * DesirableDistance + targetPosition;
                        summon.Teleport(teleportPosition);
                    }
                    // 멀리 있을 때
                    summon.Teleport(summonPosition + (targetPosition - summonPosition).normalized * movableDistance);
                }
                // 가까이 있을 때
                else
                {
                    summon.Teleport( (summonPosition - targetPosition).normalized * DesirableDistance + targetPosition);
                }

                return true;
            }

            return false;
        }
    }
    public class FlecheSkill : Skill
    {
        public SkillStats skillStats = new SkillStats()
        {
            CoolTime = 20f,
            Damage = 20f,
            Range = 1f,
            Duration = 8f,
            Heal = 0f,
            CriticalChance = 0.4f,
            CriticalCoefficient = 1.4f
        };

        public bool IsUltInAnimationEventCalled = false;

        public FlecheSkill()
        {
            base.Stats = skillStats;
        }
        public override bool Execute(Summon summon, Summon target)
        {
            summon._isMoving = false;

            float DesirableDistance = 5f;
            if (base.State == ESkillState.Available)
            {
                base.SetInUse();
                summon.SetAnimationState("UltIn", skillStats.Duration / 2);
                summon.FlipSpriteTo(target);

                IsUltInAnimationEventCalled = false;

                return true;
            }
            if(base.State == ESkillState.InUse && IsUltInAnimationEventCalled)
            {
                summon.SetAnimationState("UltOut", skillStats.Duration /2);

                float appearDistance = DesirableDistance;
                Vector3 direction = (target.transform.position - summon.transform.position).normalized;
                float distance = Vector3.Distance(summon.transform.position, target.transform.position);
                float teleportDistance = distance - appearDistance;

                Vector3 teleportPosition = summon.transform.position + direction * teleportDistance;
                summon.Teleport(teleportPosition);

                Vector3 appearPosition = target.transform.position + direction * appearDistance;
                summon.Teleport(appearPosition);

                summon.FlipSpriteTo(target);

                List<Summon> targets = new List<Summon>();
                targets.Add(target);

                summon.GiveDamage(targets, skillStats.Damage);

                IsUltInAnimationEventCalled = false;
                return true;
            }
            return false;
        }
    }

    public void CallAttackAnimationEvent()
    {
        Debug.Log("Attack_SenorZorro Animation Event is called");
        attack.IsAnimationEventCalled = true;
    }
    public void CallFootworkAnimationEvent()
    {
        Debug.Log("Footwork_SenorZorro Animation Event is called");
        footworkSkill.IsAnimationEventCalled = true;
    }
    public void CallFlecheAnimatonEvent()
    {
        Debug.Log("Fleche_SenorZorro Animation Event is called");
        flecheSkill.IsUltInAnimationEventCalled = true;
    }
    #endregion
    #region BehaviorTree
    protected override Node CreateBehaviorTree()
    {
//        Node node = new Selector(new List<Node>
        base._root = new Selector(new List<Node>
        {
            //행동 결정
            new Selector(new List<Node>{
                // 리스폰
                new Sequence(new List<Node>
                {
                    new CheckRespawn(),
                    new DutyRespawn(Constants.RESPAWN_TIME)
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
                    new CheckHardCC(),
                    new DutyHardCC(),
                }),
                //적이 씬 안에 있다면, 행동
                new Sequence(new List<Node>
                {
                    new CheckEnemyInScene(),
                    new Selector(new List<Node>
                    {
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new Inverter (new CheckEnemyInAttackRange(attack.Stats.Range)),
                            //스킬이 있다면 스킬 사용, 아니면 이동
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node> //스킬 사용
                                {
                                    new CheckSkill(footworkSkill),
                                    new TaskSkill(footworkSkill)
                                }),
                                new DoMoveToEnemy() // 일반 이동
                            })
                        }),
                        //적이 공격 범위 안에 있다면, 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(attack.Stats.Range),
                            new Inverter(new CheckEnemyTooClose(_desirableDistance)),
                            new Selector(new List<Node>{
                                new Sequence(new List<Node> // 궁국기 사용
                                {
                                    new CheckSkill(flecheSkill),
                                    new TaskSkill(flecheSkill)
                                }),
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(attack),
                                    new TaskSkill(attack), // 일반 공격
                                })
                            })
                        }),
                        //적이 너무 가까우면, 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(_desirableDistance),
                            //스킬이 있다면 스킬 사용, 아니면 이동
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node> // 이동 스킬 사용
                                {
                                    new CheckSkill(footworkSkill),
                                    new TaskSkill(footworkSkill)
                                }),
                                new DoMoveAwayFromEnemy() // 일반 이동으로 멀어짐
                            })
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

        return base._root;
    }

    #endregion
}