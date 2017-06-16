using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RethinkDb.Driver.Net.Clustering;
using Microsoft.Extensions.Configuration;
namespace Eft.Core.Configuration
{
    public class RethinkClientConfig : ConfigurationBase
    {
        
        private ICollection<string> ipAddresses;
        private IPoolingStrategy poolingStrategy;
        private bool discover;
        
        private string databaseName;

        public ICollection<string> IPAddresses { get { return ipAddresses;} }
        public IPoolingStrategy PoolingStrategy { get { return poolingStrategy; } }
        public bool Discover { get { return discover;} }
        public string DatabaseName { get { return databaseName;} }

        public RethinkClientConfig(): base("rethinksettings.json")
        {
            discover = root["discover"] == "true";
            databaseName = root["databaseName"];
            ipAddresses = new List<string>();
            foreach (var ip in root.GetSection("ipAddresses").GetChildren())
            {
                ipAddresses.Add(ip.Value);
            }
            switch (root["poolingStrategy"])
            {
                default: 
                    poolingStrategy = new RoundRobinHostPool();
                    break;
            }

        }
    }
}
