namespace BehaviorTree
{
    /// <summary>
    /// Node which repeats action until action success or running for a limit times
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class UntilNode<T> : DecoratorNode<T>
    {
        private readonly int limit;
        private int count;

        /// <summary>
        /// Node which repeats action until action success or running for a limit times (100 as default)
        /// </summary>
        public UntilNode() : base()
        {
            limit = 100;
        }
        /// <summary>
        /// Node which repeats action until action success or running for a limit times
        /// </summary>
        /// <param name="limit">Attemp uplimit</param>
        public UntilNode(int limit) : base()
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
                            if (++count >= limit)
                            {
                                return BehaviorStatus.Failure;
                            }
                            else return BehaviorStatus.Running;
                        }
                    case BehaviorStatus.Running:
                        {
                            return BehaviorStatus.Running;
                        }

                    case BehaviorStatus.Success:
                        {
                            return BehaviorStatus.Success;
                        }
                    default:
                        Child.Reset();
                        return status;
                }
            }
        }
    }
}
