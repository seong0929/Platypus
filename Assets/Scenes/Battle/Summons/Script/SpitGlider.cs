using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SpitGlider : Summon
{
    [SerializeField] Animator _animator;  //애니메이션
    #region Settings
    public SpitGlider()
    {
        //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
        float[] summonStats = { 0.8f, 1f, 150f, 8f, 2f, 0.3f, 5f, 15f };

        //Summon 클래스의 생성자를 호출하면서 초기화된 값을 전달
        base.stats = summonStats;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        }
    }
    public class SeedSpitting : Skill
    {
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: 내용 변경
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
        public override void Execute(GameObject summon, GameObject target, Animator animator)   //ToDo: 내용 변경
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
            //행동 결정
            new Selector(new List<Node>{
                // 리스폰
                new Sequence(new List<Node>
                {
                    new CheckRespawn(this.gameObject),
                    new TaskRespawn(this.transform)
                }),
                // 캐릭터 생존 여부 확인 후, 리스폰
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive(IsDead())),
                    new TaskDie(this.transform),
                }),
                // CC 여부 확인
                new Sequence(new List<Node>
                {
                    new CheckCC(this.transform),
                    new TaskCC(this.GetComponent<Summon>()),
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
                            new CheckUltGage(skills[((int)Enums.ESummonAction.Ult)], stats[((int)Enums.ESummonStats.UltGauge)]),
                            new TaskUlt(this.transform, skills[((int)Enums.ESummonAction.Ult)])
                        }),
                        //적이 멀리 있다면, 가까이 이동
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(this.transform, stats[((int)Enums.ESummonStats.AttackRange)]),
                            new TaskMoveToEnemy(this.transform, stats[((int)Enums.ESummonStats.MoveSpeed)])
                        }),
                        //적이 너무 가까우면, 스킬
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyTooClose(this.transform,stats[((int)Enums.ESummonStats.PersonalDistance)]),
                            new CheckSkill(skills[((int)Enums.ESummonAction.Skill)], stats[((int)Enums.ESummonStats.CoolTime)]),
                            new TaskSkill(this.transform, skills[((int)Enums.ESummonAction.Skill)])
                        }),
                        //적이 공격 범위 안에 있다면, 공격
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(this.transform , this.stats[((int)Enums.ESummonStats.AttackRange)]),
                            new TaskAttack(this.transform, skills[((int)Enums.ESummonAction.Attack)]),
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
}