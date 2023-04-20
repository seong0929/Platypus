using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SenorZorro : Summon
{
    [SerializeField] Animator animator;  //�ִϸ��̼�

    public SenorZorro()
    {
        //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
        float[] summonStats = { 0.8f, 0.2f, 150f, 8f, 2f };

        //Summon Ŭ������ �����ڸ� ȣ���ϸ鼭 �ʱ�ȭ�� ���� ����
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