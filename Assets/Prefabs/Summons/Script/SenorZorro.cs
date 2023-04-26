using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        TakeDamage(stats[((int)Enums.ESummonStats.NormalDamage)]);
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