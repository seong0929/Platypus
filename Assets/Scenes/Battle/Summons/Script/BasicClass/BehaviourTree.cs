using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Enums;
using static UnityEngine.Rendering.DebugUI;

namespace BehaviorTree
{
    // 비헤버트리
    public abstract class Tree //: MonoBehaviour
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
    // 노드 클래스
    public class Node
    {
        public Node Parent; // 부모 노드
        public bool IsRoot => Parent == null; // 루트 노드인지 확인
        
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
        public void SetData(string key, object value) // 데이터 저장 
        {
            Node node = this;

            if (IsRoot) // 루트노드면 저장하기
            {
                _dataContext[key] = value;
                return;
            }
            else // 루트노드가 아니라면 루트노드를 찾아 저장하기
            {
                Node rootNode = FindRootNode();
                if (rootNode != null)
                {
                    rootNode.SetData(key, value);
                }
            }
                        
        }
        // 데이터 가져오기
        public object GetData(string key)
        {
            object value = null;

            if (IsRoot)
            {
                if (_dataContext.TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    Debug.Log("Get Data: Key not found");
                    return null; // Key not found in the dictionary
                }
            }
            else
            {
                Node rootNode = FindRootNode();
                if (rootNode != null)
                {
                    return rootNode.GetData(key);
                }
                else
                {
                    Debug.Log("Get Data: Root node not found");
                    return null; // Root node not found
                }
            }
        }
        // 데이터 지우기
        public bool ClearData(string key)
        {
            Debug.Log("Clear Data: " + key);

            if(IsRoot)
            {
                if (_dataContext.ContainsKey(key))
                {
                    _dataContext.Remove(key);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Node rootNode = FindRootNode();
                if (rootNode != null)
                {
                    return rootNode.ClearData(key);
                }
                else
                {
                    Debug.Log("Clear Data: Root node not found");
                    return false; // Root node not found
                }
            }
        }

        // Helper method to find the root node
        private Node FindRootNode()
        {
            Debug.Log("Find Root Node");
            Node currentNode = this;
            while (!currentNode.IsRoot)
            {
                currentNode = currentNode.Parent;
            }
            return currentNode;
        }

    }
    // 시퀀스 노드
    public class Sequence : Node
    {
        private int lastRunningNodeIndex = -1;
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
        // == and 하나라도 실패 시 실패
        public override ENodeState Evaluate()
        {
            // Start from the beginning or the last running node
            int startIndex = lastRunningNodeIndex >= 0 ? lastRunningNodeIndex : 0;

            for (int i = startIndex; i < children.Count; i++)
            {
                ENodeState childState = children[i].Evaluate();

                switch (childState)
                {
                    case ENodeState.Failure:
                        lastRunningNodeIndex = -1; // Reset
                        state = ENodeState.Failure;
                        return state;
                    case ENodeState.Running:
                        lastRunningNodeIndex = i; // Remember the running node
                        state = ENodeState.Running;
                        return state;
                    case ENodeState.Success:
                       lastRunningNodeIndex = -1;
                        // Continue to next child
                        continue;
                }
            }

            // All nodes succeeded
            lastRunningNodeIndex = -1; // Reset
            state = ENodeState.Success;
            return state;

        }
    }
    // 셀렉터 노드
    public class Selector : Node
    {
        private int lastRunningNodeIndex = -1;
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }
        // == or 모든 자식이 실패해야 실패
        public override ENodeState Evaluate()
        {
            int startIndex = lastRunningNodeIndex >= 0 ? lastRunningNodeIndex : 0;

            for (int i = startIndex; i < children.Count; i++)
            {
                ENodeState childState = children[i].Evaluate();

                switch (childState)
                {
                    case ENodeState.Success:
                        lastRunningNodeIndex = -1; // Reset
                        state = ENodeState.Success;
                        return state;
                    case ENodeState.Running:
                        lastRunningNodeIndex = i; // Remember the running node
                        state = ENodeState.Running;
                        return state;
                    case ENodeState.Failure:
                        lastRunningNodeIndex = -1;
                        // Continue to next child
                        continue;
                }
            }

            // All nodes failed
            lastRunningNodeIndex = -1; // Reset
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