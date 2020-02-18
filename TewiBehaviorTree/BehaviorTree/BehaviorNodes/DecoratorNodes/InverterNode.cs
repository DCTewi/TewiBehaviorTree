
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    /// <summary>
    /// Node which can invert the result of child
    /// </summary>
    /// <typeparam name="T">Owner Type</typeparam>
    public class InverterNode<T> : DecoratorNode<T>
    {
        public InverterNode() : base() { }

        protected override BehaviorStatus Update()
        {
            var status = Child.Tick();
            switch (status)
            {
                case BehaviorStatus.Success:
                    {
                        return BehaviorStatus.Failure;
                    }
                case BehaviorStatus.Failure:
                    {
                        return BehaviorStatus.Success;
                    }
                default:
                    return status;
            }
        }
    }
}
