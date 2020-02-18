namespace BehaviorTree
{
    /// <summary>
    /// [Leaf Node]
    /// Node which contains conditions
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public abstract class ConditionNode<T> : BehaviorNodeBase<T>
    {
        protected readonly T owner;
        public ConditionNode(T owner) : base()
        {
            this.owner = owner;
        }

        /// <summary>
        /// Condition in this node
        /// </summary>
        /// <returns>True if condition</returns>
        protected abstract bool Condition();

        protected override BehaviorStatus Update()
        {
            bool result = Condition();
            return result ? BehaviorStatus.Success : BehaviorStatus.Failure;
        }
    }
}
