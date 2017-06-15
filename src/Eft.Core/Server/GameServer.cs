using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eft.Core.Data;
using Eft.Core.ECS;
using Eft.Core.ECS.Components;

namespace Eft.Core.Server
{
    public class GameServer
    {
        protected RethinkClient db;
        protected CancellationTokenSource cancellationTokenSource;


        public GameServer()
        {
            db = new RethinkClient();
            cancellationTokenSource = new CancellationTokenSource();
            
        }
        public async Task  Run()
        {
            //var begin = DateTime.Now;
            //for (int i = 0; i < 10000; i++)
            //{
            //    var entity = new Entity();
            //    entity.AddComponent(new PhysicalObjectComponent());
            //    db.SaveEntity(entity);
            //}
            //Console.WriteLine("Saved 1000 entities in " + (DateTime.Now - begin).TotalSeconds + " seconds.");
            while (!cancellationTokenSource.IsCancellationRequested)
            {

                // Do Work.
            }
        }
    }
}
