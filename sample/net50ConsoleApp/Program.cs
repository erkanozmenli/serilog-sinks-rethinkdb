using Serilog;
using System;

namespace net50ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .AuditTo.RethinkDB().CreateLogger();

            Log.Information(".NET 5.0");

            Console.ReadLine();
        }
    }
}
