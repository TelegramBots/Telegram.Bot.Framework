using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NetTelegram.Bot.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = ".NET Telegram Bot Framework - Samples";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
