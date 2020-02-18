using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Node which ticks every child nodes **until get a Success**
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class SelectorNode<T> : CompositeNode<T>
    {
        protected List<BehaviorNodeBase<T>>.Enumerator currentChild;

        public SelectorNode() : base() { }

        protected override void Initialize()
        {
            currentChild = children.GetEnumerator();
            // Get first enumerator
            // (Fixed bug) Can't Movenext() before Tick() in Update() because it'll skip the running node.
            currentChild.MoveNext();
        }

        protected override BehaviorStatus Update()
        {
            while (true) // Foreach child
            {
                // Tick current child
                var status = currentChild.Current.Tick();
                // If get success or running, make self as same as it
                if (status != BehaviorStatus.Failure)
                {
                    return status;
                }
                // Get the end of children, but no success or running (Can't move up)
                if (!currentChild.MoveNext())
                {
                    return BehaviorStatus.Failure;
                }
            }
        }
    }
}
