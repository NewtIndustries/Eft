using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Eft.Core.ECS
{
    public class Component
    {
        private Guid id;
        [JsonProperty("id")]
        public Guid Id { get { return id; } }
        internal Guid EntityId { get; set; }
        


        public Component()
        {
            id = Guid.NewGuid();
        }
    }
}
