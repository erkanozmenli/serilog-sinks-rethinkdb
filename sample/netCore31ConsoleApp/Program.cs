using Serilog;
using System;

namespace netCore31ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .AuditTo.RethinkDB().CreateLogger();

            Log.Information(".NET Core 3.1");

            Console.ReadLine();
        }
    }
}
