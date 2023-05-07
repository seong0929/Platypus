using System.Collections.Generic;

namespace BehaviorTree
{
    public class InverterNode : Node
    {
        protected Node child;

        public InverterNode(Node child)
        {
            this.child = child;
            this.child.parent = this;
        }

        public override NodeState Evaluate()
        {
            switch (child.Evaluate())
            {
                case NodeState.SUCCESS:
                    state = NodeState.FAILURE;
                    break;

                case NodeState.FAILURE:
                    state = NodeState.SUCCESS;
                    break;

                case NodeState.RUNNING:
                    state = NodeState.RUNNING;
                    break;

                default:
                    state = NodeState.FAILURE;
                    break;
            }

            return state;
        }
    }
}
