using UnityEngine;
using Skills;
using static Enums;

namespace BehaviorTree
{
    #region 리스폰 관련
    // 리스폰 확인
    public class CheckRespawn : Node
    {
    // ToDo: SetData 초기화를 GamaManager에 넣기
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
                Parent.Parent.SetData("State", ESummonState.Running);
                Parent.Parent.SetData("Self", _gameObject);
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
    public class TaskRespawn : Node
    {
        private Animator _animator;
        private GameObject _summon;

        public TaskRespawn(GameObject obj)
        {
            _animator = obj.GetComponent<Animator>();
            _summon = obj;
        }
        public override ENodeState Evaluate()
        {
            //ToDo: 리스폰 위치에 순간이동, 게임 메니저에서 리스폰 지점 찾기
            //_animator.SetTrigger("Respawn"); ToDo: 리스폰 애니메이션 고려
            SetData("State", ESummonState.Running);
            _summon.tag = "Summon";
            return ENodeState.Running;
        }
    }
    #endregion
    #region 죽음 관련
    //생존 확인
    public class CheckIfAlive : Node
    {
        private bool _isAlive;

        public CheckIfAlive(GameObject obj)
        {
            this._isAlive = obj.GetComponent<Summon>().IsDead();
        }
        public override ENodeState Evaluate()
        {
            if (_isAlive == true)
            {
                Parent.Parent.SetData("State", ESummonState.Running);
                return ENodeState.Success;
            }
            else
            {
                Parent.Parent.SetData("State", ESummonState.Dead);
                return ENodeState.Failure;
            }
        }
    }
    // 죽기
    public class TaskDie : Node
    {
        private Animator _animator;
        private float _waitTime = Constants.RESPAWN_TIME;
        private float _timer;
        private GameObject _summon;

