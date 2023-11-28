using UnityEngine;
using System.Collections;
using static Enums;

namespace Skills
{
    // 스킬 큰 틀
    public class Skill
    {
        #region Settings
        public float SkiilCounter = 0;
        public ECC HasCc;
        protected float[] _stats;
        protected float _skillCooldown = 0f;
        protected bool _isStart = false;
        private bool _done = false;

        public virtual bool Execute(GameObject summon, GameObject target, Animator animator)
        {
            return false;            // 동작 구현
        }
        public float[] Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }
        public void FlipSprite(GameObject summon, GameObject target)
        {
            if (summon.transform.position.x < target.transform.position.x)
            {
                summon.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                summon.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        #endregion
        #region 쿨타임
        public bool IsSkillCooldown()
        {
            return _skillCooldown > 0f;
        }
        public void StartSkillCooldown()
        {
            _skillCooldown = _stats[(int)ESkillStats.CoolTime];
        }
        public void UpdateSkillCooldown(float deltaTime)
        {
            if (_stats[((int)ESkillStats.CoolTime)] > 0f)
            {
                _skillCooldown -= deltaTime;
                if (_skillCooldown <= 0f)
                {
                    _skillCooldown = 0f;
                }
            }
        }
        public bool IsDone(float duration)
        {
            Debug.Log("In IsDone fn");
            CheckDone(duration);
            return _done;
        }
        private IEnumerator CheckDone(float duration)
        {
            Debug.Log("In CheckDone fn");
            yield return new WaitForSeconds(duration);
            Debug.Log("In CheckDone fn next");
            _done = true;
        }
        #endregion
    }
    // 버퍼 디버퍼 큰틀
    public class Buffer
    {
        #region 공통
        private float[] _stats;
        private float _bufferCooldown;
        private EBufferType _type;

        public float[] Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }
        public EBufferType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion
        #region 쿨타임
        public bool IsBufferCooldown(float time)
        {
            return _bufferCooldown >= time;
        }
        public void ResetBufferCooldown()
        {
            _bufferCooldown = 0;
        }
        public void UpdateBufferCooldown(float deltaTime)
        {
            _bufferCooldown += deltaTime;
        }
        #endregion
        #region 버퍼 세팅
        public void Heling(GameObject target, float heal)
        {
            target.GetComponent<Summon>().Stats[((int)ESummonStats.Health)] += heal;
        }
        #endregion
        #region CC 세팅
        public void ApplyCC(GameObject summon, GameObject target, float[] stats)
        {
            // summon이 시전자, target이 cc 걸린 타겟, 그 외 필요 수치
            switch (target.GetComponent<Summon>().CurrentCC)
            {
                case ECC.Stun:
                    Stun(target, stats[((int)EBufferStats.Time)]);
                    break;
                case ECC.KnockBack:
                    KnockBack(summon, target, stats[((int)EBufferStats.Power)]);
                    break;
                case ECC.SlowDown:
                    SlowDown(target, stats[((int)EBufferStats.Power)], stats[((int)EBufferStats.Time)]);
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
        #endregion
        #region CC기 종류
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
        private void KnockBack(GameObject summon, GameObject target, float power) 
        {
            target.GetComponent<Summon>().CurrentCC = ECC.KnockBack;

            target.transform.position = (target.transform.position - summon.transform.position).normalized * power + target.transform.position;
        }
        private void SlowDown(GameObject target, float power, float duration)
        {
            target.GetComponent<Summon>().CurrentCC = ECC.SlowDown;
            target.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)] -= power;
            Debug.Log("SlowDown" + target + ":" + target.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)]);
            target.GetComponent<MonoBehaviour>().StartCoroutine(ReleaseSlowDown(target, power, duration));
        }
        private IEnumerator ReleaseSlowDown(GameObject target, float power, float duration)
        {
            Debug.Log("In ReleaseSlowDown fn");
            yield return new WaitForSeconds(duration);
            target.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)] += power;
            target.GetComponent<Summon>().CurrentCC = ECC.None;
            Debug.Log("SlowUp" + target + ":" + target.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)]);
        }
        #endregion
    }
}