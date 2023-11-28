using UnityEngine;
using Skills;
using static Enums;

namespace BehaviorTree
{
    #region 리스폰 관련
    // 리스폰 확인
    public class CheckRespawn : Node
    {
        private GameObject _gameObject;

        public CheckRespawn(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        public override ENodeState Evaluate()
        {
            object s = GetData("State");
            if (s == null)
            {
                SetData("State", ESummonState.Default);
                SetData("Self", _gameObject);
                return ENodeState.Failure;
            }
            if (GetData("State").Equals(ESummonState.Respawn))
            {                   
                return ENodeState.Success;
            }
            return ENodeState.Failure;
        }
    }
    // 리스폰 하기
    public class DutyRespawn : Node
    {
        private Animator _animator;
        private GameObject _summon;

        public DutyRespawn() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            if(self == null)
            {
               Debug.Log("self is null");
            }

            _animator = self.GetComponent<Animator>();
            _summon = self;

            // 리스폰 위치에 순간이동
            if (self.GetComponent<Summon>().MyTeam)
            {
                // 해당 위치에 다른 유닛이 있는지 확인
                foreach (Transform point in BattleManager.instance.ASpawn)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, 1f); // 조절 가능한 반지름 사용

                    if (colliders.Length == 0)
                    {
                        self.transform.position = point.position;
                        break;
                    }
                }
            }
            else
            {
                foreach (Transform point in BattleManager.instance.BSpawn)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, 1f); // 조절 가능한 반지름 사용

