using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//공통적이되 함수 내용이 달라지는 클래스
public abstract class Summon : SummonBase
{
    public bool isMoving;
    public bool isAlive = true;

    //ToDo: GameManager에서 팀 판별 초기화
    public List<GameObject> myTeam;
    public List<GameObject> theirTeam;
    float deadTime;

    protected float[] stats;  //임시 스탯 사거리, 이동속도, 체력, 데미지, 방어력
    protected List<Skill> skills = new List<Skill>();

    private void Update()
    {
        opponent = SearchOpponent();
        MoveForOpponent();
    }
    public virtual bool IsEnemy() {
        if (theirTeam.Count != 0){ return true;}
        else { return false; }
    }
    public virtual bool IsDead()
    {
        if(stats[((int)Enums.ESummonStats.Health)] <= 0)
        {
            isAlive = false;
            return isAlive;
        }
        isAlive = true;
        return isAlive;
    }
    public virtual void MoveForOpponent()
    {
        if (opponent != null)
        {
            float distance = Vector2.Distance(transform.position, opponent.transform.position);
            if (transform.position.x < opponent.transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            if (distance <= base.circleCollider.radius)
            {
                Attack(opponent.GetComponent<Summon>(), stats[((int)Enums.ESummonStats.NormalDamage)]);
                isMoving = false;
                return; // 사거리 이내에 있으면 이동을 멈춤
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
            //ToDo: 생성 위치
        }
    }
    public abstract void Die();
    public abstract void Attack(Summon target, float damage);
    public abstract void UseSkill(int skillIndex, Summon target);   //skillIndex == 0: 스킬, skillIndex == 1: 궁

    // BehaviorTree에서 사용할 메서드
    protected abstract Node CreateBehaviorTree();

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