using System.Collections.Generic;
//공통 노드 선언
namespace BehaviorTree
{
    public class CheckIfAlive : Node
    {
        public override NodeState Evaluate()
        {
            // 원하는 노드들을 생성하여 루트 노드를 반환하는 코드
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
    //특수 노트
    public class CheckEnemyTooClose : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
}