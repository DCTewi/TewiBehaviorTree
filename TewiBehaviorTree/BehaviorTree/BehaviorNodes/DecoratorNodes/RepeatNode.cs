namespace BehaviorTree
{
    /// <summary>
    /// Node which repeats action for limit times
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class RepeatNode<T> : DecoratorNode<T>
    {
        private readonly int limit;
        private int count;

        /// <summary>
        /// Node which repeats action for limit times
        /// </summary>
        /// <param name="Owner">Owner</param>
        /// <param name="limit">Limits per time</param>
        public RepeatNode(int limit) : base()
        {
            this.limit = limit;
        }

        protected override void Initialize()
        {
            count = 0;
        }

        protected override BehaviorStatus Update()
        {
            while (true)
            {
                var status = Child.Tick();
                switch (status)
                {
                    case BehaviorStatus.Failure:
                        {
                            return BehaviorStatus.Failure;
                        }
                    case BehaviorStatus.Running:
                        {
                            return BehaviorStatus.Running;
                        }
                    case BehaviorStatus.Success:
                        {
                            return ++count == limit ? BehaviorStatus.Success : BehaviorStatus.Running;
                        }
                    default:
                        Child.Reset();
                        return status;
                }
            }
        }
    }
}