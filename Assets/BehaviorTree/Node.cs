using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }
        public Node(List<Node> children)
        {
            foreach (Node child in children)
                AddChild(child);
        }

        private void AddChild(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }
    //노드 선언
    public class IdleNode : Node
    {
        public override NodeState Evaluate()
        {
            // 원하는 노드들을 생성하여 루트 노드를 반환하는 코드
            return NodeState.SUCCESS;
        }
    }
    public class AttackNode : Node {
        public override NodeState Evaluate() {
            return NodeState.SUCCESS;
        }
    }
    public class MoveNode : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class SkillNode : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
    public class UltNode : Node
    {
        public override NodeState Evaluate()
        {
            return NodeState.SUCCESS;
        }
    }
}
