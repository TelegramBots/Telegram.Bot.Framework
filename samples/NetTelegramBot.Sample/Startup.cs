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
            services.Configure<SampleBotOptions>(Configuration.GetSection("SampleBotOptions"));
            services.AddSingleton<SampleBot>();

            services.Configure<AzureStorageServiceOptions>(Configuration.GetSection("AzureStorageServiceOptions"));
            services.AddSingleton<IStorageService, AzureStorageService>();

            services.AddTransient<ICommandParser>(_ => new CommandParser(true));

            services.AddTask<BotRunner<SampleBot>>();
            //services.AddSingleton<TelegramBotMiddleware<SampleBot>>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.StartTask<BotRunner<SampleBot>>(TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(3));
            //app.UseTelegramBot<SampleBot>("/hello");

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
