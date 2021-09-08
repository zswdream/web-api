using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace SwitDish.Common.Logger
{
    public class LoggerManager : Interfaces.ILoggerManager
    {
        private static LoggingConfiguration NLogConfiguration
        {
            get
            {
                LoggingConfiguration nlog_config = new LoggingConfiguration();
                
                // Log to Database
                DatabaseTarget dbTarget = new DatabaseTarget(name: "database")
                {
                    ConnectionString = Configuration.GetConnectionString("SwitDishDatabase"),
                    CommandText = "insert into AppNLogs ([TimeStamp],[Level],Logger, [Message], UserId, Exception, StackTrace) values (@TimeStamp, @Level, @Logger, @Message, case when len(@UserID) = 0 then null else @UserId end, @Exception, @StackTrace);",
                    DBProvider = "System.Data.SqlClient"
                };
                dbTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@TimeStamp", Layout = new SimpleLayout("${date}") });
                dbTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@Level", Layout = new SimpleLayout("${level}") });
                dbTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@Logger", Layout = new SimpleLayout("${logger}") });
                dbTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@Message", Layout = new SimpleLayout("${message}") });
                dbTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@UserId", Layout = new SimpleLayout("${aspnet-user-identity}") });
                dbTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@Exception", Layout = new SimpleLayout("${exception}") });
                dbTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@StackTrace", Layout = new SimpleLayout("${stacktrace}") });

                LoggingRule dbLoggingRule = new LoggingRule("*", LogLevel.Trace, dbTarget);
                nlog_config.LoggingRules.Add(dbLoggingRule);

                // Log to Email
                MailTarget emailTarget = new MailTarget
                {
                    SmtpServer = Configuration["NLog_Email:SmtpServer"],
                    SmtpPort = Convert.ToInt32(Configuration["NLog_Email:SmtpPort"]),
                    SmtpUserName = Configuration["NLog_Email:SmtpUserName"],
                    SmtpPassword = Configuration["NLog_Email:SmtpPassword"],
                    EnableSsl = true,
                    From = Configuration["NLog_Email:From"],
                    To = Configuration["NLog_Email:To"],
                    Subject = Configuration["NLog_Email:Subject"],
                    SmtpAuthentication = SmtpAuthenticationMode.Basic
                };

                LoggingRule emailLogginRule = new LoggingRule("*", LogLevel.Error, emailTarget);
                nlog_config.LoggingRules.Add(emailLogginRule);

                return nlog_config;
            }
        }
        private static IConfiguration Configuration
        {
            get
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();
                return configuration;
            }
        } 
        private static ILogger logger
        {
            get
            {
                //InternalLogger.LogFile = "nlog.text";
                return NLog.Web.NLogBuilder.ConfigureNLog(NLogConfiguration).GetCurrentClassLogger();
            }
        }
        
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        public void LogError(string message)
        {
            logger.Error(message);
        }
        public void LogInfo(string message)
        {
            logger.Info(message);
        }
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

    }
}
