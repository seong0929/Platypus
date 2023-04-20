using System.Collections.Generic;
using UnityEngine;

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : SummonBase
{
    public bool isMoving;

    //ToDo: GameManager에서 팀 판별 초기화
    private bool myteam;

    protected float[] stats;  //임시 스탯 사거리, 이동속도, 체력, 데미지, 방어력
    protected List<Skill> skills = new List<Skill>();

    private void Update()
    {
        opponent = SearchOpponent();
        MoveForOpponent();
    }

    public virtual void MoveForOpponent()
    {
        if (opponent != null)
        {
            float distance = Vector2.Distance(transform.position, opponent.transform.position);

            if (distance <= base.circleCollider.radius)
            {
                isMoving = false;
                return; // 사거리 이내에 있으면 이동을 멈춤
            }
            isMoving = true;
            Vector3 direction = (opponent.transform.position - transform.position).normalized;
            transform.position += direction * stats[((int)Enums.ESummonStats.SummonSpeed)] * Time.deltaTime;
        }
    }
    public abstract void UseSkill(int skillIndex, Summon target);

    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(damage - stats[((int)Enums.ESummonStats.Defence)], 0);
        stats[((int)Enums.ESummonStats.Health)] -= actualDamage;
    }

    public void GiveDamage(Summon target, float damage)
    {
        target.TakeDamage(damage);
    }
}
public class Skill
{
    public string name;
    public int cooldown;

    public virtual void Execute(Summon user, Summon target)
    {
        // Implement the basic behavior of the skill here
    }
}