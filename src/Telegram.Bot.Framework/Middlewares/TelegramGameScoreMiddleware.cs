using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Middlewares
{
    public class TelegramGameScoreMiddleware<TBot>
        where TBot : BotBase<TBot>
    {
        private readonly RequestDelegate _next;

        private readonly IBotManager<TBot> _botManager;

        /// <summary>
        /// Initializes an instance of middleware
        /// </summary>
        /// <param name="next">Instance of request delegate</param>
        /// <param name="botManager">Bot manager for the bot</param>
        public TelegramGameScoreMiddleware(RequestDelegate next, IBotManager<TBot> botManager)
        {
            _next = next;
            _botManager = botManager;
        }

        public async Task Invoke(HttpContext context)
        {
            var bManager = (BotManager<TBot>)_botManager; // todo use an internal interface --> IInternalManager : IBotManager

            string route = $"/bots/{bManager.Bot.UserName}/games/{{game}}/scores"; // todo allow override default value from appsettings

            string gameShortname = bManager.BotGameOptions
                .SingleOrDefault(g =>
                    context.Request.Path.StartsWithSegments(route.Replace("{game}", g.ShortName)))
                    ?.ShortName;

            // todo use PUT method instead of POST
            if (string.IsNullOrWhiteSpace(gameShortname) ||
                !new[] { HttpMethods.Post, HttpMethods.Get }.Contains(context.Request.Method))
            {
                await _next.Invoke(context);
                return;
            }

            var gameHandlerTuple = _botManager.TryFindGameUpdateHandler(gameShortname);
            if (!gameHandlerTuple.Success)
            {
                await _next.Invoke(context);
                return;
            }

            var gameHandler = (GameUpdateHandlerBase)gameHandlerTuple.gameUpdateHandler;

            if (context.Request.Method == HttpMethods.Get)
            {
                string playerid = context.Request.Query["id"];

                var highScores = await gameHandler.GetHighestScoresAsync(bManager.Bot, playerid);

                var responseData = JsonConvert.SerializeObject(highScores);
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(responseData);
                return;
            }
            else if (context.Request.Method == HttpMethods.Post)
            {
                string dataContent;
                using (var reader = new StreamReader(context.Request.Body as Stream))
                {
                    dataContent = await reader.ReadToEndAsync();
                }

                var scoreData = JsonConvert.DeserializeObject<SetGameScoreDto>(dataContent);
                if (scoreData == null)
                {
                    throw new Exception();
                }

                await gameHandler.SetGameScoreAsync(bManager.Bot, scoreData.PlayerId, scoreData.Score);
                context.Response.StatusCode = StatusCodes.Status201Created;
                return;
            }
            else
            {
                await _next.Invoke(context);
                return;
            }
        }
    }
}
