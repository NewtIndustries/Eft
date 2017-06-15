using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.Data;
using RethinkDb.Driver.Ast;

namespace Eft.Core.ECS.Components
{
    public class PhysicalObjectComponentCriteria : ComponentCriteria<PhysicalObjectComponent>
    {
        public int X;
        public int Y;
    }

    public class PhysicalObjectProximityCriteria : ComponentCriteria<PhysicalObjectComponent>
    {
        public int X;
        public int Y;

        public override ReqlFunction1 GetFilter()
        {
            return expr => expr["X"].Gt(X - 5).Lt(X + 5) && expr["Y"].Gt(Y - 5).Lt(Y + 5);
        }
    }
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
