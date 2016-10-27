using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Common;

namespace EntityFramework.Performance.Demos
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            //QueryOne();
            //QueryTwo();
            //QueryThree();
            //QueryFour();
            //QueryFive();
            //QuerySix();

            // Avoid nesting LINQ queries in loops at all cost!
            // This almost always leads to performance bottlenecks

            // Single() vs First()

            // Always have in mind the "N+1" problem, when accessing navigation properties
            // i.e. Person.Address.Country.Town.Street
        }

        public static void QueryOne()
        {
            using (var dbContext = new AdventureWorks2014Entities())
            {
                Console.WriteLine("Enter last name to search: ");
                var lastName = Console.ReadLine();

                var people = dbContext.People.Select(x => x);

                foreach (var person in people)
                {
                    if (person.LastName == lastName)
                    {
                        Console.WriteLine(person.BusinessEntityID);
                    }
                }
            }
        }

        /// <summary>
        /// Filter on the server in order to utilize the heavy mathematics and algorithms that work under the hood of SQL Server
        /// Also to retrieve less data from the server, because that is an unnecessary overhead
        /// </summary>
        public static void QueryTwo()
        {
            using (var dbContext = new AdventureWorks2014Entities())
            {
                Console.WriteLine("Enter last name to search: ");
                var lastName = Console.ReadLine();

                var people = dbContext.People
                    .Where(x => x.LastName == lastName)
                    .Select(x => x);

                foreach (var person in people)
                {
                    Console.WriteLine(person.BusinessEntityID);
                }
            }
        }

        /// <summary>
        /// Using a projection to retrieve less data as possible from the server
        /// </summary>
        public static void QueryThree()
        {
            using (var dbContext = new AdventureWorks2014Entities())
            {
                Console.WriteLine("Enter last name to search: ");
                var lastName = Console.ReadLine();

                var peopleBusinessEntityIDs = dbContext.People
                    .Where(x => x.LastName == lastName)
                    .Select(x => x.BusinessEntityID);

                foreach (var businessEntityID in peopleBusinessEntityIDs)
                {
                    Console.WriteLine(businessEntityID);
                }
            }
        }

        /// <summary>
        /// Not a set based operation
        /// More like individual T-SQL Statements looking up each individual email address
        /// </summary>
        public static void QueryFour()
        {
            using (var dbContext = new AdventureWorks2014Entities())
            {
                Console.WriteLine("Enter last name to search email addresses: ");
                var lastName = Console.ReadLine();

                var people = dbContext.People
                    .Where(x => x.LastName == lastName)
                    .Select(x => x);

                foreach (var person in people)
                {
                    foreach (var email in person.EmailAddresses)
                    {
                        Console.WriteLine(email.EmailAddress1);
                    }
                }
            }
        }

        /// <summary>
        /// Eager loading the email addresses in one server request
        /// SET Based operation
        /// </summary>
        public static void QueryFive()
        {
            using (var dbContext = new AdventureWorks2014Entities())
            {
                Console.WriteLine("Enter last name to search email addresses: ");
                var lastName = Console.ReadLine();

                var people = dbContext.People
                    .Include(x => x.EmailAddresses)
                    .Where(x => x.LastName == lastName)
                    .Select(x => x);

                //var people = from person in dbContext.People.Include(x => x.EmailAddresses)
                //             where person.LastName == lastName
                //             select person;

                foreach (var person in people)
                {
                    foreach (var email in person.EmailAddresses)
                    {
                        Console.WriteLine(email.EmailAddress1);
                    }
                }
            }
        }

        /// <summary>
        /// Eager loading done properly
        /// With explicit join, server filtering and projection
        /// </summary>
        public static void QuerySix()
        {
            using (var dbContext = new AdventureWorks2014Entities())
            {
                Console.WriteLine("Enter last name to search email addresses: ");
                var lastName = Console.ReadLine();

                var emails = from person in dbContext.People
                             join emailAddress in dbContext.EmailAddresses
                             on person.BusinessEntityID equals emailAddress.BusinessEntityID
                             where person.LastName == lastName
                             select emailAddress.EmailAddress1;

                //var emails = dbContext.People
                //    .Where(x => x.LastName == lastName)
                //    .SelectMany(x => x.EmailAddresses)
                //    .Select(x => x.EmailAddress1);

                //var emails =
                //    dbContext.People
                //    .Where(x => x.LastName == lastName)
                //    .Join(dbContext.EmailAddresses,
                //          l => l.BusinessEntityID,
                //          r => r.BusinessEntityID,
                //          (l, r) => r.EmailAddress1);


                foreach (var email in emails)
                {
                    Console.WriteLine(email);
                }
            }
        }
    }

    public class TSQLCodeConfig : DbConfiguration
    {
        public TSQLCodeConfig()
        {
            this.AddInterceptor(new TSQLSyntaxLogger());
        }
    }

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
