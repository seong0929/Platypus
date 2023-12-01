using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;

public class SenorZorro : Summon
{

    #region make data show on inspector
    public string[] dictionaryArray;
    public string[] currentAnimatorStateArray;
    public Dictionary<string, object> rootNodeData = new Dictionary<string, object>();
    public AnimatorStateInfo currentAnimatorState;

    private void SetInspectatorArray()
    {
        int dicLength = rootNodeData.Count;
        dictionaryArray = new string[dicLength];

        int i = 0;
        foreach(KeyValuePair<string, object> data in rootNodeData)
        {
            dictionaryArray[i] = data.Key.ToString() + " : " + data.Value.ToString();
            i++;
        }

        currentAnimatorStateArray = new string[10];
        currentAnimatorStateArray[0] = "fullPathHash : " + currentAnimatorState.fullPathHash.ToString();
        currentAnimatorStateArray[1] = "length : " + currentAnimatorState.length.ToString();
        currentAnimatorStateArray[2] = "loop : " + currentAnimatorState.loop.ToString();
        currentAnimatorStateArray[3] = "speed : " + currentAnimatorState.speed.ToString();
        currentAnimatorStateArray[4] = "speedMultiplier : " + currentAnimatorState.speedMultiplier.ToString();
        currentAnimatorStateArray[5] = "tagHash : " + currentAnimatorState.tagHash.ToString();
        currentAnimatorStateArray[6] = "normalizedTime : " + currentAnimatorState.normalizedTime.ToString();        
        currentAnimatorStateArray[7] = " Name : " + currentAnimatorState.ToString();
        currentAnimatorStateArray[8] = " IsNameAttack : " + currentAnimatorState.IsName("Attack").ToString();
        currentAnimatorStateArray[9] = " IsNameSkill : " + currentAnimatorState.IsName("Skill").ToString();


    }

    private void UpdateInspectatorData()
    {
        Transform targetT = (Transform)_rootNode.GetData("target");
        base.Target = targetT.gameObject;
        base.State = (ESummonState)_rootNode.GetData("State");
        rootNodeData = _rootNode.GetAllData();
        Debug.Log("rootNodeData: " + rootNodeData);
        currentAnimatorState = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        Debug.Log("currentAnimatorState: " + currentAnimatorState);
        SetInspectatorArray();

    }
    #endregion

    #region Settings
    public SenorZorro()
    {
        //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
        float[] summonStats = { 0.7f, 1f, 40f, 5f};

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
//        base.stats = summonStats;
        base.BaseStats = summonStats;
    }
    private void Awake()
    {
        skills.Add(new Attack());
        skills.Add(new FootworkSkill());
        skills.Add(new FlecheSkill());


        CreateBehaviorTree();
        _rootNode.SetData("State", ESummonState.Respawn);

        foreach (Skill skill in skills)
        {
            skill.StartSkillCooldown();
        }
    }
    private void Update()
    {
        UpdateSkillCooldowns(Time.deltaTime);
        if (_rootNode != null) 
        {
            _rootNode.Evaluate();
            //UpdateInspectatorData();
        }
    }
    #endregion
    #region Skill
    public class Attack: Skill
    {
        private Buffer buffer = new Buffer();
        private float[] skillStats = { 0.8f, 1f, 2f };   // 사거리, 쿨타임, 데미지

