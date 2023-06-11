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
                    case ENodeState.FAILURE:
                        state = ENodeState.FAILURE;
                        return state;
                    case ENodeState.SUCCESS:
                        continue;
                    case ENodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = ENodeState.SUCCESS;
                        return state;
                }
            }
            state = anyChildIsRunning ? ENodeState.RUNNING : ENodeState.SUCCESS;
            return state;
        }
    }
}