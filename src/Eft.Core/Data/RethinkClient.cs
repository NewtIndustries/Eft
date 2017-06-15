using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Eft.Core.Configuration;
using Eft.Core.ECS;
using Newtonsoft.Json.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Net.Clustering;

namespace Eft.Core.Data
{
    public class RethinkClient
    {
        #region Fields
        private RethinkDb.Driver.RethinkDB r;
        private Configuration.RethinkClientConfig config;
        private ConnectionPool pool;
        #endregion
        #region Properties
        #endregion
        #region Constructors
        public RethinkClient()
        {
            r = RethinkDB.R;
            config = new RethinkClientConfig();
            connect();
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

            //var output = r.Db("test").Table("chat").Limit(10).RunResult<JArray>(pool);

        }
        #region Entity CRUD

        public bool SaveEntity(Entity e)
        {
            r.Db(config.DatabaseName).Table("Entities").Insert(e).Run(pool);
            

            // Save all components to components tables separately.   Each component has its own table named what the class name is.  

            foreach (var c in e.Components)
            {
                var type = c.GetType();
                var attr = type.GetTypeInfo().GetCustomAttribute<TableNameAttribute>();
                string tName = "";
                if (attr != null)
                {
                    tName = attr.Name;
                }
                else
                {
                    tName = type.Name;
                }
                if (!r.Db(config.DatabaseName).TableList().Contains(tName).Run<bool>(pool))
                {
                    r.Db(config.DatabaseName).TableCreate(tName).Run(pool);
                }
                r.Db(config.DatabaseName).Table(tName).Insert(c).Run(pool);
            }


            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity LoadEntity(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entity> LoadEntitiesWithComponent<T>() where T : Component
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
