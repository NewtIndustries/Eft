using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eft.Core.Data;
using Eft.Core.ECS;
using Eft.Core.ECS.Components;
using RethinkDb.Driver.Ast;

namespace Eft.Core.Server
{
    public class GameServer
    {
        protected static RethinkClient db;
        protected static CancellationTokenSource cancellationTokenSource;

        public static RethinkClient Db { get { return db; } }

        static GameServer()
        {
            db = new RethinkClient();
            cancellationTokenSource = new CancellationTokenSource();
            
        }
        public static async Task  Run()
        {
            //var begin = DateTime.Now;
            //for (int i = 0; i < 10; i++)
            //{
            //    var entity = new Entity();
            //    entity.AddComponent(new PhysicalObjectComponent());
            //    db.SaveEntity(entity);
            //}
            //var es = db.LoadEntitiesWithComponent<PhysicalObjectComponent>();
            var es = db.LoadEntitiesByComponentCriteria<PhysicalObjectComponent>(new PhysicalObjectProximityCriteria() {X = 3, Y = 1111}); 
            while (!cancellationTokenSource.IsCancellationRequested)
            {

                // Do Work.
            }
        }

        public static void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
