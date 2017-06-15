using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Eft.Core.Data;
using Newtonsoft.Json;
using RethinkDb.Driver;
using RethinkDb.Driver.Net.Clustering;

namespace Eft.Core.ECS
{
    public class Component
    {
        
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public static string TableName(Type t)
        {
            string tName = "";
            var attr = t.GetTypeInfo().GetCustomAttribute<TableNameAttribute>();
            if (attr != null)
            {
                tName = attr.Name;
            }
            else
            {
                tName = t.Name;

            }
            return tName;
        }
        public static string TableName<T>(T component = null) where T : Component
        {
            string tName = "";
            if (component != null)
            {
                var attr = component.GetType().GetTypeInfo().GetCustomAttribute<TableNameAttribute>();
                if (attr != null)
                {
                    tName = attr.Name;
                }
                else
                {
                    tName = component.GetType().Name;

                }
            }
            else
            {
                var attr = typeof(T).GetTypeInfo().GetCustomAttribute<TableNameAttribute>();
                if (attr != null)
                {
                    tName = attr.Name;
                }
                else
                {
                    tName = typeof(T).Name;

                }
            }
            return tName;
        }

        public Component()
        {
            Id = Guid.NewGuid();
        }

        public virtual void Save(RethinkDB r, ConnectionPool pool)
        {
            throw new NotImplementedException();
        }

    }
}
