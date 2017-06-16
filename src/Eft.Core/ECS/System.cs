using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.ECS.Results;
using Eft.Core.Engine;
using Eft.Core.Server;

namespace Eft.Core.ECS
{
    public class System
    {

        public virtual ProcessComponentResult ProcessComponent(Component component)
        {
            return new ProcessComponentResult(ProcessComponentResultOptions.Clean);
        }

        public virtual ProcessSystemResult Process()
        {
            return new ProcessSystemResult();
        }
    }

    public class System<T> : System where T : Component
    {
        public override ProcessSystemResult Process()
        {
            var entities = GameServer.Db.LoadEntitiesWithComponent<T>().ToList();
            foreach (var e in entities)
            {
                var c = e.GetComponent<T>();
                c.Dirty = (ProcessComponent(c).Result == ProcessComponentResultOptions.Dirty);
                if (c.Dirty) e.Dirty = true;
            }
            Manager.Db.SaveEntities(entities);
            return new ProcessSystemResult();
        }

        public override ProcessComponentResult ProcessComponent(Component component)
        {
            return new ProcessComponentResult(ProcessComponentResultOptions.Clean);
        }

        public virtual ProcessComponentResult ProcessComponent(T component)
        {
            return new ProcessComponentResult(ProcessComponentResultOptions.Clean);
        }
    }
}
