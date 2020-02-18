namespace BehaviorTree
{
    /// <summary>
    /// Base Class of Behavior Nodes
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public abstract class BehaviorNodeBase<T>
    {
        public BehaviorNodeBase()
        {
            Status = BehaviorStatus.Invalid;
        }

        public BehaviorStatus Status { get; protected set; }

        /// <summary>
        /// Behavior tree should run this funtion per tick
        /// </summary>
        /// <returns>Node Status after this tick</returns>
        public BehaviorStatus Tick()
        {
            if (Status != BehaviorStatus.Running) Initialize();

            Status = Update();

            if (Status != BehaviorStatus.Running) Terminate();

            return Status;
        }

        /// <summary>
        /// Reset this node status to Invalid
        /// </summary>
        public void Reset() => Status = BehaviorStatus.Invalid;

        /// <summary>
        /// Abort this node
        /// </summary>
        public void Abort()
        {
            Status = BehaviorStatus.Aborted;
            Terminate();
        }

        public virtual void AddChild(BehaviorNodeBase<T> child) { }

        /// <summary>
        /// Funtion which should be called on initialization
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// What this node will do in every tick, 
        /// **The returned status will be the Status of this Node**
        /// </summary>
        /// <returns>Status after this tick</returns>
        protected abstract BehaviorStatus Update();

        /// <summary>
        /// Funtion which should be called on terminatation
        /// </summary>
        protected virtual void Terminate() { }
    }
}