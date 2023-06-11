using UnityEngine;

//공통 노드 선언
namespace BehaviorTree
{
    // 리스폰 확인
    public class CheckRespawn : Node
    {
    // ToDo: SetData 초기화를 GamaManager에 넣기
        private GameObject _gameObject;
        public CheckRespawn(GameObject gameObject) { _gameObject = gameObject; }
        public override ENodeState Evaluate()
        {
            object s = GetData("State");
            if (s == null)
            {
                parent.parent.SetData("State", ESummonState.RUNNING);
                parent.parent.SetData("Self", _gameObject);
                return ENodeState.FAILURE;
            }
            if (GetData("State").Equals(ESummonState.RESPAWN))
            {
                return ENodeState.SUCCESS;
            }
            return ENodeState.FAILURE;
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
            SetData("State", ESummonState.RUNNING);
            return ENodeState.RUNNING;
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
                parent.parent.SetData("State", ESummonState.RUNNING);
                return ENodeState.SUCCESS;
            }
            else
            {
                parent.parent.SetData("State", ESummonState.DEAD);
                return ENodeState.FAILURE;
            }
        }
    }
    // 죽기
    public class TaskDie : Node
    {
        private Animator _animator;
        float waitTime = Constants.Respawn_TIME;
        float timer;
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
            parent.parent.SetData("State", ESummonState.RESPAWN);
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > waitTime)
                {
                    break;
                }
            }
            return ENodeState.RUNNING;
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
                    parent.parent.SetData("target", summon.transform);
                    return ENodeState.SUCCESS;
                    //}
                    //else
                    //{
                    //parent.parent.SetData("target", summon.transform);
                    //return NodeState.SUCCESS;
                    //}
                }
            }

            ClearData("target");
            return ENodeState.FAILURE;
        }
    }

    // 사거리 밖에 있는 지 확인
    public class CheckEnemyOutOfAttackRange : Node
    {
        private Transform _transform;
        float _attackRange;

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
                return ENodeState.FAILURE;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) > _attackRange)
            {
                return ENodeState.SUCCESS;
            }
            return ENodeState.FAILURE;
        }
    }
    //적을 향해 움직이기
    public class TaskMoveToEnemy : Node
    {
        private Animator _animator;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        float _moveSpeed;

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
            return ENodeState.RUNNING;
        }
    }
    // 사거리 이내에 있는 지 확인
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;
        float _attackRange;

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
                return ENodeState.FAILURE;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
            {
                return ENodeState.SUCCESS;
            }
            return ENodeState.FAILURE;
        }
    }
    // 일반 공격하기
    public class TaskAttack : Node
    {
        // 체력 감소에 관한
        private Animator _animator;
        private Transform _transform;

        //private Transform _lastTarget;

        //private float _attackTime = 1f;
        //private float _attackCounter = 0f;

        public TaskAttack(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override ENodeState Evaluate()
        {
            /* 데미지 관련
            Transform target = (Transform)GetData("target");
            if (target != _lastTarget)
            {
                _lastTarget = target;
            }

            _attackCounter += Time.deltaTime;
            if (_attackCounter >= _attackTime)
            {
                    _attackCounter = 0f;
            }
             */
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
            return ENodeState.RUNNING;
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
            return ENodeState.RUNNING;
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
                return ENodeState.SUCCESS;
            }
            else
            {
                return ENodeState.FAILURE;
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
            return ENodeState.RUNNING;
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
                return ENodeState.SUCCESS;
            } else
            {
                return ENodeState.FAILURE;
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
            return ENodeState.RUNNING;
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
                return ENodeState.FAILURE;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) < _personalDistance)
            {
                return ENodeState.SUCCESS;
            }
            else 
            {
                return ENodeState.FAILURE;
            }
        }
    }
}
