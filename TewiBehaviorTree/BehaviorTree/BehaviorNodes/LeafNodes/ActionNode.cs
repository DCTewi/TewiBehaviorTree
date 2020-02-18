namespace BehaviorTree
{
    /// <summary>
    /// [Leaf Node]
    /// Node which contains actuall actions
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public abstract class ActionNode<T> : BehaviorNodeBase<T>
    {
        protected readonly T owner;

        public ActionNode(T owner) : base()
        {
            this.owner = owner;
        }
    }
}
