using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.ECS.Components;
using Eft.Core.ECS.Results;
using Eft.Core.Engine;

namespace Eft.Core.ECS.Systems
{
    public class PhysicalObjectSystem : System<PhysicalObjectComponent>
    {
        public override ProcessComponentResult ProcessComponent(PhysicalObjectComponent component)
        {
            var difX = Manager.Random.Next(-2, 2);
            var difY = Manager.Random.Next(-2, 2);

            component.X += difX;
            component.Y += difY;
            return new ProcessComponentResult(ProcessComponentResultOptions.Dirty);
        }
    }
}
