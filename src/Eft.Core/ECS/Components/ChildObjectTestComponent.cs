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

        [SubComponent("SubComponentTestComponent")][JsonIgnore]
        public List<SubComponentTestComponent> ChildObjects { get; set; }

        public ChildObjectTestComponent()
        {
            ChildObjects = new List<SubComponentTestComponent>();
            for (var i = 0; i < 10; i++)
            {
                ChildObjects.Add(new SubComponentTestComponent());
            }
        }

    }
}
