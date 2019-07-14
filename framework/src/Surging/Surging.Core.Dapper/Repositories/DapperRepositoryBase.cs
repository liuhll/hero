using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Utilities;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Surging.Core.Dapper.Repositories
{
    public abstract class DapperRepositoryBase : ITransientDependency
    {

        protected virtual DbConnection GetDbConnection()
        {
            if (DbSetting.Instance == null)
            {
                var dbSetting = AppConfig.GetSection("dbSetting");
                if (!dbSetting.Exists())
                {
                    throw new DataAccessException("未对数据库进行配置");
                }

                DbSetting.Instance = new DbSetting()
                {
                    DbType = Enum.Parse<DbType>(EnvironmentHelper.GetEnvironmentVariable(AppConfig.GetSection("dbSetting:dbType").Get<string>())),
                    ConnectionString = EnvironmentHelper.GetEnvironmentVariable(AppConfig.GetSection("dbSetting:connectionString").Get<string>())
                };

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
}
