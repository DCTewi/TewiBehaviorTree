using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Node which have multiple children
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public abstract class CompositeNode<T> : BehaviorNodeBase<T>
    {
        protected List<BehaviorNodeBase<T>> children;

        public CompositeNode() : base()
        {
            children = new List<BehaviorNodeBase<T>>();
        }

        public override void AddChild(BehaviorNodeBase<T> node)
        {
            children.Add(node);
        }
    }
}
