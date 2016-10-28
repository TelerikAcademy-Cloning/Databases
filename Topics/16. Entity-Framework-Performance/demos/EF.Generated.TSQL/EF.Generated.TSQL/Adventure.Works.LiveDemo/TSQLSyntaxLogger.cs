using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.IO;

namespace Adventure.Works.LiveDemo
{
    public class TSQLSyntaxLogger : IDbCommandInterceptor
    {
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            this.OnHandlerCalled();
            this.LogTSQL("NonQueryExecuting", $" IsAsync: {interceptionContext.IsAsync} / Command Text: {Environment.NewLine}{command.CommandText}");
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            this.OnHandlerCalled();
            this.LogTSQL("NonQueryExecuted", $" IsAsync: {interceptionContext.IsAsync} / Command Text: {Environment.NewLine}{command.CommandText}");
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            this.OnHandlerCalled();
            this.LogTSQL("ReaderExecuting", $" IsAsync: {interceptionContext.IsAsync} / Command Text: {Environment.NewLine}{command.CommandText}");
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            this.OnHandlerCalled();
            this.LogTSQL("ReaderExecuted", $" IsAsync: {interceptionContext.IsAsync} / Command Text: {Environment.NewLine}{command.CommandText}");
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            this.OnHandlerCalled();
            this.LogTSQL("ScalarExecuting", $" IsAsync: {interceptionContext.IsAsync} / Command Text: {Environment.NewLine}{command.CommandText}");
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            this.OnHandlerCalled();
            this.LogTSQL("ScalarExecuted", $" IsAsync: {interceptionContext.IsAsync} / Command Text: {Environment.NewLine}{command.CommandText}");
        }

        private void OnHandlerCalled()
        {
            this.LogSeparatorLine();
            this.LogExecutionDatetime();
        }

        private void Log(string line)
        {
            var appendData = true;

            using (var fileWriter = new StreamWriter(@"..\..\data\EFGenerated.TSQL.txt", appendData))
            {
                fileWriter.WriteLine(line);
            }
        }

        private void LogTSQL(string command, string commandText)
        {
            var line = $"Intercepted on : {command} : - {commandText}";
            this.Log(line);
        }

        private void LogSeparatorLine()
        {
            string separatorLine = new string('=', 30);
            this.Log(separatorLine);
        }

        private void LogExecutionDatetime()
        {
            this.Log($"Executed on: [{DateTime.Now.ToString()}]");
        }
    }
}
