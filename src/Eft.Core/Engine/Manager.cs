using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eft.Core.Data;
using Eft.Core.ECS.Systems;
using Troschuetz.Random.Generators;

namespace Eft.Core.Engine
{
    public static class Manager
    {
        private static ICollection<ECS.System> systems;

        private static RethinkClient db;
        private static Troschuetz.Random.Generators.StandardGenerator random;
        private static CancellationTokenSource cancellationTokenSource;

        public static RethinkClient Db { get { return db; } }
        public static Troschuetz.Random.Generators.StandardGenerator Random { get { return random; } }

        static Manager()
        {
            db = new RethinkClient();
            cancellationTokenSource = new  CancellationTokenSource();
            systems = new List<ECS.System>();
            random = new StandardGenerator(0);

        }

        public static async Task Run()
        {
            systems.Add(new PhysicalObjectSystem());

            
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var start = DateTime.Now;
                foreach (var s in systems)
                {
                    s.Process();
                }    
                Console.WriteLine("Process time:" +(DateTime.Now - start).TotalMilliseconds);
                Thread.Sleep(2000);
            }

        }

        public static void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
