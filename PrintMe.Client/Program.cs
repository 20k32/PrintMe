using System.Diagnostics;
using System.Net.Mime;

namespace printme.client;

internal static class Program
{
    private static void Main(string[] _)
    {
        var workingDirectory = Directory.GetCurrentDirectory();
        
        var info = new ProcessStartInfo
        {
            FileName = "cmd",
            RedirectStandardInput = true,
            WorkingDirectory = workingDirectory
        };
        var process = Process.Start(info);

        if (process is not null)
        {
            process.StandardInput.WriteLine("npm run dev");
        }
        else
        {
            Console.WriteLine("Could not start dev-server.");
            Console.ReadKey();
        }
    }
}