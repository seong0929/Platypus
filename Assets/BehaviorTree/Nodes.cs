using System.Collections.Generic;
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
        //Todo: �״� ���� �ε�
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
        private List<GameObject> enemies;

        public CheckEnemyInScene(List<GameObject> enemies)
        {
            this.enemies = enemies;
        }
        public override NodeState Evaluate()
        {
            // ���� ���� �ִ� ��� GameObject���� �����ͼ� enemies ����Ʈ�� �߰��մϴ�.
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag("Summon"))
                {
                    if (!enemies.Contains(obj))
                    {
                        enemies.Add(obj);
                    }
                }
            }
            if (allObjects == null) { return NodeState.FAILURE; }
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