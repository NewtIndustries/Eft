using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.Data;
using Newtonsoft.Json;

namespace Eft.Core.ECS.Components
{
    [TableName(Name = "ChildObjectTest")]
    public class ChildObjectTestComponent : Component
    {
        public int SomeValue { get; set; }

        [HasOwnTable("SubObjectTest")][JsonIgnore]
        public List<object> ChildObjects { get; set; }

        public ChildObjectTestComponent()
        {
            SomeValue = 73;
            ChildObjects = new List<object>();
            for (var i = 0; i < 10; i++)
            {
                ChildObjects.Add(new {Some = "Data"});
            }
        }

    }
}
