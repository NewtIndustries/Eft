using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.Engine;

namespace Eft.TestManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Manager.Run().Wait();
        }
    }
}
