using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eft.TestServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var server = new Eft.Core.Server.GameServer();
            server.Run().Wait();
        }
    }
}
