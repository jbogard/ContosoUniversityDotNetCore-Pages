using System;
using System.Linq;
using DbUp;

namespace ContosoUniversity.Database
{
    public class Program
    {
        static int Main(string[] args)
        {
            var connectionString =
                args.FirstOrDefault()
                ?? @"Server=(LocalDb)\mssqllocaldb; Database=ContosoUniversity; Trusted_connection=true";

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(typeof(Program).Assembly, s => s.EndsWith(".sql"))
                    .LogToConsole()
                    .Build();

            EnsureDatabase.For.SqlDatabase(connectionString);

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
