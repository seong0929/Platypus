namespace BehaviorTree
{
    public class ParallelNode : Node
    {
        private int _successChildrenNum;
        private int _failureChildrenNum;
        private int _thresholdM;

        public ParallelNode(int thresholdM)
        {
            _successChildrenNum = 0;
            _failureChildrenNum = 0;
            this._thresholdM = thresholdM;
        }
        public override ENodeState Evaluate()
        {
            _successChildrenNum = 0;
            _failureChildrenNum = 0;

            foreach (Node child in children)
            {
                ENodeState childState = child.Evaluate();

                switch (childState)
                {
                    case ENodeState.Success:
                        _successChildrenNum++;
                        break;
                    case ENodeState.Failure:
                        _failureChildrenNum++;
                        break;
                }
            }
            if (_successChildrenNum >= _thresholdM)
            {
                state = ENodeState.Success;
            }
            else if (_failureChildrenNum > children.Count - _thresholdM)
            {
                state = ENodeState.Failure;
            }
            else
            {
                state = ENodeState.Running;
            }
            return state;
        }
    }
}