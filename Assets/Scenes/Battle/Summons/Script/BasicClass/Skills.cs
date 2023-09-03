using UnityEngine;
using System.Collections;

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
            switch (target.GetComponent<Summon>().CurrentCC)
            {
                case Enums.ECC.Stun:
                    Stun(target, stats[((int)Enums.ECCStats.Time)]);
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
            target.GetComponent<Summon>().CurrentCC = Enums.ECC.KnockBack;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            Vector2 dirVec = (target.transform.position - summon.transform.position).normalized;
            rb.AddForce(dirVec * power, ForceMode2D.Impulse);
        }
        private void Stun(GameObject target, float duration)
        {
            target.GetComponent<Summon>().CurrentCC = Enums.ECC.Stun;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            target.GetComponent<MonoBehaviour>().StartCoroutine(ReleaseStun(target, duration));
        }
        private IEnumerator ReleaseStun(GameObject target, float duration)
        {
            yield return new WaitForSeconds(duration);

            // Stun 상태를 해제하고 움직일 수 있도록 변경
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

            target.GetComponent<Summon>().CurrentCC = Enums.ECC.None;
        }
        #endregion
    }
}