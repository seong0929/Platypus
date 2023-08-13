using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    // 비헤버트리
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
                _root.Evaluate();   // 평가
        }
        protected abstract Node SetupTree();
    }
    // 노드 상태
    public enum ENodeState
    {
        Running,
        Success,
        Failure
    }
    // 소환수 노드 상태
    public enum ESummonState
    {
        Running,    // 살아있음
        Dead,   // 죽음
        Respawn // 리스폰 중
    }
    // 노드 클래스
    public class Node
    {
        public Node Parent; // 부모 노드
        protected ENodeState state; // 현 상태
        protected List<Node> children = new List<Node>();   // 자식 노드
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>(); // 저장한 값
        #region 생성자
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
        public virtual ENodeState Evaluate() => ENodeState.Failure; // 평가 가상 함수
        #endregion
        public void SetData(string key, object value) { _dataContext[key] = value; }    // 데이터 저장
        // 데이터 가져오기
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
        // 데이터 지우기
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
    // 시퀀스 노드
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
        // == and 하나라도 실패 시 실패
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
    // 셀렉터 노드
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }
        // == or 모든 자식이 실패해야 실패
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
    // 패럴 노드
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
    // 인버터 노드
    public class Inverter : Node
    {
        protected Node child;

        public Inverter(Node child)
        {
            this.child = child;
            this.child.Parent = this;
        }
        // 자식의 결과의 반대로 반환
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
