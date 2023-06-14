using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
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
}