using UnityEngine;
using System.Collections;
using static Enums;
using System.Collections.Generic;
using System;

namespace Skills
{
    // 스킬 큰 틀
    public class Skill
    {
        public SkillStats Stats;
        public CCStats CCStats;
        public BuffDebuffStats BuffStats;
        public BuffDebuffStats DebuffStats;

        public ESkillState State;

        protected float _skillCooldown = 0f;
        protected int _skillCounters = 0;

        public void InitSkill(SkillStats skillStats, CCStats ccStats, BuffDebuffStats buffStats, BuffDebuffStats debuffStats)
        {
            Stats = new SkillStats();
            CCStats = new CCStats();
            BuffStats = new BuffDebuffStats();
            DebuffStats = new BuffDebuffStats();

            State = ESkillState.CoolingDown;
        }
        public virtual bool Execute(Summon summon, List<Summon> targets)
        {
            return false;            // 동작 구현
        }
        public virtual bool Execute(Summon summon, Summon target)
        {
            return false;            // 동작 구현
        }

        #region 쿨타임
        public bool IsSkillCooldown()
        {
            return _skillCooldown > 0f;
        }
        public void StartSkillCooldown()
        {
            _skillCooldown = Stats.CoolTime;
            State = ESkillState.CoolingDown;
        }
        public void UpdateSkillCooldown(float deltaTime)
        {
            if (Stats.CoolTime > 0f)
            {
                _skillCooldown -= deltaTime;
            }
            if (_skillCooldown <= 0f)
            {
                if(State != ESkillState.InUse)
                    State = ESkillState.Available;
            }
        }
        public void SetInUse()
        {
            State = ESkillState.InUse;
        }

        public void AddSkillCounter()
        {
            _skillCounters++;
        }
        #endregion

        #region Getters
        public float GetCoolTime()
        {
            return Stats.CoolTime;
        }
        public float GetDamage()
        {
            return Stats.Damage;
        }
        public float GetRange()
        {
            return Stats.Range;
        }
        public float GetDuration()
        {
            return Stats.Duration;
        }
        public float GetHeal()
        {
            return Stats.Heal;
        }
        public float GetCriticalChance()
        {
            return Stats.CriticalChance;
        }
        public float GetCriticalCoefficient()
        {
            return Stats.CriticalCoefficient;
        }
        public float GetSkillCooldown()
        {
            return _skillCooldown;
        }
        public int GetSkillCounters()
        {
            return _skillCounters;
        }

        public void SetCoolTime(float coolTime)
        {
            Stats.CoolTime = coolTime;
        }
        public void SetDamage(float damage)
        {
            Stats.Damage = damage;
        }
        public void SetRange(float range)
        {
            Stats.Range = range;
        }
        public void SetDuration(float duration)
        {
            Stats.Duration = duration;
        }
        public void SetHeal(float heal)
        {
            Stats.Heal = heal;
        }
        public void SetCriticalChance(float criticalChance)
        {
            Stats.CriticalChance = criticalChance;
        }
        public void SetCriticalCoefficient(float criticalCoefficient)
        {
            Stats.CriticalCoefficient = criticalCoefficient;
        }
        #endregion
    }
}