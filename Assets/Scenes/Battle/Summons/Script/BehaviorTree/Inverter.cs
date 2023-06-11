using System.Collections.Generic;

namespace BehaviorTree
{
    public class Inverter : Node
    {
        protected Node child;

        public Inverter(Node child)
        {
            this.child = child;
            this.child.parent = this;
        }
        public override ENodeState Evaluate()
        {
            switch (child.Evaluate())
            {
                case ENodeState.SUCCESS:
                    state = ENodeState.FAILURE;
                    break;

                case ENodeState.FAILURE:
                    state = ENodeState.SUCCESS;
                    break;

                case ENodeState.RUNNING:
                    state = ENodeState.RUNNING;
                    break;

                default:
                    state = ENodeState.FAILURE;
                    break;
            }
            return state;
        }
    }
}
