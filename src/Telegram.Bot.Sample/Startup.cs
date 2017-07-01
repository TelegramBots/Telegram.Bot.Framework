using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RecurrentTasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Sample.Bots.EchoBot;
using Telegram.Bot.Sample.Bots.GreeterBot;

namespace Telegram.Bot.Sample
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables("SampleEchoBot_")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Echo Bot
            
            var echoBotOptions = new BotOptions<EchoerBot>();
            Configuration.GetSection("EchoerBot").Bind(echoBotOptions);

            services.AddTelegramBot(echoBotOptions)
                .AddUpdateHandler<TextMessageEchoer>()
                .Configure();
            services.AddTask<BotUpdateGetterTask<EchoerBot>>();
            
            #endregion

            #region Greeter Bot

            services.AddTelegramBot<GreeterBot>(Configuration.GetSection("GreeterBot"))
                .AddUpdateHandler<StartCommand>()
                .AddUpdateHandler<PhotoForwarder>()
                .AddUpdateHandler<HiCommand>()
                .AddUpdateHandler<CrazyCircleGameHandler>()
                .Configure();
            services.AddTask<BotUpdateGetterTask<GreeterBot>>();

            services.AddDataProtection(); // Needed for Crazy Circle game

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ILogger<Startup> logger)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseTelegramBotLongPolling<EchoerBot>();
                app.StartTask<BotUpdateGetterTask<EchoerBot>>(TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(3));

                app.UseTelegramBotLongPolling<GreeterBot>();
                app.StartTask<BotUpdateGetterTask<GreeterBot>>(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3));

                logger.LogInformation("Update getting tasks are scheduled for bot(s)");
            }
            else
            {
                app.UseTelegramBotWebhook<EchoerBot>();
                app.UseTelegramBotWebhook<GreeterBot>();

                logger.LogInformation("Webhooks are set for bot(s)");
            }

            app.UseTelegramGame<GreeterBot>();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
