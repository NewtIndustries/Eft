using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Driver.Ast;

namespace Eft.Core.ECS
{
    public class ComponentCriteria<T> where T : Component
    {
       
        public virtual ReqlFunction1 GetFilter()
        {
            return expr => this;

        }
    }
}
