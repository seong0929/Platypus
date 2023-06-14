using System.Collections.Generic;

namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

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
}