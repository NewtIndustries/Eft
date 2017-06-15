using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eft.Core.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public class CustomSerializationAttribute : Attribute
    {
        
    }
}
