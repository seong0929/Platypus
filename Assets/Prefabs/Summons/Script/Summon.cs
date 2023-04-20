using System.Collections.Generic;
using UnityEngine;

//�������̵� �Լ� ������ �޶����� Ŭ����
public abstract class Summon : SummonBase
{
    public bool isMoving;
    public bool isAlive = true;

    //ToDo: GameManager���� �� �Ǻ� �ʱ�ȭ
    private bool myteam;
    float deadTime;

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
                Attack(opponent.GetComponent<Summon>(), stats[((int)Enums.ESummonStats.NormalDamage)]);
                isMoving = false;
                return; // ��Ÿ� �̳��� ������ �̵��� ����
            }
            isMoving = true;
            Vector3 direction = (opponent.transform.position - transform.position).normalized;
            transform.position += direction * stats[((int)Enums.ESummonStats.MovementSpeed)] * Time.deltaTime;
        }
    }
    public virtual void Respawne()
    {
        if (Constants.respawntime == GameManager.instance.GameTime - deadTime)
        {
            //ToDo: ���� ��ġ
        }
    }
    
    public abstract void Attack(Summon target, float damage);
    public abstract void UseSkill(int skillIndex, Summon target);   //skillIndex == 0: ��ų, skillIndex == 1: ��

    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(damage - stats[((int)Enums.ESummonStats.Defence)], 0);
        stats[((int)Enums.ESummonStats.Health)] -= actualDamage;
        if (stats[((int)Enums.ESummonStats.Health)] <= 0) {
            deadTime = GameManager.instance.GameTime;
            isAlive = false;
            Destroy(this);
        }
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