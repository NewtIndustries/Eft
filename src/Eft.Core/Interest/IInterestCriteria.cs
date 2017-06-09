using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eft.Core.ECS;

namespace Eft.Core.Interest
{
    public interface IInterestCriteria
    {
        bool IsInterested(Entity source, Entity target);
    }
}
