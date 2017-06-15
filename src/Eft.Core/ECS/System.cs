using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.ECS.Results;

namespace Eft.Core.ECS
{
    public class System
    {

        public virtual ProcessComponentResult ProcessComponent<T>(T component) where T : Component
        {
            return new ProcessComponentResult();
        }

        public virtual ProcessSystemResult Process()
        {
            return new ProcessSystemResult();
        }
    }

    public class System<T> : System where T : Component
    {
        public override ProcessComponentResult ProcessComponent<T>(T component)
        {
            return new ProcessComponentResult();
        }
    }
}
