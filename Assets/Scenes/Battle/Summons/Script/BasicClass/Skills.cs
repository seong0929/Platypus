using UnityEngine;
using System.Collections;
using static Enums;

namespace Skills
{
    // 스킬 큰 틀
    public class Skill
    {
        public float SkiilCounter = 0;
        public ECC HasCc;
        protected float[] stats;
        protected float skillCooldown = 0f;
        protected bool isStart = false;

        public virtual bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            return false;            // 동작 구현
        }
        public float[] Stats
        {
            get { return stats; }
            set { stats = value; }
        }
        #region 쿨타임
        public bool IsSkillCooldown()
        {
            return skillCooldown > 0f;
        }
        public void StartSkillCooldown()
        {
            skillCooldown = stats[(int)ESkillStats.CoolTime];
        }
        public void UpdateSkillCooldown(float deltaTime)
        {
            if (stats[((int)ESkillStats.CoolTime)] > 0f)
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
                summon.transform.localScale = new Vector3(-1, 1, 1);
                //summon.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                summon.transform.localScale = new Vector3(1, 1, 1);
                //summon.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
    // CC 기 큰틀
    public class CC
    {
        #region CC 세팅
        private float[] stats;
        protected float ccCooldown;

        public void ApplyCC(GameObject summon, GameObject target, float[] stats)
        {
            // summon이 시전자, target이 cc 걸린 타겟, 그 외 필요 수치
            switch (target.GetComponent<Summon>().CurrentCC)
            {
                case ECC.Stun:
                    Stun(target, stats[((int)ECCStats.Time)]);
                    break;
                case ECC.KnockBack:
                    KnockBack(summon, target, stats[((int)ECCStats.Power)]);
                    break;
                case ECC.None:
                default:
                    FinishedCC(summon);
                    break;
            }
        }
        public void FinishedCC(GameObject summon)
        {
            summon.GetComponent<Summon>().CurrentCC = ECC.None;
        }
        public float[] Stats
        {
            get { return stats; }
            set { stats = value; }
        }
        #endregion
        #region 쿨타임
        public bool IsCcCooldown(float time)
        {
            return ccCooldown >= time;
        }
        public void ResetCcCooldown()
        {
            ccCooldown = 0;
        }
        public void UpdateCcCooldown(float deltaTime)
        {
            ccCooldown += deltaTime;
        }
        #endregion
        #region CC기 종류
        private void KnockBack(GameObject summon, GameObject target, float power) 
        {
            target.GetComponent<Summon>().CurrentCC = ECC.KnockBack;

            target.transform.position = (target.transform.position - summon.transform.position).normalized * power + target.transform.position;
        }
        private void Stun(GameObject target, float duration)
        {
            target.GetComponent<Summon>().CurrentCC = ECC.Stun;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            target.GetComponent<MonoBehaviour>().StartCoroutine(ReleaseStun(target, duration));
        }
        private IEnumerator ReleaseStun(GameObject target, float duration)
        {
            yield return new WaitForSeconds(duration);

            // Stun 상태를 해제하고 움직일 수 있도록 변경
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

            target.GetComponent<Summon>().CurrentCC = ECC.None;
        }
        #endregion
    }
}