using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eft.Core.ECS.Results
{
    public enum ProcessComponentResultOptions
    {
        Clean,
        Dirty,
        Removed
          
    }
    public class ProcessComponentResult
    {
        public ProcessComponentResultOptions Result { get; set; }

        public ProcessComponentResult(ProcessComponentResultOptions state)
        {
            Result = state;
        }

    }
}
