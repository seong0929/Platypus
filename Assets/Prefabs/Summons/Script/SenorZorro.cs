using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class SenorZorro : Summon
{
    [SerializeField] Animator animator;  //애니메이션

    public SenorZorro()
    {
        //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
        float[] summonStats = { 0.8f, 1f, 150f, 8f, 2f };

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
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
        GiveDamage(target,stats[((int)Enums.ESummonStats.NormalDamage)]);
    }
    public override void Die()
    {
        //죽음 행동
        animator.SetBool("Die", true);
        //ToDo: 팀 리스트 필요
        /*죽은 캐릭터에 대한 정보를 모든 캐릭터들에게 전달
        foreach (var character in characters)
        {
            character.OnCharacterDeath(deadCharacter);
        }
         */
        /* 다른 캐릭터들에서 이 캐릭터를 타겟팅에서 제외
        foreach (var otherCharacter in otherCharacters)
        {
            otherCharacter.RemoveTarget(this);
        }*/
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
                    new TaskDie(),
                    new TaskWait(Constants.respawntime),
                    new TaskRespawn()
                }),
                //적이 씬 안에 있다면, 행동
                new Sequence(new List<Node>
                {
                    new CheckEnemyInScene(IsEnemy()),
                    new Selector(new List<Node>
                    {
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfRange(),
                            //스킬이 있다면 스킬 사용, 아니면 이동
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
                        //적이 공격 범위 안에 있다면, 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(),
                            new CheckEnemyNotToClose(),
                            new TaskAttack()
                        }),
                        //적이 너무 가까우면, 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(),
                            //스킬이 있다면 스킬 사용, 아니면 이동
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
                //적이 씬 안에 없다면, Idle
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckEnemyInScene(IsEnemy())),
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