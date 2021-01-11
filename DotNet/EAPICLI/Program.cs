using SSoTme.Default.Lib.CLIHandler;
using System;

namespace EAPICLI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                EAPICLIHandler.HandleRequest(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: ");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Stacktrace............................");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
