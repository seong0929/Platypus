using UnityEngine;

namespace Skills
{
    // 스킬 큰 틀
    public class Skill
    {
        public float SkiilCounter = 0;
        public Enums.ECC HasCc;
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
        #region CC 세팅
        private float[] stats;

        public void ApplyCC(GameObject summon, GameObject target, float[] stats)
        {
            switch (summon.GetComponent<Summon>().CurrentCC)
            {
                case Enums.ECC.Stun:
                    break;
                case Enums.ECC.KnockBack:
                    KnockBack(summon, target, stats[((int)Enums.ECCStats.Power)]);
                    break;
                case Enums.ECC.None:
                default:
                    FinishedCC(summon);
                    break;
            }
        }
        public void FinishedCC(GameObject summon)
        {
            summon.GetComponent<Summon>().CurrentCC = Enums.ECC.None;
        }
        public float[] Stats
        {
            get { return stats; }
            set { stats = value; }
        }
        #endregion
        #region CC기 종류
        private void KnockBack(GameObject summon, GameObject target, float power) 
        {
            summon.GetComponent<Summon>().CurrentCC = Enums.ECC.KnockBack;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            Vector2 dirVec = target.transform.position - summon.transform.position;
            rb.AddForce(dirVec.normalized * power, ForceMode2D.Impulse);
        }
        private void Stun(GameObject summon, GameObject target, float timer)
        {

        }
        #endregion
    }
}