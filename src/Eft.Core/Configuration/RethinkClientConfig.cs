using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RethinkDb.Driver.Net.Clustering;

namespace Eft.Core.Configuration
{
    public class RethinkClientConfig
    {
        private IEnumerable<IPAddress> ipAddresses;
        private IPoolingStrategy poolingStrategy;
        private bool discover;


        public RethinkClientConfig()
        {
            
        }
    }
}
