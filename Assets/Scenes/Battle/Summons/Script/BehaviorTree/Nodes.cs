using UnityEngine;

//공통 노드 선언
namespace BehaviorTree
{
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

        public TaskRespawn(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            //ToDo: 리스폰 위치에 순간이동, 게임 메니저에서 리스폰 지점 찾기
            //_animator.SetTrigger("Respawn"); ToDo: 리스폰 애니메이션 고려
            SetData("State", ESummonState.Running);
            return ENodeState.Running;
        }
    }
    //생존 확인
    public class CheckIfAlive : Node
    {
        private bool isAlive;

        public CheckIfAlive(bool isAlive)
        {
            this.isAlive = isAlive;
        }
        public override ENodeState Evaluate()
        {
            if (isAlive == true)
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
        private float _waitTime = Constants.Respawn_TIME;
        private float _timer;

        public TaskDie(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
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
            return ENodeState.Running;
        }
    }
    // CC기 걸렸는 지 확인
    public class CheckCC : Node
    {
        private Transform _transform;

        public CheckCC(Transform transform)
        {
            _transform = transform;
        }
        public override ENodeState Evaluate()
        {
            if (_transform.GetComponent<Summon>().IsCC)
            {
                return ENodeState.Success;
            }
            else 
            {
                return ENodeState.Failure;
            }
        }
    }
    // CC기 걸린 행동
    public class TaskCC : Node 
    {
        public override ENodeState Evaluate() 
        {
            /*
             switch()
            {
                case Enums.ECC.Stan:
                    break;
                default:
                    IsCC = false;
                    break;
            }
             */
            return ENodeState.Running;
        }
    }
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

        public CheckEnemyOutOfAttackRange(Transform transform, float attackRange)
        {
            _transform = transform;
            _attackRange = attackRange;
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
        private float _moveSpeed;

        public TaskMoveToEnemy(Transform transform, float moveSpeed)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _spriteRenderer = transform.GetComponent<SpriteRenderer>();
            _moveSpeed = moveSpeed;
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
            return ENodeState.Running;
        }
    }
    // 사거리 이내에 있는 지 확인
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;
        private float _attackRange;

        public CheckEnemyInAttackRange(Transform transform, float attackRange)
        {
            _transform = transform;
            _attackRange = attackRange;
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
    // 일반 공격하기 행동
    public class TaskAttack : Node
    {
        private Animator _animator;
        private Transform _transform;

        public TaskAttack(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (_transform.position.x < target.position.x)
            {
                _transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                _transform.GetComponent<SpriteRenderer>().flipX = false;
            }
            _animator.SetBool("Idle", false);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", true);
            return ENodeState.Running;
        }
    }
    // Idle 상태
    public class TaskIdle : Node
    {
        private Animator _animator;

        public TaskIdle(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            _animator.SetBool("Idle", true);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", false);
            return ENodeState.Running;
        }
    }
    // 스킬 사용 가능 여부 확인
    public class CheckSkill : Node
    {
        private Skill _skill;
        private float _coolTime;
        public CheckSkill(Skill skill, float coolTime)
        {
            _skill = skill;
            _coolTime = coolTime;
        }
        public override ENodeState Evaluate()
        {
            if (BattleManager.instance.GameTime - (_skill.skiilCounter * _coolTime) >= _coolTime)
            {
                _skill.skiilCounter += 1;
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
        public TaskSkill(Transform transform, Skill skill)
        {
            _skill = skill;
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
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
        public CheckUltGage(Skill skill, float coolTime)
        {
            _skill = skill;
            _coolTime = coolTime;
        }
        public override ENodeState Evaluate()
        {
            if (BattleManager.instance.GameTime - (_skill.skiilCounter * _coolTime) >= _coolTime)
            {
                _skill.skiilCounter += 1;
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
        public TaskUlt(Transform transform, Skill skill)
        {
            _skill = skill;
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }
        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            return ENodeState.Running;
        }
    }
    //특수 노트
    // 원거리 캐릭터 전용 +(밀당 캐릭터)
    public class CheckEnemyTooClose : Node
    {
        private Transform _transform;
        float _personalDistance;
        public CheckEnemyTooClose(Transform transform, float personalDistance)
        {
            _transform = transform;
            _personalDistance = personalDistance;
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
}
