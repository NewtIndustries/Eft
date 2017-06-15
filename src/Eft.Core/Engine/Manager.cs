using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eft.Core.Data;

namespace Eft.Core.Engine
{
    public static class Manager
    {

        private static RethinkClient db;
        private static CancellationTokenSource cancellationTokenSource;
        static Manager()
        {
            db = new RethinkClient();
            cancellationTokenSource = new  CancellationTokenSource();
        }

        public static async Task Run()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                
            }

        }

        public static void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
