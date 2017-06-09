using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Net.Clustering;

namespace Eft.Core.Data
{
    public class RethinkClient
    {
        private RethinkDb.Driver.RethinkDB r;
        private Configuration.RethinkClientConfig config;
        private ConnectionPool pool;
        public RethinkClient()
        {
            r = RethinkDB.R;
            connect();
        }

        private void connect()
        {
            pool = r.ConnectionPool()
                .Seed(new[] {"127.0.0.1"})
                .PoolingStrategy(new RoundRobinHostPool())
                .Discover(true)
                .Connect();

            //var output = r.Db("test").Table("chat").Limit(10).RunResult<JArray>(pool);

        }

    }
}
