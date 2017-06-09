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
    public class RethinkClientConfig 
    {
        
        private IEnumerable<IPAddress> ipAddresses;
        private IPoolingStrategy poolingStrategy;
        private bool discover;
        private IConfigurationRoot root;

        public RethinkClientConfig()
        {
            var builder =
                new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("rethinksettings.json");
            root = builder.Build();
        }
    }
}
