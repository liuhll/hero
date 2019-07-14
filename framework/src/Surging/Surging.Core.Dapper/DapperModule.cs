using Autofac;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Microsoft.Extensions.Configuration;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Module;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Dapper.Filters.Action;
using Surging.Core.Dapper.Filters.Query;
using Surging.Core.Dapper.Repositories;
using System;

namespace Surging.Core.Dapper
{
    public class DapperModule : EnginePartModule
    {
        public override void Initialize(AppModuleContext context)
        {
            base.Initialize(context);
        }

        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
            base.RegisterBuilder(builder);
            builder.RegisterGeneric(typeof(DapperRepository<,>)).As(typeof(IDapperRepository<,>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(CreationAuditDapperActionFilter<,>)).Named(typeof(CreationAuditDapperActionFilter<,>).Name, typeof(IAuditActionFilter<,>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(ModificationAuditDapperActionFilter<,>)).Named(typeof(ModificationAuditDapperActionFilter<,>).Name, typeof(IAuditActionFilter<,>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(DeletionAuditDapperActionFilter<,>)).Named(typeof(DeletionAuditDapperActionFilter<,>).Name, typeof(IAuditActionFilter<,>)).InstancePerDependency();
            builder.RegisterType<SoftDeleteQueryFilter>().As<ISoftDeleteQueryFilter>().AsSelf().InstancePerDependency();
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
        }
    }
}
