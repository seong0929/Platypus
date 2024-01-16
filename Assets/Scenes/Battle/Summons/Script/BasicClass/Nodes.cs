using UnityEngine;
using Skills;
using static Enums;
using System.Collections.Generic;

namespace BehaviorTree
{
    #region 리스폰 관련
    // 리스폰 확인
    public class CheckRespawn : Node
    {
        public override ENodeState Evaluate()
        {
            //Debug.Log("CheckRespawn");
            Summon selfSummon = (Summon)GetData("self");
            ESummonState state = selfSummon.GetSummonState();

            if(state.Equals(ESummonState.Dead))
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
            Debug.Log("Fail");return ENodeState.Failure;
        }
    }
    // 리스폰 하기
    public class DutyRespawn : Node
    {
        private float _respawnSecond = 5f;
        private float _timer = 0;
        public DutyRespawn(float respawnSecond) 
        {
            _respawnSecond = respawnSecond; 
        }
        public override ENodeState Evaluate()
        {
        // log : "DutyRespawn"
        Debug.Log("DutyRespawn");
            
            Summon selfSummon = (Summon)GetData("self");
            if(selfSummon.GetSummonState() == ESummonState.Dead)
            {
                _timer = 0;
                selfSummon.Respawn();
                Debug.Log("Running");return ENodeState.Running;
            }
            if(selfSummon.GetSummonState() == ESummonState.Respawn)
            {
                _timer += Time.deltaTime;
                if(_timer >= _respawnSecond)
                {
                    _timer = 0;
                    Debug.Log("Success"); return ENodeState.Success;
                }
                else
                {
                    Debug.Log("Running");return ENodeState.Running;
                }
            }
            Debug.Log("Fail");return ENodeState.Failure;
        }
    }
    #endregion
    #region 죽음 관련
    //생존 확인
    public class CheckIfAlive : Node
    {
        public CheckIfAlive() { }
        public override ENodeState Evaluate()
        {
            //Debug.Log("CheckIfAlive");    

            Summon selfSummon = (Summon)GetData("self");
            
            if(selfSummon.GetHealth() <= 0 || selfSummon.GetSummonState() == ESummonState.Dead)
            {
                Debug.Log("Fail");return ENodeState.Failure;
            }            
                Debug.Log("Succ");return ENodeState.Success;
        }
    }
    // 죽기
    public class DutyDie : Node
    {
        public DutyDie() { }
        public override ENodeState Evaluate()
        {
            Debug.Log("DutyDie");
            Summon selfSummon = (Summon)GetData("self");
            GameObject self = selfSummon.gameObject;

            self.tag = "NonTarget";
            bool isOver = !selfSummon.Die();
            if(isOver)
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
            else
            {
                Debug.Log("Running");return ENodeState.Running;
            }
        }
    }
    #endregion
    #region CC기 관련
    // CC기 걸렸는 지 확인
    public class CheckHardCC : Node
    {
        public CheckHardCC() { }
        public override ENodeState Evaluate()
        {
            //Debug.Log("CheckHardCC");
            Summon selfSummon = (Summon)GetData("self");
            ESummonState state = selfSummon.GetSummonState();

            if (state == ESummonState.HardCC)
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
            
            Debug.Log("Fail");return ENodeState.Failure;
        }
    }
    // CC 기 행동
    public class DutyHardCC : Node
    {
        public DutyHardCC() { }
        public override ENodeState Evaluate()
        {
            Debug.Log("DutyHardCC");
            Summon selfSummon = (Summon)GetData("self");
            ESummonState state = selfSummon.GetSummonState();

            // 죽었을 시, interrupt
            if(selfSummon.GetSummonState() == ESummonState.Dead)
            {
                Debug.Log("Fail");return ENodeState.Failure;
            }

            GivenSkillContainer ccContainer = selfSummon.GetHardCCContainer();
            if (ccContainer.Stun.Duration <= 0)
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
            else
            {
                Debug.Log("Running");return ENodeState.Running;
            }
        }
    }
    #endregion
    #region 이동
    // 상대방 있는 지 확인
    public class CheckEnemyInScene : Node
    {
        public override ENodeState Evaluate()
        {
            // log : "CheckEnemyInScene"
            //Debug.Log("CheckEnemyInScene");

            Summon selfSummon = (Summon)GetData("self");
            List<Summon> enemies = selfSummon.GetEnemies();

            // 상대방 있는 경우
            foreach (Summon summon in enemies)
            {
                if (summon.GetSummonState() != ESummonState.Dead)
                {
                    SetData("target", summon);
                    Debug.Log("Succ");return ENodeState.Success;
                }
            }
            // 상대방 없는 경우
            ClearData("target");
            Debug.Log("Fail");return ENodeState.Failure;
        }
    }
    // 사거리 밖에 있는 지 확인
    public class CheckEnemyInAttackRange : Node
    {
        private float _attackRange;

