using BehaviorTree;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float vision;

    [Header("Debug")]
    public bool SeePlayer = false;

    private BehaviorTree<Enemy> behavior;

    private void Awake()
    {
        direction = Vector2.right;
        speed = 5f;
        vision = 3f;

        behavior = new BehaviorTreeBuilder<Enemy>(this)
            .Selector()
                .Sequence()
                    .Condition<CanSeePlayer>().Back()
                    .Action<AttackAction>().Back()
                .Back()
                .Selector()
                    .Sequence()
                        .Condition<CanTurn>().Back()
                        .Action<TurnAction>().Back()
                    .Back()
                    .Action<WanderAction>().Back()
                .Back()
            .End();

         /**
         * Behavior Tree Constuction
         * 
         * Selector
         * |-- Sequence
         * |    |-- CanSeePlayer
         * |    |-- Repeat
         * |    |   `-- AttackAction
         * |    `-- Until
         * |        `-- IsEscaped
         * `-- Selector
         *      |-- Sequence
         *      |   |-- CanTurn
         *      |   `-- TurnAction
         *      `--WanderAction
         */
        //behavior = new BehaviorTreeBuilder<Enemy>(this)
        //    .Selector()
        //        .Sequence()
        //            .Condition<CanSeePlayer>().Back()
        //            .Decorator<RepeatNode<Enemy>>(5)
        //                .Action<AttackAction>().Back()
        //            .Back()
        //            .Decorator<UntilNode<Enemy>>()
        //                .Condition<IsEscaped>().Back()
        //            .Back()
        //        .Back()
        //        .Selector()
        //            .Sequence()
        //                .Condition<CanTurn>().Back()
        //                .Action<TurnAction>().Back()
        //            .Back()
        //            .Action<WanderAction>().Back()
        //        .Back()
        //    .End();

        //behavior = new BehaviorTreeBuilder<Enemy>()
        //    .AddBehavior(new SelectorNode<Enemy>(this))
        //        .AddBehavior(new SequenceNode<Enemy>(this))
        //            .AddBehavior(new CanSeePlayer(this, false))
        //                .Back()
        //            .AddBehavior(new AttackBehavior(this))
        //                .Back()
        //            .Back()
        //        .AddBehavior(new SelectorNode<Enemy>(this))
        //            .AddBehavior(new SequenceNode<Enemy>(this))
        //                .AddBehavior(new CanTurn(this, false))
        //                    .Back()
        //                .AddBehavior(new TurnBehavior(this))
        //                    .Back()
        //                .Back()
        //            .AddBehavior(new WanderBehavior(this))
        //                .Back()
        //            .Back()
        //        .Back()
        //    .End();
    }

    private void Update()
    {
        behavior.Tick();
    }

    private class CanSeePlayer : ConditionNode<Enemy>
    {
        public CanSeePlayer(Enemy Owner) : base(Owner) { }
        protected override bool Condition()
        {
            Debug.Log("Check player");
            float dis = (Player.Instance.transform.position - owner.transform.position).magnitude;

            return owner.SeePlayer = dis - owner.vision < 0f;
        }
    }

    private class CanTurn : ConditionNode<Enemy>
    {
        public CanTurn(Enemy Owner) : base(Owner) { }
        protected override bool Condition()
        {
            return Random.Range(0, 100) < 10;
        }
    }

    private class IsEscaped : ConditionNode<Enemy>
    {
        public IsEscaped(Enemy owner) : base(owner) { }
        protected override bool Condition()
        {
            Debug.Log("Yingyingying");
            float dis = (Player.Instance.transform.position - owner.transform.position).magnitude;

            return dis > owner.vision;
        }
    }

    private class TurnAction : ActionNode<Enemy>
    {
        public TurnAction(Enemy Owner) : base(Owner) { }
        protected override BehaviorStatus Update()
        {
            //Debug.Log("Turning...");
            owner.direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            return BehaviorStatus.Success;
        }
    }

    private class WanderAction : ActionNode<Enemy>
    {
        public WanderAction(Enemy Owner) : base(Owner) { }
        protected override BehaviorStatus Update()
        {
            Debug.Log("Wandering...");
            owner.transform.Translate(owner.direction * owner.speed * Time.deltaTime);
            return BehaviorStatus.Success;
        }
    }

    private class AttackAction : ActionNode<Enemy>
    {
        public AttackAction(Enemy Owner) : base(Owner) { }
        protected override BehaviorStatus Update()
        {
            Debug.Log("Attacking!");
            return BehaviorStatus.Success;
        }
    }
}
