using System.Collections.Generic;
using UnityEngine;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : SummonBase
{
    public bool isMoving;

    //ToDo: GameManager���� �� �Ǻ� �ʱ�ȭ
    private bool myteam;

    protected float[] stats;  //�ӽ� ���� ��Ÿ�, �̵��ӵ�, ü��, ������, ����
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
                return; // ��Ÿ� �̳��� ������ �̵��� ����
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