using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Eft.Core.Configuration;
using Eft.Core.ECS;
using Newtonsoft.Json.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net.Clustering;
using System = Eft.Core.ECS.System;

namespace Eft.Core.Data
{
    public class RethinkClient
    {
        #region Fields
        private RethinkDb.Driver.RethinkDB r;
        private Configuration.RethinkClientConfig config;
        private ConnectionPool pool;

        private Dictionary<string, Type> componentTypeMap;
        
        #endregion
        #region Properties
        #endregion
        #region Constructors
        public RethinkClient()
        {

            r = RethinkDB.R;
            config = new RethinkClientConfig();
            connect();
            populateComponentTypes();
           
        }
        #endregion
        #region Methods
        private void connect()
        {
            pool = r.ConnectionPool()
                .Seed(config.IPAddresses)
                .PoolingStrategy(config.PoolingStrategy)
                .Discover(config.Discover)
                .Connect();



        }

        private void populateComponentTypes()
        {
            if (!r.Db(config.DatabaseName).TableList().Contains("Entities").Run(pool))
            {
                r.Db(config.DatabaseName).TableCreate("Entities").Run(pool);
            }


            componentTypeMap = new Dictionary<string, Type>();
            foreach (var a in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            {
                var ass = Assembly.Load(a);
                foreach (var t in ass.GetTypes().Where(x => x.GetTypeInfo().IsSubclassOf(typeof(Component))))
                {
                    var tName = Component.TableName(t);
                    componentTypeMap.Add(tName, t);
                    if (!r.Db(config.DatabaseName).TableList().Contains(tName).Run<bool>(pool))
                    {
                        r.Db(config.DatabaseName).TableCreate(tName).Run(pool);
                        r.Db(config.DatabaseName).Table(tName).IndexCreate("ParentId").Run(pool);
                        var props = t.GetProperties().Where(x => x.GetCustomAttribute<SecondaryIndexAttribute>() != null);
                        foreach (var p in props)
                        {
                            r.Db(config.DatabaseName).Table(tName).IndexCreate(p.Name).Run(pool);

                        }
                    }
                }
            }


        }
        #region Entity CRUD

        public bool SaveEntity(Entity e)
        {
            r.Db(config.DatabaseName).Table("Entities").Insert(e).Run(pool);


            // Save all components to components tables separately.   Each component has its own table named what the class name is.  

            foreach (var c in e.Components.Where(x => x.Dirty))
            {
                SaveComponent(c);
            }
            e.Dirty = false;
            
            return true;
        }

        public bool SaveEntities(IEnumerable<Entity> entities)
        {
            entities = entities.ToList();
            r.Db(config.DatabaseName).Table("Entities").Insert(entities).Run(pool);
            foreach (var components in entities.Select(x => x.Components.Where(y => y.Dirty)).GroupBy(x => x.GetType()))
            {
                foreach (var v in components)
                {
                    SaveComponents(v);
                }
            }
            entities.ToList().ForEach(x => x.Dirty = false);
            return true;
        }

        public bool SaveComponent(Component c)
        {
            var tName = Component.TableName(c);
            var subProps =
                c.GetType()
                    .GetProperties()
                    .Where(prop => prop.GetCustomAttribute<SubComponentAttribute>() != null).ToList();
            if (subProps.Any())
            {
                foreach (var sp in subProps)
                {
                    SaveSubComponent(sp, c);
                }
            }

            if (c.GetType().GetTypeInfo().GetCustomAttribute<CustomSerializationAttribute>() != null)
            {
                c.Save(r, pool);
            }
            else
            {
                r.Db(config.DatabaseName).Table(tName).Insert(c).OptArg("conflict", Conflict.Update).Run(pool);
            }
            c.Dirty = false;
            return true;
        }

        public bool SaveComponents(IEnumerable<Component> components)
        {
            components = components.ToList();
            var tName = Component.TableName(components.First());
            var type = components.First().GetType();
            if (type.GetTypeInfo().GetCustomAttribute<CustomSerializationAttribute>() != null)
            {
                foreach (var c in components)
                {
                    c.Save(r, pool);
                }
            }
            else
            {
                foreach (var c in components)
                {
                    SaveComponent(c);
                }
                
            }
            return true;
        }

        public bool SaveSubComponent(PropertyInfo prop, Component component)
        {
            var tName = prop.GetCustomAttribute<SubComponentAttribute>().TableName;
            var pValue = prop.GetValue(component) as IEnumerable<Component>;
            pValue.ToList().ForEach(x => x.ParentId = component.Id);
            //if (!subComponentTableList.Contains(tName))
            //{
            //    r.Db(config.DatabaseName).TableCreate(tName).Run(pool);
            //    r.Db(config.DatabaseName).Table(tName).IndexCreate("ComponentId").Run(pool);
                
            //    subComponentTableList.Add(tName);
            //}

            r.Db(config.DatabaseName).Table(tName).Insert(pValue).OptArg("conflict", Conflict.Update).Run(pool);
            //foreach (var key in keys)
            //{
            //    r.Db(config.DatabaseName).Table(tName).Get(key).Update(new {ComponentId = component.Id}).Run(pool);
            //}


            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity LoadEntity(Guid id)
        {
            var e = r.Db(config.DatabaseName).Table("Entities").Get(id).RunResult<Entity>(pool);
            LoadEntityComponents(e);
            return e;
        }

        public IEnumerable<Entity> LoadEntities(IEnumerable<Guid> ids)
        {
            var es = r.Db(config.DatabaseName).Table("Entities").GetAll(r.Args(ids)).RunResult<IEnumerable<Entity>>(pool);
            foreach (var e in es) LoadEntityComponents(e);
            return es;
        }

        public T LoadComponent<T>(Guid id) where T : Component
        {
            return (T)LoadComponent(id, Component.TableName<T>());
        }

        public IEnumerable<T> LoadComponents<T>(ComponentCriteria<T> criteria = null) where T : Component
        {
            var tName = Component.TableName<T>();
            if (criteria != null)
            {
                var objs =
                    r.Db(config.DatabaseName)
                        .Table(tName)
                        .Filter(criteria.GetFilter())
                        .RunResult<JArray>(pool)
                        .ToObject<IEnumerable<T>>().ToList();
                var subProps =
                    (typeof(T).GetProperties().Where(x => x.GetCustomAttribute<SubComponentAttribute>() != null)).ToList();
                if (subProps != null)
                {
                    foreach (var s in subProps)
                    {
                        foreach (var o in objs)
                        {
                            s.SetValue(o, LoadSubComponents(s, o.Id));
                        }
                    }
                }

                return objs;
            }
            else
            {
                var objs =
                    r.Db(config.DatabaseName)
                        .Table(tName)
                        .RunResult<JArray>(pool)
                        .ToObject<IEnumerable<T>>().ToList();
                var subProps =
                    (typeof(T).GetProperties().Where(x => x.GetCustomAttribute<SubComponentAttribute>() != null)).ToList();
                if (subProps != null)
                {
                    foreach (var s in subProps)
                    {
                        foreach (var o in objs)
                        {
                            s.SetValue(o, LoadSubComponents(s, o.Id));
                        }
                    }
                }
                return objs;
            }
        } 
        public Component LoadComponent(Guid id, string table)
        {
            var type = componentTypeMap[table];
            var obj = r.Db(config.DatabaseName).Table(table).Filter(new { ParentId = id }).RunResult<JArray>(pool).FirstOrDefault()?.ToObject(type) as Component;

            var subProps = type.GetProperties().Where(x => x.GetCustomAttribute<SubComponentAttribute>() != null).ToList();
            if (subProps.Any())
            {
                foreach (var subProp in subProps)
                {
                    subProp.SetValue(obj, LoadSubComponents(subProp, obj.Id));
                }
            }
            return obj;

        }

        public IEnumerable<Component> LoadSubComponents(PropertyInfo prop, Guid componentId)
        {
            var table = prop.GetCustomAttribute<SubComponentAttribute>().TableName;

            var obj =
                r.Db(config.DatabaseName).Table(table).Filter(new {ParentId = componentId}).RunResult<JArray>(pool);
            var results = obj.ToObject(prop.PropertyType) as IEnumerable<Component>;

            return results;
        }
        public bool LoadEntityComponents(Entity e)
        {
            foreach (var comp in e.ImplementedComponents)
            {
                e.AddComponent(LoadComponent(e.Id, comp));
            }
            return true;
        }
        public IEnumerable<Entity> LoadEntitiesWithComponent<T>() where T : Component
        {

            var ids = r.Db(config.DatabaseName).Table(Component.TableName<T>()).GetField("ParentId").RunResult<IEnumerable<Guid>>(pool);
            return LoadEntities(ids);
        }

        public IEnumerable<Entity> LoadEntitiesByComponentCriteria<T>(ComponentCriteria<T> criteria) where T : Component
        {

            var ids =
                r.Db(config.DatabaseName)
                    .Table(Component.TableName<T>())
                    .Filter(criteria.GetFilter())
                    .GetField("ParentId")
                    .RunResult<IEnumerable<Guid>>(pool);
            return LoadEntities(ids);
        }
        #endregion
        #endregion
    }
}
