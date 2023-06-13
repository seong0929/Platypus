namespace BehaviorTree
{
    public class Inverter : Node
    {
        protected Node child;

        public Inverter(Node child)
        {
            this.child = child;
            this.child.Parent = this;
        }
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
