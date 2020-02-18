namespace BehaviorTree
{
    /// <summary>
    /// Node which will run children paralllely
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class ParallelNode<T> : CompositeNode<T>
    {
        public BehaviorPolicy SuccessPolicy { get; protected set; }
        public BehaviorPolicy FailurePolicy { get; protected set; }

        /// <summary>
        /// Node which will run children paralllely
        /// </summary>
        /// <param name="success">Policy to success(High level)</param>
        /// <param name="failure">Policy to failure</param>
        public ParallelNode(BehaviorPolicy success, BehaviorPolicy failure) : base()
        {
            SuccessPolicy = success;
            FailurePolicy = failure;
        }

        protected override BehaviorStatus Update()
        {
            int cntSuccess = 0, cntFailure = 0;
            foreach (var node in children) // Foreach node
            {
                // If node is running or invaild
                if (node.Status != BehaviorStatus.Success && node.Status != BehaviorStatus.Failure)
                {
                    node.Tick();
                }

                // Get one success node
                if (node.Status == BehaviorStatus.Success)
                {
                    ++cntSuccess;
                    if (SuccessPolicy == BehaviorPolicy.RequireOne)
                    {
                        node.Reset();
                        return BehaviorStatus.Success;
                    }
                }
                // Get one failure node
                if (node.Status == BehaviorStatus.Failure)
                {
                    ++cntFailure;
                    if (FailurePolicy == BehaviorPolicy.RequireOne)
                    {
                        node.Reset();
                        return BehaviorStatus.Failure;
                    }
                }
            }

            // Check Require All 
            if (SuccessPolicy == BehaviorPolicy.RequireAll && cntSuccess == children.Count)
            {
                foreach (var i in children) i.Reset();
                return BehaviorStatus.Success;
            }
            if (FailurePolicy == BehaviorPolicy.RequireAll && cntFailure == children.Count)
            {
                foreach (var i in children) i.Reset();
                return BehaviorStatus.Failure;
            }

            // Else, Continue running
            return BehaviorStatus.Running;
        }

        protected override void Terminate()
        {
            foreach (var i in children)
            {
                if (i.Status == BehaviorStatus.Running)
                {
                    i.Abort();
                }
            }
        }
    }
}