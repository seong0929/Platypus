using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class SenorZorro : Summon
{
    [SerializeField] Animator animator;  //애니메이션

    public SenorZorro()
    {
        //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
        float[] summonStats = { 0.8f, 1f, 150f, 8f, 2f, 0.3f, 5f, 100f };

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
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
            //행동 결정
            new Selector(new List<Node>{
                //캐릭터 생존 여부 확인 후, 리스폰
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive(IsDead())),
                    new TaskDie(this.transform),
                    new TaskWait(),
                    new TaskRespawn(this.transform)
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
                            new CheckEnemyOutOfAttackRange(this.transform, stats[((int)Enums.ESummonStats.AttackRange)]),
                            //스킬이 있다면 스킬 사용, 아니면 이동
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
                        //적이 공격 범위 안에 있다면, 공격
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
                        //적이 너무 가까우면, 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(this.transform,stats[((int)Enums.ESummonStats.PersonalDistance)]),
                            //스킬이 있다면 스킬 사용, 아니면 이동
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
                //적이 씬 안에 없다면, Idle
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
            // Implement the Footwork skill, 애니메이션 추가
        }
    }

    public class FlecheSkill : Skill
    {
        public override void Execute(GameObject target)
        {
            // Implement the Fleche skill, 애니메이션 추가
        }
    }
}