using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.ECS;
using Eft.Core.ECS.Components;
using Eft.Core.Engine;

namespace Eft.TestManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var begin = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                var entity = new Entity();
                entity.AddComponent(new PhysicalObjectComponent());
                entity.AddComponent(new ChildObjectTestComponent());
                Manager.Db.SaveEntity(entity);
            }
            //var es = db.LoadEntitiesWithComponent<PhysicalObjectComponent>();
            //var es = db.LoadEntitiesByComponentCriteria<PhysicalObjectComponent>(new PhysicalObjectProximityCriteria() {X = 5, Y = 13}); 
            Manager.Run().Wait();
        }
    }
}
