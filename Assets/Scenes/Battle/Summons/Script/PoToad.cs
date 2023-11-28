using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Skills;
using static Enums;

public class PoToad : Summon
{
    public GameObject Area;

    #region Settings
    public PoToad()
    {
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 1f, 0.7f, 800f, 3f };

        //Summon Ŭ������ �����ڸ� ȣ���ϸ鼭 �ʱ�ȭ�� ���� ����
        base.stats = summonStats;
    }

    private void Awake()
    {
        skills.Add(new Attack());
        skills.Add(new SlimeLick());
        skills.Add(new ElixirTorrent());

        foreach (Skill skill in skills)
        {
            skill.StartSkillCooldown();
        }
        CreateBehaviorTree();
    }
    private void Update()
    {
        UpdateSkillCooldowns(Time.deltaTime);
        _rootNode.Evaluate();
    }
    #endregion
    #region Skill
    // �ش� ĳ���ʹ� �Ϲ� ������ ����
    public class Attack : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { -1f, -1f, -1f };   // ��Ÿ�, ��Ÿ��, ������

        public Attack()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            return false;
        }
    }
    public class SlimeLick : Skill
    {
        private CC cc = new CC();
        private float[] skillStats = { 1f, 1f, 10f };   // ��Ÿ�, ��Ÿ��, ������

        public SlimeLick()
        {
            base.stats = skillStats;

            HasCc = ECC.SlowDown;
            float[] ccStats = { 2f, 2f };
            cc.Stats = ccStats;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (isStart == false)
            {
                animator.SetTrigger("Skill");
                FlipSprite(summon, target);
                isStart = true;
                return true;
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    target.GetComponent<Summon>().CurrentCCStats = cc.Stats;
                    target.GetComponent<Summon>().CurrentCC = HasCc;
                    cc.ApplyCC(summon, target, cc.Stats);

                    summon.GetComponent<Summon>().GiveDamage(target.GetComponent<Summon>(), skillStats[((int)ESkillStats.Damage)]);
                    isStart = false;
                    return false;
                }
                return true;
            }
        }
    }
    public class ElixirTorrent : Skill
    {
        private CC cc = new CC();
        private GameObject _area;
        private float[] skillStats = { 5f, 30f, 0f };   // ��Ÿ�, ��Ÿ��, ������

        public ElixirTorrent()
        {
            base.stats = skillStats;

            HasCc = ECC.None;
            float[] ccStats = { 0f, 0f };
            cc.Stats = ccStats;
        }
        public override bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            if (isStart == false)
            {
                animator.SetTrigger("UltIn");
                FlipSprite(summon, target);
                isStart = true;
                return true;
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                _area = summon.GetComponent<PoToad>().Area;
                _area.transform.localScale *= skillStats[0];
                GameObject area = Instantiate(_area, summon.transform.position, Quaternion.identity, summon.transform);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(area.transform.position, skillStats[0]);
                
                // ȸ��
                // ToDo: �� ����, ȸ�� �ð�
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Summon"))
                    {
                        Summon buffedTeam = collider.GetComponent<Summon>();
                        if (buffedTeam != null)
                        {
                            buffedTeam.Stats[((int)ESummonStats.Health)] += 2; // ����
                        }
                    }
                }
                // �ñر� ���� ��
                if (stateInfo.IsName("Ult") && stateInfo.normalizedTime >= 0.9f) //ToDo: ���� �ð�
                {
                    animator.SetTrigger("UltOut");
                    Destroy(area);
                    isStart = false;
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
            //�ൿ ����
            new Selector(new List<Node>{
                // ������
                new Sequence(new List<Node>
                {
                    new CheckRespawn(this.gameObject),
                    new DutyRespawn()
                }),
                // ĳ���� ���� ���� Ȯ�� ��, ������
                new Sequence(new List<Node>
                {
                    new Inverter(new CheckIfAlive()),
                    new DutyDie(),
                }),
                // CC ���� Ȯ��
                new Sequence(new List<Node>
                {
                    new CheckCC(),
                    new DutyCC(),
                }),
                // �ñرⰡ �غ�Ǿ��ٸ�
                new Sequence(new List<Node>
                {
                    //new CheckAnyne(),
                    new CheckUltGage(skills[((int)ESummonAction.Ult)]),
                    new TaskUlt(skills[((int)ESummonAction.Ult)])
                }),
                new Sequence(new List<Node>
                {
                    // �� �ȿ� �ִ� �� Ȯ��
                    new CheckEnemyInScene(),
                    new Selector(new List<Node>
                    {
                        //���� �ָ� �ִٸ�, ������ �̵�
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyOutOfAttackRange(),
                            new DoMoveToEnemy()
                        }),
                        //���� ���� �ȿ� ��� �Դٸ� ����
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyInAttackRange(),
                            new CheckSkill(skills[((int)ESummonAction.Skill)]),
                            new TaskSkill(skills[((int)ESummonAction.Skill)])
                        }),
                    })
                }),
                //���� �� �ȿ� ���ٸ�, Idle
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