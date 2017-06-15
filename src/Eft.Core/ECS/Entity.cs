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
        //private Guid id;
        private ICollection<Component> components;
        #endregion
        #region Properties
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonIgnore]
        public IEnumerable<Component> Components { get { return components;} }
        #endregion


        private ICollection<string> implementedComponents;

        public ICollection<string> ImplementedComponents
        {
            get
            {
                implementedComponents = implementedComponents ?? Components.Select(Component.TableName).ToArray();
                return implementedComponents;
            }
            set
            {
                implementedComponents = value;
            }
        }

        #region Constructors
        public Entity()
        {
            Id = Guid.NewGuid();
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
            implementedComponents = Components.Select(Component.TableName).ToArray();
            return true;
        }
        public T GetComponent<T>() where T : Component
        {
            return (T)components.FirstOrDefault(x => x is T);
        }
        #endregion

    }
}