        public CheckEnemyInAttackRange(float range) { _attackRange = range; }
        public override ENodeState Evaluate()
        {
            //Debug.Log("CheckEnemyInAttackRange");

            Summon self = (Summon)GetData("self");

            Summon target = (Summon)GetData("target");
            if (target == null)
            {
                Debug.Log("Fail");return ENodeState.Failure;
            }

            float distance = Vector3.Distance(self.transform.position, target.transform.position);
            if (distance <= _attackRange)
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
            Debug.Log("Fail");return ENodeState.Failure;
        }
    }
    //적을 향해 움직이기
    public class DoMoveToEnemy : Node
    {
        public DoMoveToEnemy() { }
        public override ENodeState Evaluate()
        {
            // log : "DoMoveToEnemy"
            Debug.Log("DoMoveToEnemy");

            Summon self = (Summon)GetData("self");
            Summon target = (Summon)GetData("target");
            if (target == null)
            {
                Debug.Log("Fail");return ENodeState.Failure;
            }

            // 상대방 바라보기
            if (target != null)
            {
                self.FlipSpriteTo(target);
                self.MoveTo(target);
            }

            Debug.Log("Succ");return ENodeState.Success;
        }
    }

    //적과 멀어지기
    public class DoMoveAwayFromEnemy : Node
    { 
        public DoMoveAwayFromEnemy() { }
        public override ENodeState Evaluate()
        {
            // log : "DoMoveAwayFromEnemy"
            Debug.Log("DoMoveAwayFromEnemy");

            Summon self = (Summon)GetData("self");
            Summon target = (Summon)GetData("target");

            if (target != null)
            {
                self.FlipSpriteTo(target);

                Vector2 direction = (self.transform.position - target.transform.position).normalized;
                if (self.transform.position == target.transform.position)
                {
                    // random direction
                    direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                }
                self.Move(direction);
            }

            Debug.Log("Succ");return ENodeState.Success;
        }
    }

    // Idle 상태
    public class DoIdle : Node
    {
        public DoIdle() { }
        public override ENodeState Evaluate()
        {
            // log : "DoIdle"
            Debug.Log("DoIdle");

            Summon self = (Summon)GetData("self");
            self.Idle();

            Debug.Log("Succ");return ENodeState.Success;
        }
    }
    #endregion

    #region 스킬
    // 스킬 사용 가능 여부 확인
    public class CheckSkill : Node
    {
        private Skill _skill;

        public CheckSkill(Skill skill)
        {
            _skill = skill;
        }
        public override ENodeState Evaluate()
        {
            // log : "CheckSkill"
            //Debug.Log("CheckSkill : " + _skill.ToString());
            if (_skill.IsSkillCooldown())
            {
                Debug.Log("Fail"); return ENodeState.Failure;
            }
            else
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
        }
    }
    // 스킬 사용하기
    public class TaskSkill : Node
    {
        private Skill _skill;
        private float _timer = 0f;

        private bool _wtf = false;

        public TaskSkill(Skill skill)
        {
            _skill = skill;
        }
        public override ENodeState Evaluate()
        {
            Debug.Log("TaskSkill : " + _skill.ToString());


            Summon self = (Summon)GetData("self");
            // Duty Event시, interrupt
            if(self.CheckCriticalEvent())
            {                
                _skill.StartSkillCooldown();
                _timer = 0f;
                Debug.Log("Fail");return ENodeState.Failure;
            }

            Summon target = (Summon)GetData("target");
            _skill.Execute(self, target);
            Debug.Log("Skill State : "+_skill.State.ToString());

            if (_timer >= _skill.Stats.Duration) 
            { 
                _timer = 0f;
                _skill.StartSkillCooldown();
                Debug.Log("Succ"); return ENodeState.Success; 
            }
            _timer += Time.deltaTime;

            Debug.Log("Running"); return ENodeState.Running;
        }
    }
    #endregion
    #region 특수 노드
    // 원거리 캐릭터 전용 +(밀당 캐릭터)
    public class CheckEnemyTooClose : Node
    {
        private float _personalDistance;

        public CheckEnemyTooClose(float distance) { _personalDistance = distance; }
        public override ENodeState Evaluate()
        {
            // log : "CheckEnemyTooClose"
            //Debug.Log("CheckEnemyTooClose");

            Summon self = (Summon)GetData("self");
            Summon target = (Summon)GetData("target");
            if(target == null)
            {
                Debug.Log("Fail");return ENodeState.Failure;
            }

            if (Vector3.Distance(self.transform.position, target.transform.position) <= _personalDistance)
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
            else 
            {
                Debug.Log("Fail");return ENodeState.Failure;
            }
        }
    }
    // 사거리 내 누군가라도 있는 가
    public class CheckAnyone : Node
    {
        private GameObject _area;

        public CheckAnyone(GameObject area) 
        {
            _area = area; 
        }
        public override ENodeState Evaluate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_area.transform.position, _area.transform.localScale.x);

            if (colliders != null)
            {
                Debug.Log("Succ");return ENodeState.Success;
            }
            else
            {
                Debug.Log("Fail");return ENodeState.Failure;
            }
        }
    }
    #endregion
}