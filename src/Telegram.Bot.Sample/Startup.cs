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
            #region Echoer Bot

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
                .AddUpdateHandler<CrazyCircleGameHandler>()
                .AddUpdateHandler<HiCommand>()
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

            #region Echoer Bot

            if (env.IsDevelopment())
            {
                app.UseTelegramBotLongPolling<EchoerBot>();
                app.StartTask<BotUpdateGetterTask<EchoerBot>>(TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(3));
                logger.LogInformation("Update getting task is scheduled for bot " + nameof(EchoerBot));
            }
            else
            {
                app.UseTelegramBotWebhook<EchoerBot>();
                logger.LogInformation("Webhook is set for bot " + nameof(EchoerBot));
            }

            #endregion

            #region Greeter Bot

            app.UseTelegramGame<GreeterBot>();

            if (env.IsDevelopment())
            {
                app.UseTelegramBotLongPolling<GreeterBot>();
                app.StartTask<BotUpdateGetterTask<GreeterBot>>(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3));
                logger.LogInformation("Update getting task is scheduled for bot " + nameof(GreeterBot));
            }
            else
            {
                app.UseTelegramBotWebhook<GreeterBot>();
                logger.LogInformation("Webhook is set for bot " + nameof(GreeterBot));
            }

            #endregion

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
