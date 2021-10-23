using System;
using Serilog;
using Serilog.Sinks.RethinkDB;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net461ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .AuditTo.RethinkDB()
            //    .Enrich.With<SequentialIdEnricher>()
            //    .CreateLogger();


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RethinkDB()
                .Enrich.With<SequentialIdEnricher>()
                .CreateLogger();


            var customer = new { CustomerName = "XCompany", CustomerID = 112345, CustomerCode = "0022222" };
            var log = Log.ForContext(customer.Log());

            for (int i = 0; i < 1000; i++)
            {
                log.Information("{test}", "console");
            }

            Log.CloseAndFlush();
        }
    }
}