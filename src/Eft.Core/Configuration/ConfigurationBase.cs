using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Eft.Core.Configuration
{
    public class ConfigurationBase
    {
        protected IConfigurationRoot root;

        public ConfigurationBase(string configurationFileName)
        {
            var builder =
                new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(configurationFileName);
            root = builder.Build();
        }
    }
}
