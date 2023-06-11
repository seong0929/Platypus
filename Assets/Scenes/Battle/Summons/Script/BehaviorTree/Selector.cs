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
                    case ENodeState.FAILURE:
                        continue;
                    case ENodeState.SUCCESS:
                        state = ENodeState.SUCCESS;
                        return state;
                    case ENodeState.RUNNING:
                        state = ENodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = ENodeState.FAILURE;
            return state;
        }
    }
}