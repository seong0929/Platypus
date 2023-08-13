using UnityEngine;

namespace Skills
{
    // 스킬 큰 틀
    public class Skill
    {
        public float skiilCounter = 0;
        protected float[] stats;
        protected float skillCooldown = 0f;

        public virtual void Execute(GameObject summon, GameObject target, Animator animator)
        {
            // 동작 구현
        }
        public float[] Stats
        {
            get { return stats; }
            set { stats = value; }
        }
        #region 쿨타임
        public bool IsCooldown()
        {
            return skillCooldown > 0f;
        }
        public void StartCooldown()
        {
            skillCooldown = stats[(int)Enums.ESkillStats.CoolTime];
        }
        public void UpdateCooldown(float deltaTime)
        {
            if (stats[((int)Enums.ESkillStats.CoolTime)] > 0f)
            {
                skillCooldown -= deltaTime;
                if (skillCooldown <= 0f)
                {
                    skillCooldown = 0f;
                }
            }
        }
        #endregion
        public void FlipSprite(GameObject summon, GameObject target)
        {
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
    // CC 기 큰틀
    public class CC
    {
        private WaitForFixedUpdate _wait;
        private int _knockBackPower = 0;

        private void Awake()
        {
            _wait = new WaitForFixedUpdate();
        }
        public virtual void Execute(GameObject summon, GameObject target)
        {
            // 동작 구현
        }
    }
}
