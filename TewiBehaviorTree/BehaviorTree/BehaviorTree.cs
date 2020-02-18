namespace BehaviorTree
{
    /// <summary>
    /// Behavior Tree that Object really have
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class BehaviorTree<T>
    {
        private readonly BehaviorNodeBase<T> root;

        public BehaviorTree(BehaviorNodeBase<T> root)
        {
            this.root = root;
        }

        /// <summary>
        /// Behavior tree should call Tick() in every tick
        /// </summary>
        public void Tick()
        {
            root.Tick();
        }
    }
}