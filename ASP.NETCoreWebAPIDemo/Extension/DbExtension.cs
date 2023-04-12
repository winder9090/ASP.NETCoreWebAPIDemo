using Microsoft.Extensions.Configuration;
using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;

namespace ASP.NETCoreWebAPIDemo.Extension
{
    public static class DbExtension
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //全部数据权限
        public static string DATA_SCOPE_ALL = "1";
        //自定数据权限
        public static string DATA_SCOPE_CUSTOM = "2";
        //部门数据权限
        public static string DATA_SCOPE_DEPT = "3";
        //部门及以下数据权限
        public static string DATA_SCOPE_DEPT_AND_CHILD = "4";
        //仅本人数据权限
        public static string DATA_SCOPE_SELF = "5";

        public static void AddDb(IConfiguration Configuration)
        {
            string connStr = Configuration.GetConnectionString("conn_db");
            int dbType = Convert.ToInt32(Configuration.GetConnectionString("conn_db_type"));

            var iocList = new List<IocConfig>() {
                   new IocConfig() {
                    ConfigId = "0",//默认db
                    ConnectionString = connStr,
                    DbType = (IocDbType)dbType,
                    IsAutoCloseConnection = true
                },
                   new IocConfig() {
                    ConfigId = "1",
                    ConnectionString = connStr,
                    DbType = (IocDbType)dbType,
                    IsAutoCloseConnection = true
                }
                   //...增加其他数据库
                };
            SugarIocServices.AddSqlSugar(iocList);
            SugarIocServices.ConfigurationSugar(db =>
            {
                //db0数据过滤
                FilterData(0);

                iocList.ForEach(iocConfig =>
                {
                    SetSugarAop(db, iocConfig);
                });
            });
        }

        private static void SetSugarAop(SqlSugarClient db, IocConfig iocConfig)
        {
            var config = db.GetConnection(iocConfig.ConfigId).CurrentConnectionConfig;

            string configId = config.ConfigId;
            db.GetConnectionScope(configId).Aop.OnLogExecuting = (sql, pars) =>
            {
                string log = $"【db{configId} SQL语句】{UtilMethods.GetSqlString(config.DbType, sql, pars)}\n";
                if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                    logger.Warn(log);
                else if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("TRUNCATE", StringComparison.OrdinalIgnoreCase))
                    logger.Error(log);
                else
                    logger.Info(log);
            };

            db.GetConnectionScope(configId).Aop.OnError = (e) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                logger.Error(e, $"执行SQL出错：{e.Message}");
            };
        }

        /// <summary>
        /// 数据过滤
        /// </summary>
        /// <param name="configId">多库id</param>
        private static void FilterData(int configId)
        {

        }
    }
}
