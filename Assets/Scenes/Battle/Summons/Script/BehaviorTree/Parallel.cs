namespace BehaviorTree
{
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
}