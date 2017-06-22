using Newtonsoft.Json;
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
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomSerializationAttribute : Attribute
    {
        
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class SecondaryIndexAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Property)]
    
    public class SubComponentAttribute: Attribute
    {
        public string TableName;

        public SubComponentAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
