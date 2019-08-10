using Autofac;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Surging.Core.CPlatform.DependencyResolution;
using Surging.Core.CPlatform.EventBus.Events;
using Surging.Core.CPlatform.EventBus.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Surging.Core.Dapper.Manager
{
    public abstract class ManagerBase 
    {
 
        protected virtual DbConnection Connection
        {
            get
            {
                if (DbSetting.Instance == null)
                {
                    throw new Exception("未设置数据库连接");
                }
                DbConnection conn = null;
                switch (DbSetting.Instance.DbType)
                {
                    case DbType.MySql:
                        conn = new MySqlConnection(DbSetting.Instance.ConnectionString);
                        break;
                    case DbType.Oracle:
                        conn = new OracleConnection(DbSetting.Instance.ConnectionString);
                        break;
                    case DbType.SqlServer:
                        conn = new SqlConnection(DbSetting.Instance.ConnectionString);
                        break;
                }
                conn.Open();
                return conn;
            }
        }

        protected virtual void UnitOfWork(Action<DbConnection, DbTransaction> action, DbConnection conn)
        {
            var trans = conn.BeginTransaction();
            try
            {
                action(conn, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        protected virtual async Task UnitOfWorkAsync(Func<DbConnection, DbTransaction, Task> action, DbConnection conn)
        {
            var trans = conn.BeginTransaction();
            try
            {
                await action(conn, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        public virtual T GetService<T>() where T : class
        {
            if (ServiceLocator.Current.IsRegistered<T>())
            {
                return ServiceLocator.GetService<T>();
            }
            else
            {
                var result = ServiceResolver.Current.GetService<T>();
                if (result == null) {
                    result = ServiceLocator.GetService<IServiceProxyFactory>().CreateProxy<T>();
                    ServiceResolver.Current.Register(null,result);
                }
                return result;
            }

            
        }

        public virtual T GetService<T>(string key) where T : class
        {
            if (ServiceLocator.Current.IsRegisteredWithKey<T>(key))
            {
                return ServiceLocator.GetService<T>(key);
            }
            else
            {
                var result = ServiceResolver.Current.GetService<T>(key);
                if (result == null)
                {
                    result = ServiceLocator.GetService<IServiceProxyFactory>().CreateProxy<T>(key);
                    ServiceResolver.Current.Register(key, result);
                }
                return result;
            }
        }

        public virtual object GetService(Type type)
        {
            if (ServiceLocator.Current.IsRegistered(type))
            {
                return ServiceLocator.GetService(type);
            }
            else
            {
                var result = ServiceResolver.Current.GetService(type);
                if (result == null)
                {
                    result = ServiceLocator.GetService<IServiceProxyFactory>().CreateProxy(type);
                    ServiceResolver.Current.Register(null, result);
                }
                return result;
            }
        }

        public virtual object GetService(string key, Type type)
        {
            if (ServiceLocator.Current.IsRegisteredWithKey(key,type))
            {
                return ServiceLocator.GetService(key, type);
            }
            else
            {
                var result = ServiceResolver.Current.GetService(type);
                if (result == null)
                {
                    result = ServiceLocator.GetService<IServiceProxyFactory>().CreateProxy(type);
                    ServiceResolver.Current.Register(key, result);
                }
                return result;
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            GetService<IEventBus>().Publish(@event);
        }
    }
}
