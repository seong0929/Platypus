using System.Collections.Generic;

namespace BehaviorTree
{
    public enum ENodeState
    {
        Running,
        Success,
        Failure
    }
    public enum ESummonState
    {
        Running,
        Dead,
        Respawn
    }

    public class Node
    {
        public Node Parent;
        protected ENodeState state;
        protected List<Node> children = new List<Node>();
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            Parent = null;
        }
        public Node(List<Node> children)
        {
            foreach (Node child in children)
                AddChild(child);
        }
        private void AddChild(Node node)
        {
            node.Parent = this;
            children.Add(node);
        }
        public virtual ENodeState Evaluate() => ENodeState.Failure;
        public void SetData(string key, object value){ _dataContext[key] = value; }
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = Parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.Parent;
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
            Node node = Parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.Parent;
            }
            return false;
        }
    }
}
