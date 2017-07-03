using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RecurrentTasks;
using SampleGames.Bots.CrazyCircle;
using Telegram.Bot.Framework;

namespace SampleGames
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
            services.AddCors();
            services.AddDataProtection();

            #region CrazyCircle Bot

            services.AddTelegramBot<CrazyCircleBot>(_configuration.GetSection("CrazyCircleBot"))
                .AddUpdateHandler<StartCommand>()
                .AddUpdateHandler<CrazyCircleGameHandler>()
                .Configure();
            services.AddTask<BotUpdateGetterTask<CrazyCircleBot>>();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ILogger logger = loggerFactory.CreateLogger<Startup>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
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
            }

            app.UseCors(builder => builder
                .WithOrigins("http://crazy-circle-game.apphb.com")
                .WithMethods(HttpMethods.Get, HttpMethods.Post)
                .DisallowCredentials()
            );

            app.UseStaticFiles();

            #region CrazyCircle Bot

            app.UseTelegramGame<CrazyCircleBot>();

            if (env.IsDevelopment())
            {
                app.UseTelegramBotLongPolling<CrazyCircleBot>();
                app.StartTask<BotUpdateGetterTask<CrazyCircleBot>>(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3));
                logger.LogInformation("Update getting task is scheduled for bot " + nameof(CrazyCircleBot));
            }
            else
            {
                app.UseTelegramBotWebhook<CrazyCircleBot>();
                logger.LogInformation("Webhook is set for bot " + nameof(CrazyCircleBot));
            }

            #endregion
        }
    }
}
