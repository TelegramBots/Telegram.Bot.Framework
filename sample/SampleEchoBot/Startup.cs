using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstractions;

namespace SampleEchoBot
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTelegramBot<EchoBot>(_configuration.GetSection("EchoBot"))
                .AddUpdateHandler<EchoCommand>()
                .Configure();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var source = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Press Enter to stop bot manager...");
                Console.ReadLine();
                source.Cancel();
            });
            
            Task.Factory.StartNew(async () =>
            {
                var botManager = app.ApplicationServices.GetRequiredService<IBotManager<EchoBot>>();
                while (!source.IsCancellationRequested)
                {
                    await Task.Delay(3_000);
                    await botManager.GetAndHandleNewUpdatesAsync();
                }
                Console.WriteLine("Bot manager stopped.");
            }).ContinueWith(t =>
            {
                if (t.IsFaulted) throw t.Exception;
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
