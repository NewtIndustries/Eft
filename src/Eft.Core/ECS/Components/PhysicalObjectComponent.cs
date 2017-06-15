using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.Data;

namespace Eft.Core.ECS.Components
{
    [TableName(Name = "PhysicalObject")]
    public class PhysicalObjectComponent : Component
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PhysicalObjectComponent()
        {
            X = 7;
            Y = 17;
        }
    }
}