        public Attack()
        {
            base._stats = skillStats;
            HasCc = ECC.None;
            float[] bufferStats = { -1f, -1f, -1f };
            buffer.Stats = bufferStats;
            buffer.Type = EBufferType.None;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if(_isStart == false)
            {
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

                if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 0.9f)
                {
                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
                    _isStart = false;
                    return false;
                }
                return true;
            }
            
        }
    }
    public class FootworkSkill : Skill
    {
        private Buffer buffer = new Buffer();
        private float[] skillStats = { 0.8f, 7f, 0f };   // 사거리, 쿨타임, 데미지
        private bool isMoved = false;
        private float movableDistance = 0f;

        public FootworkSkill()
        {
            base._stats = skillStats;
            float[] bufferStats = { -1f, -1f, -1f };
            buffer.Stats = bufferStats;
            buffer.Type = EBufferType.None;
            HasCc = ECC.None;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if(_isStart == false)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Move", false);
                animator.SetTrigger("Skill");

                _isStart = true;
                isMoved = false;
                movableDistance = summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)] * 2;
                return true;
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                FlipSprite(summon, target);
                if(isMoved && stateInfo.IsName("Skill") && stateInfo.normalizedTime >= 0.9f)
                {
                    isMoved = false;
                    _isStart = false;
                    return false;
                }
                if (stateInfo.IsName("Skill") && stateInfo.normalizedTime >= 0.4f && isMoved == false)
                {
                    Vector2 summonPosition = summon.transform.position;
                    Vector2 targetPosition = target.transform.position;
                    float distance = Vector2.Distance(summonPosition, targetPosition);
                    float attackRagne = summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)];

                    // 멀리 있을 때
                    if (distance > attackRagne)
                    {
                        // 멀지만, 최대 이동시 공격 범위 보다 가까워질 위험이 있는 경우
                        if ((distance - movableDistance) < attackRagne)
                        {
                            summon.transform.position = (summonPosition- targetPosition).normalized * attackRagne + targetPosition;
                        }

                        // 멀리 있을 때
                        summon.transform.position = summonPosition + (targetPosition - summonPosition).normalized * movableDistance;
                    }
                    // 가까이 있을 때
                    else
                    {
                        summon.transform.position = (summonPosition - targetPosition).normalized * attackRagne + targetPosition;
                    }
                    
                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
                    isMoved = true;
                    return true;
                }
                return true;
            }
        }
    }
    public class FlecheSkill : Skill
    {
        private Buffer buffer = new Buffer();
        private float[] skillStats = { 20f, 19f, 10f };   // 사거리, 쿨타임, 데미지

        public FlecheSkill()
        {
            base._stats = skillStats;
            float[] bufferStats = { -1f, -1f, -1f };
            buffer.Stats = bufferStats;
            buffer.Type = EBufferType.None;
            HasCc = ECC.None;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {            
            if (_isStart == false)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Move", false);
                animator.SetTrigger("UltIn");
                _isStart = true;
                return true;
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.IsName("UltIn") && stateInfo.normalizedTime >= 0.9f)
                {
                    animator.SetTrigger("UltOut");
                    _isStart = false;

                    float appearDistance = summon.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)];
                    Vector3 direction = (target.transform.position - summon.transform.position).normalized;
                    float distance = Vector3.Distance(summon.transform.position, target.transform.position);
                    float teleportDistance = distance - appearDistance;

                    Vector3 teleportPosition = summon.transform.position + direction * teleportDistance;
                    summon.transform.position = teleportPosition;

                    Vector3 appearPosition = target.transform.position + direction * appearDistance;
                    summon.transform.position = appearPosition;
                    FlipSprite(summon, target);

                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
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
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(),
                            //스킬이 있다면 스킬 사용, 아니면 이동
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node> //스킬 사용
                                {
                                    new CheckSkill(skills[((int)ESummonAction.Skill)]),
                                    new TaskSkill(skills[((int)ESummonAction.Skill)])
                                }),
                                new DoMoveToEnemy() // 일반 이동
                            })
                        }),
                        //적이 공격 범위 안에 있다면, 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(),
                            new Inverter(new CheckEnemyTooClose()),
                            new Selector(new List<Node>{
                                new Sequence(new List<Node> // 궁국기 사용
                                {
                                    new CheckUltGage(skills[((int)ESummonAction.Ult)]),
                                    new TaskUlt(skills[((int)ESummonAction.Ult)])
                                }),
                                new Sequence(new List<Node>
                                {
                                    new CheckSkill(skills[((int)ESummonAction.Attack)]),
                                    new TaskAttack(skills[((int)ESummonAction.Attack)]), // 일반 공격
                                })
                            })
                        }),
                        //적이 너무 가까우면, 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(),
                            //스킬이 있다면 스킬 사용, 아니면 이동
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node> // 이동 스킬 사용
                                {
                                    new CheckSkill(skills[((int)ESummonAction.Skill)]),
                                    new TaskSkill(skills[((int)ESummonAction.Skill)])
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
        return _rootNode;
    }
    #endregion
}