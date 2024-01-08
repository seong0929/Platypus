using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;


public class SenorZorroBT
{
    private Summon summon;
    private Node _rootNode = null;
    protected List<Skill> skills;
    protected Node CreateBehaviorTree()
    {
        _rootNode = new Selector(new List<Node>
        {
            //행동 결정
            new Selector(new List<Node>{
                // 리스폰
                new Sequence(new List<Node>
                {
                    new CheckRespawn(summon.gameObject),
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
}
