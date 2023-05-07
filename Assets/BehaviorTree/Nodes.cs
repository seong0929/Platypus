using System.Collections.Generic;
//���� ��� ����
namespace BehaviorTree
{
    public class CheckIfAlive : Node
    {
        public override NodeState Evaluate()
        {
            // ���ϴ� ������ �����Ͽ� ��Ʈ ��带 ��ȯ�ϴ� �ڵ�
            return NodeState.SUCCESS;
        }
    }
    public class TaskDie : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskWait : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskRespawn : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class CheckEnemyInScene : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class CheckEnemyOutOfRange : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskMoveToEnemy : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class CheckEnemyInAttackRange : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class CheckEnemyNotToClose : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class CheckEnemyNotToRange : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskAttack : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskIdle : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class CheckSkill : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskSkill : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class CheckUltGage : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskUlt : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    //Ư�� ��Ʈ
    public class CheckEnemyTooClose : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
}