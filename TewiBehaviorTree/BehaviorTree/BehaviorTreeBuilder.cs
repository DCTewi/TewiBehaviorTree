using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Builder interface to coders to build a Behavior Tree
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class BehaviorTreeBuilder<T>
    {
        private readonly Stack<BehaviorNodeBase<T>> stack = new Stack<BehaviorNodeBase<T>>();
        private BehaviorNodeBase<T> root;
        private readonly T owner;

        public BehaviorTreeBuilder(T Owner)
        {
            owner = Owner;
        }

        /// <summary>
        /// Add a Sequence Node
        /// </summary>
        public BehaviorTreeBuilder<T> Sequence() => AddBehavior(new SequenceNode<T>());

        /// <summary>
        /// Add a Selector Node
        /// </summary>
        public BehaviorTreeBuilder<T> Selector() => AddBehavior(new SelectorNode<T>());

        /// <summary>
        /// Add a Parallel Node
        /// </summary>
        /// <param name="successPolicy">Policy to success(High level)</param>
        /// <param name="failurePolicy">Policy to failure</param>
        public BehaviorTreeBuilder<T> Parallel(BehaviorPolicy successPolicy, BehaviorPolicy failurePolicy)
        {
            return AddBehavior(new ParallelNode<T>(successPolicy, failurePolicy));
        }

        /// <summary>
        /// Add a Decorator Node
        /// </summary>
        /// <typeparam name="TDecoratorNode">Decorator Type</typeparam>
        public BehaviorTreeBuilder<T> Decorator<TDecoratorNode>() where TDecoratorNode : DecoratorNode<T>
        {
            return AddBehavior(System.Activator.CreateInstance(typeof(TDecoratorNode)) as TDecoratorNode);
        }

        /// <summary>
        /// Add a Decorator Node with custom args
        /// </summary>
        /// <typeparam name="TDecoratorNode">Decorator Type</typeparam>
        /// <param name="args">Custom Args</param>
        public BehaviorTreeBuilder<T> Decorator<TDecoratorNode>(params object[] args) where TDecoratorNode : DecoratorNode<T>
        {
            return AddBehavior(System.Activator.CreateInstance(typeof(TDecoratorNode), args) as TDecoratorNode);
        }

        /// <summary>
        /// Add a Action Node
        /// </summary>
        /// <typeparam name="TAction">Action Type</typeparam>
        public BehaviorTreeBuilder<T> Action<TAction>() where TAction : ActionNode<T>
        {
            return AddBehavior(System.Activator.CreateInstance(typeof(TAction), owner) as TAction);
        }

        /// <summary>
        /// Add a Action Node with custom args, **Custom Args must after owner**
        /// </summary>
        /// <typeparam name="TAction">Action Type</typeparam>
        /// <param name="args">Custom Args</param>
        /// <returns></returns>
        public BehaviorTreeBuilder<T> Action<TAction>(params object[] args) where TAction : ActionNode<T>
        {
            return AddBehavior(System.Activator.CreateInstance(typeof(TAction), owner, args) as TAction);
        }

        /// <summary>
        /// Add a Condition Node
        /// </summary>
        /// <typeparam name="TCondition">Condition Type</typeparam>
        /// <param name="Inverted">Is this condition inverted</param>
        public BehaviorTreeBuilder<T> Condition<TCondition>() where TCondition : ConditionNode<T>
        {
            return AddBehavior(System.Activator.CreateInstance(typeof(TCondition), owner) as TCondition);
        }

        /// <summary>
        /// Add a Condition Node with custom args, **Custom Args must after owner**
        /// </summary>
        /// <typeparam name="TCondition">Condition Type</typeparam>
        /// <param name="Inverted">Is this condition inverted</param>
        /// <param name="args">Custom Args</param>
        public BehaviorTreeBuilder<T> Condition<TCondition>(params object[] args) where TCondition : ConditionNode<T>
        {
            return AddBehavior(System.Activator.CreateInstance(typeof(TCondition), owner, args) as TCondition);
        }

        /// <summary>
        /// Backtrack from this level
        /// </summary>
        public BehaviorTreeBuilder<T> Back()
        {
            stack.Pop();
            return this;
        }

        /// <summary>
        /// End of building, get the tree
        /// </summary>
        /// <returns>Tree</returns>
        public BehaviorTree<T> End()
        {
            while (stack.Count > 0)
            {
                stack.Pop();
            }
            return new BehaviorTree<T>(root);
        }

        /// <summary>
        /// Builder stack management
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        private BehaviorTreeBuilder<T> AddBehavior(BehaviorNodeBase<T> behavior)
        {
            // No root
            if (root == null)
            {
                root = behavior;
            }
            // Add child to top
            else
            {
                stack.Peek().AddChild(behavior);
            }

            // Push into stack
            stack.Push(behavior);
            return this;
        }
    }
}
