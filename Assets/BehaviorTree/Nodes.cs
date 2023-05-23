using UnityEngine;

//���� ��� ����
namespace BehaviorTree
{
    // ������ Ȯ��
    public class CheckRespawn : Node
    {
        // GameManager���� �ʱ� SummonState�� ����
        public override NodeState Evaluate()
        {
            if (GetData("Self").Equals(SummonState.RESPAWN))
            {
                return NodeState.SUCCESS;
            }
            else if (!GetData("Self").Equals(SummonState.DEAD))
            {
                SetData("Self", SummonState.RUNNING);
                return NodeState.FAILURE;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
    // ������ �ϱ�
    public class TaskRespawn : Node
    {
        private Animator _animator;

        public TaskRespawn(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }
        public override NodeState Evaluate()
        {
            //ToDo: ������ ��ġ�� �����̵�, ���� �޴������� ������ ���� ã��
            //_animator.SetTrigger("Respawn"); ToDo: ������ �ִϸ��̼� ���
            SetData("Self", SummonState.RUNNING);
            return NodeState.RUNNING;
        }
    }
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
                parent.parent.SetData("Self", SummonState.RUNNING);
                return NodeState.SUCCESS;
            }
            else
            {
                parent.parent.SetData("Self", SummonState.DEAD);
                return NodeState.FAILURE;
            }
        }
    }
    // �ױ�
    public class TaskDie : Node
    {
        private Animator _animator;
        float waitTime = Constants.respawntime;
        float timer;
        public TaskDie(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }
        public override NodeState Evaluate()
        {
            _animator.SetTrigger("Dead");
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
            parent.parent.SetData("Self", SummonState.RESPAWN);
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > waitTime)
                {
                    break;
                }
            }
            return NodeState.RUNNING;
        }
    }
    // ���� �ִ� �� Ȯ��
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
                else
                {
                    ClearData("target");
                    return NodeState.FAILURE;
                }
            }
            return NodeState.SUCCESS;
        }
    }
    // ��Ÿ� �ۿ� �ִ� �� Ȯ��
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
    //���� ���� �����̱�
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
            _animator.SetBool("Idle", false);
            _animator.SetBool("Move", true);
            Vector3 direction = (target.transform.position - _transform.position).normalized;
            _transform.position += direction * _moveSpeed * Time.deltaTime;
            return NodeState.RUNNING;
        }
    }
    // ��Ÿ� �̳��� �ִ� �� Ȯ��
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
    // �Ϲ� �����ϱ�
    public class TaskAttack : Node
    {
        // ü�� ���ҿ� ����
        private Animator _animator;

        //private Transform _lastTarget;

        //private float _attackTime = 1f;
        //private float _attackCounter = 0f;

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
            _animator.SetBool("Idle", false);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", true);
            return NodeState.RUNNING;
        }
    }
    // Idle ����
    public class TaskIdle : Node
    {
        private Animator _animator;

        public TaskIdle(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            _animator.SetBool("Idle", true);
            _animator.SetBool("Move", false);
            _animator.SetBool("Attack", false);
            return NodeState.RUNNING;
        }
    }
    // ��ų ��� ���� ���� Ȯ��
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
    // ��ų ����ϱ�
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
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(_transform.gameObject, target.gameObject, _animator);
            return NodeState.RUNNING;
        }
    }
    // �ñر� ��� ���� ���� Ȯ��
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
    // �ñر� ����ϱ�
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
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _skill.Execute(_transform.gameObject, target.gameObject, _animator);
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