        public TaskDie(GameObject obj)
        {
            _animator = obj.GetComponent<Animator>();
            _summon = obj;
        }
        public override ENodeState Evaluate()
        {
            _animator.SetTrigger("Dead");
            //ToDo: 팀 리스트 필요
            /* 죽은 캐릭터에 대한 정보를 모든 캐릭터들에게 전달
            foreach (var character in characters)
            {
                character.OnCharacterDeath(deadCharacter);
            }
             */
            /* 다른 캐릭터들에서 이 캐릭터를 타겟팅에서 제외
            foreach (var otherCharacter in otherCharacters)
            {
                otherCharacter.RemoveTarget(this);
            }*/
            Parent.Parent.SetData("State", ESummonState.Respawn);
            while (true)
            {
                _timer += Time.deltaTime;
                if (_timer > _waitTime)
                {
                    break;
                }
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

        public CheckCC(GameObject obj)
        {
            _transform = obj.transform;
        }
        public override ENodeState Evaluate()
        {
            if (_transform.GetComponent<Summon>().HasCC())
            {
                return ENodeState.Success;
            }
            else
            {
                return ENodeState.Failure;
            }
        }
    }
    // CC 기 행동
    public class TaskCC : Node
    {
        private Summon _summon;
        private CC cc = new CC();

        public TaskCC(GameObject obj)
        {
            _summon = obj.GetComponent<Summon>();
        }
        public override ENodeState Evaluate()
        {
            if (_summon.HasCC())
            {
                if (!cc.IsCcCooldown(_summon.CurrentCCStats[((int)ECCStats.Time)]))
                {
                    cc.FinishedCC(_summon.gameObject);
                    cc.ResetCcCooldown();
                    return ENodeState.Success;
                }
                else
                {
                    cc.UpdateCcCooldown(Time.deltaTime);
                    return ENodeState.Running;
                }
            }
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

            foreach (GameObject summon in summons)
            {
                if (summon != self) // && summon.GetComponent<Summon>().myTeam == false
                {
                    //if(summon.GetComponent<Summon>().myTeam == false){
                    Parent.Parent.SetData("target", summon.transform);
                    return ENodeState.Success;
                    //}
                    //else
                    //{
                    //parent.parent.SetData("target", summon.transform);
                    //return NodeState.SUCCESS;
                    //}
                }
            }
            ClearData("target");
            return ENodeState.Failure;
        }
    }

    // 사거리 밖에 있는 지 확인
    public class CheckEnemyOutOfAttackRange : Node
    {
        private Transform _transform;
        private float _attackRange;

        public CheckEnemyOutOfAttackRange(GameObject obj)
        {
            _transform = obj.transform;
            _attackRange = obj.GetComponent<Summon>().Stats[((int)Enums.ESummonStats.AttackRange)];
        }
        public override ENodeState Evaluate()
        {
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
    public class TaskMoveToEnemy : Node
    {
        private Animator _animator;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private float _moveSpeed;

        public TaskMoveToEnemy(GameObject obj)
        {
            _transform = obj.transform;
            _animator = obj.GetComponent<Animator>();
            _spriteRenderer = obj.GetComponent<SpriteRenderer>();
            _moveSpeed = obj.GetComponent<Summon>().Stats[((int)ESummonStats.MoveSpeed)];
            _rb = obj.GetComponent<Rigidbody2D>();
        }
        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            // ToDo: 여러 상대방 중 어떤 상대방?
            // 상대방 바라보기
            if (target != null)
            {
                if (_transform.position.x < target.transform.position.x)
                {
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _spriteRenderer.flipX = false;
                }
            }
            _animator.SetBool("Idle", false);
            _animator.SetBool("Move", true);
            Vector3 direction = (target.transform.position - _transform.position).normalized;
            _transform.position += direction * _moveSpeed * Time.deltaTime;
            _rb.velocity = Vector2.zero;
            return ENodeState.Running;
        }
    }
    // 사거리 이내에 있는 지 확인
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;
        private float _attackRange;

        public CheckEnemyInAttackRange(GameObject obj)
        {
            _transform = obj.transform;
            _attackRange = obj.GetComponent<Summon>().Stats[((int)ESummonStats.AttackRange)];
        }
        public override ENodeState Evaluate()
        {
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
    public class TaskIdle : Node
    {
        private Animator _animator;

        public TaskIdle(GameObject obj)
        {
            _animator = obj.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            _animator.SetBool("Idle", true);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", false);
            return ENodeState.Running;
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

        public TaskAttack(GameObject obj, Skill skill)
        {
            _transform = obj.transform;
            _skill = skill;
            _animator = obj.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            return ENodeState.Running;
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
            _coolTime = skill.Stats[((int)ESkillStats.CoolTime)];
        }
        public override ENodeState Evaluate()
        {
            if (BattleManager.instance.GameTime - (_skill.SkiilCounter * _coolTime) >= _coolTime)
            {
                _skill.SkiilCounter += 1;
                return ENodeState.Success;
            }
            else
            {
                return ENodeState.Failure;
            }
        }
    }
    // 스킬 사용하기
    public class TaskSkill : Node
    {
        private Skill _skill;
        private Transform _transform;
        private Animator _animator;

        public TaskSkill(GameObject obj, Skill skill)
        {
            _skill = skill;
            _transform = obj.transform;
            _animator = obj.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            return ENodeState.Running;
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
            if (BattleManager.instance.GameTime - (_skill.SkiilCounter * _coolTime) >= _coolTime)
            {
                Debug.Log("궁");
                _skill.SkiilCounter += 1;
                return ENodeState.Success;
            } else
            {
                return ENodeState.Failure;
            }
        }
    }
    // 궁극기 사용하기
    public class TaskUlt : Node
    {
        private Skill _skill;
        private Transform _transform;
        private Animator _animator;

        public TaskUlt(GameObject obj, Skill skill)
        {
            _skill = skill;
            _transform = obj.transform;
            _animator = obj.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            return ENodeState.Running;
        }
    }
    #endregion
    #region 특수 노드
    // 원거리 캐릭터 전용 +(밀당 캐릭터)
    public class CheckEnemyTooClose : Node
    {
        private Transform _transform;
        float _personalDistance;
        // ToDo: 수정 필요(일반 공격 사거리보다 클 시 이동만 함)
        public CheckEnemyTooClose(GameObject obj)
        {
            _transform = obj.transform;
            _personalDistance = obj.GetComponent<Summon>().Stats[((int)ESummonStats.PersonalDistance)];
        }
        public override ENodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                return ENodeState.Failure;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) < _personalDistance)
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