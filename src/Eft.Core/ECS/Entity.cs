using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Eft.Core.ECS
{
    public sealed class Entity
    {
        #region Fields
        private Guid id;
        private ICollection<Component> components;
        #endregion
        #region Properties
        [JsonProperty("id")]
        public Guid Id { get { return id; } }

        [JsonIgnore]
        public IEnumerable<Component> Components { get { return components;} }
        #endregion

        #region Constructors
        public Entity()
        {
            id = Guid.NewGuid();
            components = new List<Component>();
        }
        #endregion
        #region Methods

        public bool AddComponent(Component c)
        {

            if (components.Contains(c))
            {
                return false;
            }
            c.EntityId = Id;
            components.Add(c);
            return true;
        }
        public T GetComponent<T>() where T : Component
        {
            return (T)components.FirstOrDefault(x => x is T);
        }
        #endregion

    }
}
