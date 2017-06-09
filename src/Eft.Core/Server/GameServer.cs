using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.Data;

namespace Eft.Core.Server
{
    public class GameServer
    {
        protected RethinkClient db;

        public GameServer()
        {
            db = new RethinkClient();
            
        }
        public async Task  Run()
        {
            
        }
    }
}