                    if (colliders.Length == 0)
                    {
                        self.transform.position = point.position;
                        break;
                    }
                }
            }
            
            //_animator.SetTrigger("Respawn"); ToDo: 리스폰 애니메이션 고려
            SetData("State", ESummonState.Default);
            _summon.tag = "Summon";
            return ENodeState.Success;
        }
    }
    #endregion
    #region 죽음 관련
    //생존 확인
    public class CheckIfAlive : Node
    {
        private bool _isAlive;

        public CheckIfAlive() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _isAlive = !self.GetComponent<Summon>().IsDead();
            if (_isAlive == true)
            {
                SetData("State", ESummonState.Default);
                return ENodeState.Success;
            }
            else
            {
                SetData("State", ESummonState.Dead);
                return ENodeState.Failure;
            }
        }
    }
    // 죽기
    public class DutyDie : Node
    {
        private Animator _animator;
        private float _waitTime = Constants.RESPAWN_TIME;
        private float _timer;   // ToDo: 초기화 방법 필요
        private GameObject _summon;

        public DutyDie() { }
        public override ENodeState Evaluate()
        {
            _summon = (GameObject)GetData("Self");
            _animator = _summon.GetComponent<Animator>();
            _timer = _summon.GetComponent<Summon>()._deadTime;

            _animator.SetTrigger("Dead");
            if (_timer + _waitTime > BattleManager.instance.GameTime)
            {
                SetData("State", ESummonState.Respawn);
                return ENodeState.Success;
            }

            _summon.tag = "NonTarget";
            return ENodeState.Running;
        }
    }
    #endregion
    #region CC기 관련
    // CC기 걸렸는 지 확인
    public class CheckCC : Node
    {
        private Transform _transform;

        public CheckCC() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;

            if (_transform.GetComponent<Summon>().HasCC())
            {
                SetData("State", ESummonState.CC);
                return ENodeState.Success;
            }
            else
            {
                return ENodeState.Failure;
            }
        }
    }
    // CC 기 행동
    public class DutyCC : Node
    {
        private Summon _summon;
        private Buffer cc = new Buffer();

        public DutyCC() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _summon = self.GetComponent<Summon>();

            // 죽었을 시, interrupt
            if(_summon.IsDead())
            {
                return ENodeState.Failure;
            }

            // CC 걸림
            if (_summon.HasCC())
            {
                // CC 쿨타임 끝
                if (!cc.IsBufferCooldown(_summon.CurrentCCStats[((int)EBufferStats.Time)]))
                {
                    cc.FinishedCC(_summon.gameObject);
                    cc.ResetBufferCooldown();
                    SetData("State", ESummonState.Default);
                    return ENodeState.Success;
                }
                // CC 쿨타임 안 끝남
                else
                {
                    cc.UpdateBufferCooldown(Time.deltaTime);
                    return ENodeState.Running;
                }
            }
            // CC 안 걸림
            else
            {
                return ENodeState.Failure;
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
            //ToDo: 상대방 리스트로 관리가 필요하지 않을까?
            GameObject[] summons = GameObject.FindGameObjectsWithTag("Summon");
            GameObject self = (GameObject)GetData("Self");

            // 상대방 있는 경우
            foreach (GameObject summon in summons)
            {
                if (summon != self) // && summon.GetComponent<Summon>().myTeam == false
                {
                    //if(summon.GetComponent<Summon>().myTeam == false){
                    SetData("target", summon.transform);
                    return ENodeState.Success;
                    //}
                    //else
                    //{
                    //parent.parent.SetData("target", summon.transform);
                    //return NodeState.SUCCESS;
                    //}
                }
            }
            // 상대방 없는 경우
            ClearData("target");
            return ENodeState.Failure;
        }
    }
    // 사거리 밖에 있는 지 확인
    public class CheckEnemyOutOfAttackRange : Node
    {
        private Transform _transform;
        private float _attackRange;

        public CheckEnemyOutOfAttackRange() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;
            _attackRange = self.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)];

            object t = GetData("target");
            if (t == null)
            {
                return ENodeState.Failure;
            }
            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) > _attackRange)
            {
                return ENodeState.Success;
            }
            return ENodeState.Failure;
        }
    }
    //적을 향해 움직이기
    public class DoMoveToEnemy : Node
    {
        private Animator _animator;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private float _moveSpeed;

        public DoMoveToEnemy() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;
            _animator = self.GetComponent<Animator>();
            _spriteRenderer = self.GetComponent<SpriteRenderer>();
            _moveSpeed = self.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)];
            _rb = self.GetComponent<Rigidbody2D>();

            Transform target = (Transform)GetData("target");
            // ToDo: 여러 상대방 중 어떤 상대방?
            // 상대방 바라보기
            if (target != null)
            {
                if (_transform.position.x < target.transform.position.x)
                {
                    _transform.localScale = new Vector3(-1, 1, 1);
                    //_spriteRenderer.flipX = true;
                }
                else
                {
                    _transform.localScale = new Vector3(1, 1, 1);
                    //_spriteRenderer.flipX = false;
                }
            }

            // 애니메이션
            _animator.SetBool("Idle", false);
            _animator.SetBool("Move", true);
            SetData("State", ESummonState.Move);

            // 이동
            Vector3 direction = (target.transform.position - _transform.position).normalized;
            _transform.position += direction * _moveSpeed * Time.deltaTime;
            _rb.velocity = Vector2.zero;

            return ENodeState.Success;
        }
    }

    //적과 멀어지기
    public class DoMoveAwayFromEnemy : Node
    {
        private Animator _animator;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private float _moveSpeed;

        public DoMoveAwayFromEnemy() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;
            _animator = self.GetComponent<Animator>();
            _spriteRenderer = self.GetComponent<SpriteRenderer>();
            _moveSpeed = self.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)];
            _rb = self.GetComponent<Rigidbody2D>();

            Transform target = (Transform)GetData("target");
            // ToDo: 여러 상대방 중 어떤 상대방?
            // 상대방 바라보기
            if (target != null)
            {
                if (_transform.position.x < target.transform.position.x)
                {
                    _transform.localScale = new Vector3(-1, 1, 1);
                    //_spriteRenderer.flipX = true;
                }
                else
                {
                    _transform.localScale = new Vector3(1, 1, 1);
                    //_spriteRenderer.flipX = false;
                }
            }

            // 애니메이션
            _animator.SetBool("Idle", false);
            _animator.SetBool("Move", true);
            SetData("State", ESummonState.Move);

            // 이동
            Vector3 direction = (_transform.position - target.transform.position).normalized;
            _transform.position += direction * _moveSpeed * Time.deltaTime;
            _rb.velocity = Vector2.zero;


            return ENodeState.Success;
        }
    }
    // 사거리 이내에 있는 지 확인
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;
        private float _attackRange;

        public CheckEnemyInAttackRange() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;
            _attackRange = self.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)];

            object t = GetData("target");
            if (t == null)
            {
                return ENodeState.Failure;
            }
            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
            {
                return ENodeState.Success;
            }
            return ENodeState.Failure;
        }
    }
    // Idle 상태
    public class DoIdle : Node
    {
        private Animator _animator;

        public DoIdle() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _animator = self.GetComponent<Animator>();

            _animator.SetBool("Idle", true);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", false);
            SetData("State", ESummonState.Default);

            return ENodeState.Success;
        }
    }
    #endregion
    #region 스킬
    // 일반 공격하기 행동
    public class TaskAttack : Node
    {
        private Transform _transform;
        private Skill _skill;
        private Animator _animator;

        public TaskAttack(Skill skill)
        {
            _skill = skill;
        }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;
            _animator = self.GetComponent<Animator>();

            // Duty Event시, interrupt
            if (self.GetComponent<Summon>().CheckCriticalEvent())
            {
                _skill.StartSkillCooldown();
                return ENodeState.Failure;
            }


            Transform target = (Transform)GetData("target");
            SetData("State", ESummonState.Attack);
            
            bool isDoing = _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            if (isDoing)
            {
                return ENodeState.Running;
            }
            else
            {
                _skill.StartSkillCooldown();
                return ENodeState.Success;
            }

        }
    }
    // 스킬 사용 가능 여부 확인
    public class CheckSkill : Node
    {
        private Skill _skill;
        private float _coolTime;

        public CheckSkill(Skill skill)
        {
            _skill = skill;
            _coolTime = _skill.Stats[((int)ESkillStats.CoolTime)];
        }
        public override ENodeState Evaluate()
        {
            if (_skill.IsSkillCooldown())
            {
                return ENodeState.Failure;
            }
            else
            {
                _skill.SkiilCounter += 1;
                return ENodeState.Success;
            }
        }
    }
    // 스킬 사용하기
    public class TaskSkill : Node
    {
        private Skill _skill;
        private Transform _transform;
        private Animator _animator;

        public TaskSkill(Skill skill)
        {
            _skill = skill;
        }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;
            _animator = self.GetComponent<Animator>();

            // Duty Event시, interrupt
            if(self.GetComponent<Summon>().CheckCriticalEvent())
            {
                _skill.StartSkillCooldown();
                return ENodeState.Failure;
            }

            Transform target = (Transform)GetData("target");
            SetData("State", ESummonState.Skill);
            bool isDoing = _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            if(isDoing)
            {
                return ENodeState.Running;
            }
            else
            {
                _skill.StartSkillCooldown();
                return ENodeState.Success;
            }
        }
    }
    // 궁극기 사용 가능 여부 확인
    public class CheckUltGage : Node
    {
        //ToDo: 게이지 형식으로 변환
        private Skill _skill;
        private float _coolTime;

        public CheckUltGage(Skill skill)
        {
            _skill = skill;
            _coolTime = skill.Stats[((int)ESkillStats.CoolTime)];
        }
        public override ENodeState Evaluate()
        {
            if (_skill.IsSkillCooldown())
            {
                return ENodeState.Failure;
            }
            else
            {
                _skill.SkiilCounter += 1;
                return ENodeState.Success;
            }
        }
    }
    // 궁극기 사용하기
    public class TaskUlt : Node
    {
        private Skill _skill;
        private Transform _transform;
        private Animator _animator;

        public TaskUlt(Skill skill)
        {
            _skill = skill;
        }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");

            // Critical Event시, interrupt
            if (self.GetComponent<Summon>().CheckCriticalEvent())
            {
                _skill.StartSkillCooldown();
                return ENodeState.Failure;
            }

            _transform = self.transform;
            _animator = self.GetComponent<Animator>();

            Transform target = (Transform)GetData("target");
            SetData("State", ESummonState.Ult);
            bool isDoing = _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            if(isDoing)
            {
                return ENodeState.Running;
            }
            else
            {
                _skill.StartSkillCooldown();
                return ENodeState.Success;
            }
        }
    }
    #endregion
    #region 특수 노드
    // 원거리 캐릭터 전용 +(밀당 캐릭터)
    public class CheckEnemyTooClose : Node
    {
        private Transform _transform;
        private float _personalDistance;

        public CheckEnemyTooClose() { }
        public override ENodeState Evaluate()
        {
            GameObject self = (GameObject)GetData("Self");
            _transform = self.transform;
            _personalDistance = (self.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)]) * 0.5f;

            object t = GetData("target");
            if (t == null)
            {
                return ENodeState.Failure;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) <= _personalDistance)
            {
                return ENodeState.Success;
            }
            else 
            {
                return ENodeState.Failure;
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
                return ENodeState.Success;
            }
            else
            {
                return ENodeState.Failure;
            }
        }
    }
    #endregion
}