using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace netCore21ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .AuditTo.RethinkDB().CreateLogger();      

            Log.Information(".NET Core 2.1");

            Console.ReadLine();
        }
    }
}
