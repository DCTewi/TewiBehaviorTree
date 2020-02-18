using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Node which ticks every child nodes **until get a Failure**
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class SequenceNode<T> : CompositeNode<T>
    {
        protected List<BehaviorNodeBase<T>>.Enumerator currentChild;

        public SequenceNode() : base() { }

        protected override void Initialize()
        {
            currentChild = children.GetEnumerator();
            currentChild.MoveNext();
        }

        protected override BehaviorStatus Update()
        {
            while (true) // Foreach child
            {
                // Tick current child
                var status = currentChild.Current.Tick();
                // If get failure or running, make self as same as it
                if (status != BehaviorStatus.Success)
                {
                    return status;
                }
                // Get the end of children successful (Can't move up)
                if (!currentChild.MoveNext())
                {
                    return BehaviorStatus.Success;
                }
            }
        }
    }
}