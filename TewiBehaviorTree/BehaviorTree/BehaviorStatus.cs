namespace BehaviorTree
{
    public enum BehaviorStatus : short
    {
        Invalid = 0,
        Running,
        Success,
        Failure,
        Aborted
    }
}