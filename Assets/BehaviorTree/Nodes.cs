using System.Collections.Generic;
using UnityEngine;

//공통 노드 선언
namespace BehaviorTree
{
    //생존 확인
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
    //소환수 죽음 행동
    public class TaskDie : Node
    {
        //Todo: 죽는 행위 로드
        public TaskDie()
        {
        }
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class TaskWait : Node
    {
        float delay;
        float timer;
        public TaskWait(float delay)
        {
            this.delay = delay;
        }
        public override NodeState Evaluate()
        {
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    break;
                }
            }
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
        private bool isEnemies;

        public CheckEnemyInScene(bool isEnemies)
        {
            this.isEnemies = isEnemies;
        }
        public override NodeState Evaluate()
        {
            // 현재 씬에 있는 모든 GameObject들을 가져와서 enemies 리스트에 추가합니다.
            if (isEnemies == true)
            { return NodeState.SUCCESS; }
            else { return NodeState.FAILURE; }
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