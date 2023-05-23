using UnityEngine;

//���� ��� ����
namespace BehaviorTree
{
    //���� Ȯ��
    public class CheckIfAlive : Node
    {
        private bool isAlive;

        public CheckIfAlive(bool isAlive)
        {
            this.isAlive = isAlive;
        }

        public override NodeState Evaluate()
        {
            if (isAlive == true)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
    //��ȯ�� ���� �ൿ
    public class TaskDie : Node
    {
        private Animator _animator;

        public TaskDie(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }
        public override NodeState Evaluate()
        {
            _animator.SetBool("Dead", true);
            //ToDo: �� ����Ʈ �ʿ�
            /* ���� ĳ���Ϳ� ���� ������ ��� ĳ���͵鿡�� ����
            foreach (var character in characters)
            {
                character.OnCharacterDeath(deadCharacter);
            }
             */
            /* �ٸ� ĳ���͵鿡�� �� ĳ���͸� Ÿ���ÿ��� ����
            foreach (var otherCharacter in otherCharacters)
            {
                otherCharacter.RemoveTarget(this);
            }*/
            return NodeState.RUNNING;
        }
    }
    //������ �ð�
    public class TaskWait : Node
    {
        float waitTime = Constants.respawntime;
        float timer;
        public override NodeState Evaluate()
        {
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > waitTime)
                {
                    break;
                }
            }
            return NodeState.SUCCESS;
        }
    }
    
    public class TaskRespawn : Node
    {
        private Animator _animator;

        public TaskRespawn(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }
        public override NodeState Evaluate()
        {
            //ToDo: ������ ��ġ�� �����̵�
            _animator.SetBool("Respawn", true);
            return NodeState.RUNNING;
        }
    }

    public class CheckEnemyInScene : Node
    {
        //ToDo: ���� ����Ʈ�� ������ �ʿ����� ������?
        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                GameObject target = GameObject.FindGameObjectWithTag("Summon");

                if (target != null)
                {
                    parent.parent.SetData("target", target.transform);
                    return NodeState.SUCCESS;
                }
                return NodeState.FAILURE;
            }
            return NodeState.SUCCESS;
        }
    }
    public class CheckEnemyOutOfAttackRange : Node
    {
        private Transform _transform;
        float _attackRange;

        public CheckEnemyOutOfAttackRange(Transform transform, float attackRange)
        {
            _transform = transform;
            _attackRange = attackRange;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                return NodeState.FAILURE;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) > _attackRange)
            {
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
    }
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
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            // ToDo: ���� ���� �� � ����?
            // ���� �ٶ󺸱�
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
            _animator.SetBool("IDLE", false);
            _animator.SetBool("Move", true);
            Vector3 direction = (target.transform.position - _transform.position).normalized;
            _transform.position += direction * _moveSpeed * Time.deltaTime;
            return NodeState.RUNNING;
        }
    }
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;
        float _attackRange;

        public CheckEnemyInAttackRange(Transform transform, float attackRange)
        {
            _transform = transform;
            _attackRange = attackRange;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                return NodeState.FAILURE;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
            {
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
    }
    public class TaskAttack : Node
    {
        // ü�� ���ҿ� ����
        private Animator _animator;

        private Transform _lastTarget;

        private float _attackTime = 1f;
        private float _attackCounter = 0f;

        public TaskAttack(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            /* ������ ����
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
            _animator.SetBool("IDLE", false);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", true);
            return NodeState.RUNNING;
        }
    }
    public class TaskIdle : Node
    {
        private Animator _animator;

        public TaskIdle(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            _animator.SetBool("IDLE", true);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", false);
            return NodeState.RUNNING;
        }
    }
    public class CheckSkill : Node
    {
        private Skill _skill;
        private float _coolTime;
        public CheckSkill(Skill skill, float coolTime)
        {
            _skill = skill;
            _coolTime = coolTime;
        }
        public override NodeState Evaluate()
        {
            if (GameManager.instance.GameTime - (_skill.skiilCounter * _coolTime) >= _coolTime)
            {
                _skill.skiilCounter += 1;
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
    public class TaskSkill : Node
    {
        private Skill _skill;
        public TaskSkill(Skill skill)
        {
            _skill = skill;
        }
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(target.gameObject);
            return NodeState.RUNNING;
        }
    }
    public class CheckUltGage : Node
    {
        //ToDo: ������ �������� ��ȯ
        private Skill _skill;
        private float _coolTime;
        public CheckUltGage(Skill skill, float coolTime)
        {
            _skill = skill;
            _coolTime = coolTime;
        }
        public override NodeState Evaluate()
        {
            if (GameManager.instance.GameTime - (_skill.skiilCounter * _coolTime) >= _coolTime)
            {
                _skill.skiilCounter += 1;
                return NodeState.SUCCESS;
            } else
            {
                return NodeState.FAILURE;
            }
        }
    }
    public class TaskUlt : Node
    {
        private Skill _skill;
        public TaskUlt(Skill skill)
        {
            _skill = skill;
        }
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(target.gameObject);
            return NodeState.RUNNING;
        }
    }
    //Ư�� ��Ʈ
    // ���Ÿ� ĳ���� ���� +(�д� ĳ����)
    public class CheckEnemyTooClose : Node
    {
        private Transform _transform;
        float _personalDistance;
        public CheckEnemyTooClose(Transform transform, float personalDistance)
        {
            _transform = transform;
            _personalDistance = personalDistance;
        }
        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                return NodeState.FAILURE;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) < _personalDistance)
            {
                return NodeState.SUCCESS;
            }
            else 
            {
                return NodeState.FAILURE;
            }
        }
    }
}