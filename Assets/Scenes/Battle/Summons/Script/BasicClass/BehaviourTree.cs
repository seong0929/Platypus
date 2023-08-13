using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    // �����Ʈ��
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }
        private void Update()
        {
            if (_root != null)
                _root.Evaluate();   // ��
        }
        protected abstract Node SetupTree();
    }
    // ��� ����
    public enum ENodeState
    {
        Running,
        Success,
        Failure
    }
    // ��ȯ�� ��� ����
    public enum ESummonState
    {
        Running,    // �������
        Dead,   // ����
        Respawn // ������ ��
    }
    // ��� Ŭ����
    public class Node
    {
        public Node Parent; // �θ� ���
        protected ENodeState state; // �� ����
        protected List<Node> children = new List<Node>();   // �ڽ� ���
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>(); // ������ ��
        #region ������
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
        public virtual ENodeState Evaluate() => ENodeState.Failure; // �� ���� �Լ�
        #endregion
        public void SetData(string key, object value) { _dataContext[key] = value; }    // ������ ����
        // ������ ��������
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
        // ������ �����
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
    // ������ ���
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
        // == and �ϳ��� ���� �� ����
        public override ENodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case ENodeState.Failure:
                        state = ENodeState.Failure;
                        return state;
                    case ENodeState.Success:
                        continue;
                    case ENodeState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = ENodeState.Success;
                        return state;
                }
            }
            state = anyChildIsRunning ? ENodeState.Running : ENodeState.Success;
            return state;
        }
    }
    // ������ ���
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }
        // == or ��� �ڽ��� �����ؾ� ����
        public override ENodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case ENodeState.Failure:
                        continue;
                    case ENodeState.Success:
                        state = ENodeState.Success;
                        return state;
                    case ENodeState.Running:
                        state = ENodeState.Running;
                        return state;
                    default:
                        continue;
                }
            }
            state = ENodeState.Failure;
            return state;
        }
    }
    // �з� ���
    public class ParallelNode : Node
    {
        private int _successChildrenNum;
        private int _failureChildrenNum;

        public ParallelNode()
        {
            _successChildrenNum = 0;
            _failureChildrenNum = 0;
        }
        public override ENodeState Evaluate()
        {
            bool hasRunningChild = false;

            foreach (Node child in children)
            {
                ENodeState childState = child.Evaluate();

                if (childState == ENodeState.Success)
                {
                    _successChildrenNum++;
                }
                else if (childState == ENodeState.Failure)
                {
                    _failureChildrenNum++;
                }
                else if (childState == ENodeState.Running)
                {
                    hasRunningChild = true;
                }
            }

            if (_successChildrenNum > 0)
            {
                state = ENodeState.Success;
            }
            else if (_failureChildrenNum == children.Count)
            {
                state = ENodeState.Failure;
            }
            else if (hasRunningChild)
            {
                state = ENodeState.Running;
            }
            return state;
        }
    }
    // �ι��� ���
    public class Inverter : Node
    {
        protected Node child;

        public Inverter(Node child)
        {
            this.child = child;
            this.child.Parent = this;
        }
        // �ڽ��� ����� �ݴ�� ��ȯ
        public override ENodeState Evaluate()
        {
            switch (child.Evaluate())
            {
                case ENodeState.Success:
                    state = ENodeState.Failure;
                    break;

                case ENodeState.Failure:
                    state = ENodeState.Success;
                    break;

                case ENodeState.Running:
                    state = ENodeState.Running;
                    break;

                default:
                    state = ENodeState.Failure;
                    break;
            }
            return state;
        }
    }
}
