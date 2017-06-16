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
            /// I'm pretty sure this is nonsensical to the GameServer's purpose.  It should have no timing based loops.  Just proxying objects/commands between players and engine.
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
