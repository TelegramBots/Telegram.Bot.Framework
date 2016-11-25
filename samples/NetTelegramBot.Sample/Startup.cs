namespace NetTelegramBot.Sample
{
    using System;
    using Framework;
    using Framework.Storage;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NetTelegramBotApi.Requests;
    using RecurrentTasks;

    public class Startup
    {
        private IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureStorageServiceOptions>(Configuration.GetSection("AzureStorageServiceOptions"));
            services.AddSingleton<IStorageService, AzureStorageService>();

            services.Configure<SampleBotOptions>(Configuration.GetSection("SampleBotOptions"));
            services.AddTransient<ICommandParser>(_ => new CommandParser('_'));

            services.AddSingleton<SampleBot>();

            var useWebhook = !string.IsNullOrEmpty(Configuration["SampleBotOptions:WebhookUrl"]);
            if (useWebhook)
            {
                services.AddSingleton<TelegramBotMiddleware<SampleBot>>();
            }
            else
            {
                services.AddTask<BotRunner<SampleBot>>();
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var logger = loggerFactory.CreateLogger<Startup>();

            var sampleBot = app.ApplicationServices.GetRequiredService<SampleBot>();
            var botOptions = app.ApplicationServices.GetRequiredService<IOptions<SampleBotOptions>>().Value;
            var useWebhook = !string.IsNullOrEmpty(botOptions.WebhookUrl);
            if (useWebhook)
            {
                // enable middleware
                app.UseTelegramBot<SampleBot>(Configuration["SampleBotOptions:Endpoint"]);

                // SetWebhook on telegram server
                sampleBot.SendAsync(new SetWebhook(botOptions.WebhookUrl)).Wait();
                logger.LogInformation("Webhook set Ok");
            }
            else
            {
                // start polling every 15 seconds (first run after 3 seconds)
                app.StartTask<BotRunner<SampleBot>>(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(3));

                // remove webhook from telegram server (in case it was set earlier)
                sampleBot.SendAsync(new SetWebhook(string.Empty)).Wait();
                logger.LogInformation("Webhook disabled");
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("<html><body><h1>Nothing here!</h1></body></html>");
            });
        }
    }
}
