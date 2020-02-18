namespace BehaviorTree
{
    /// <summary>
    /// Node which processing details
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DecoratorNode<T> : BehaviorNodeBase<T>
    {
        protected BehaviorNodeBase<T> Child;

        public override void AddChild(BehaviorNodeBase<T> child)
        {
            if (Child != null)
            {
                throw new System.ArgumentException("Decorator can only have one child!", "child");
            }
            Child = child;
        }

        public DecoratorNode() : base() { }
    }
}