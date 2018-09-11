using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Framework;

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
//            services.AddTelegramBot<EchoBot>(_configuration.GetSection("EchoBot"))
////                .AddUpdateHandler<EchoCommand>()
//                .AddUpdateHandler<>()
//                .Configure();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            ILogger logger = loggerFactory.CreateLogger<Startup>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var source = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    logger.LogDebug("Press Enter to stop bot manager...");
                    Console.ReadLine();
                    source.Cancel();
                });

                Task.Factory.StartNew(async () =>
                {
                    var botManager = app.ApplicationServices.GetRequiredService<IBotManager<EchoBot>>();

                    // make sure webhook is disabled so we can use long-polling
                    await botManager.SetWebhookStateAsync(false);
                    logger.LogDebug("Webhook is disabled. Staring update handling...");

                    while (!source.IsCancellationRequested)
                    {
                        await Task.Delay(3_000);
                        await botManager.GetAndHandleNewUpdatesAsync();
                    }
                    logger.LogDebug("Bot manager stopped.");
                }).ContinueWith(t =>
                {
                    if (t.IsFaulted) throw t.Exception;
                });
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                    appBuilder.Run(context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        return Task.CompletedTask;
                    })
                );

                logger.LogInformation($"Setting webhook for {nameof(EchoBot)}...");
//                app.UseTelegramBotWebhook<EchoBot>();
                logger.LogInformation("Webhook is set for bot " + nameof(EchoBot));
            }


            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
