using Autofac;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Microsoft.Extensions.Configuration;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Dapper.Filters.Action;
using Surging.Core.Dapper.Filters.Query;
using Surging.Core.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.Dapper
{
    public static class ContainerBuilderExtensions
    {
        public static IServiceBuilder AddDapperRepository(this IServiceBuilder builder)
        {
            builder.Services.RegisterGeneric(typeof(DapperRepository<,>)).As(typeof(IDapperRepository<,>)).InstancePerDependency();
            builder.Services.RegisterGeneric(typeof(CreationAuditDapperActionFilter<,>)).Named(typeof(CreationAuditDapperActionFilter<,>).Name,typeof(IAuditActionFilter<,>)).InstancePerDependency();
            builder.Services.RegisterGeneric(typeof(ModificationAuditDapperActionFilter<,>)).Named(typeof(ModificationAuditDapperActionFilter<,>).Name, typeof(IAuditActionFilter<,>)).InstancePerDependency();
            builder.Services.RegisterGeneric(typeof(DeletionAuditDapperActionFilter<,>)).Named(typeof(DeletionAuditDapperActionFilter<,>).Name, typeof(IAuditActionFilter<,>)).InstancePerDependency();
            builder.Services.RegisterType<SoftDeleteQueryFilter>().As<ISoftDeleteQueryFilter>().AsSelf().InstancePerDependency();
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(ClassMapper<>);

            var dbSettingSection = AppConfig.GetSection("dbSetting");
            if (!dbSettingSection.Exists())
            {
                throw new DataAccessException("未对数据库进行配置");
            }

            var dbSetting = new DbSetting()
            {
                DbType = Enum.Parse<DbType>(EnvironmentHelper.GetEnvironmentVariable(AppConfig.GetSection("dbSetting:dbType").Get<string>())),
                ConnectionString = EnvironmentHelper.GetEnvironmentVariable(AppConfig.GetSection("dbSetting:connectionString").Get<string>())
            };

            DbSetting.Instance = dbSetting;
            switch (dbSetting.DbType)
            {
                case DbType.MySql:
                    DapperExtensions.DapperExtensions.SqlDialect = new MySqlDialect();
                    break;
                case DbType.Oracle:
                    DapperExtensions.DapperExtensions.SqlDialect = new OracleDialect();
                    break;
                case DbType.SqlServer:
                    DapperExtensions.DapperExtensions.SqlDialect = new SqlServerDialect();
                    break;

            }
            return builder;
        }
    }
}
