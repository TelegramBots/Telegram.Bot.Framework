using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Middlewares
{
    /// <summary>
    /// Middleware for handling Telegram games' scores requests
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public class TelegramGameScoreMiddleware<TBot>
        where TBot : BotBase<TBot>
    {
        private readonly RequestDelegate _next;

        private readonly IInternalBotManager<TBot> _botManager;

        /// <summary>
        /// Initializes an instance of middleware
        /// </summary>
        /// <param name="next">Instance of request delegate</param>
        /// <param name="botManager">Bot manager for the bot</param>
        public TelegramGameScoreMiddleware(RequestDelegate next, IBotManager<TBot> botManager)
        {
            _next = next;
            _botManager = (IInternalBotManager<TBot>)botManager;
        }

        /// <summary>
        /// Gets invoked to handle the incoming request
        /// </summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            string path = context.Request.Path.Value;

            string gameShortname = _botManager.BotGameOptions
                .SingleOrDefault(g =>
                    _botManager.ReplaceGameUrlTokens(g.ScoresUrl, g.ShortName).EndsWith(path)
                        )
                    ?.ShortName;

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

            IGameHandler gameHandler = (IGameHandler)gameHandlerTuple.gameUpdateHandler;

            if (context.Request.Method == HttpMethods.Get)
            {
                string playerid = context.Request.Query["id"];

                var highScores = await gameHandler.GetHighestScoresAsync(_botManager.Bot, playerid);

                var responseData = JsonConvert.SerializeObject(highScores);
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(responseData);
            }
            else if (context.Request.Method == HttpMethods.Post)
            {
                string dataContent;
                using (var reader = new StreamReader(context.Request.Body))
                {
                    dataContent = await reader.ReadToEndAsync();
                }

                var scoreData = JsonConvert.DeserializeObject<SetGameScoreDto>(dataContent);
                if (scoreData == null)
                {
                    throw new Exception();
                }

                await gameHandler.SetGameScoreAsync(_botManager.Bot, scoreData.PlayerId, scoreData.Score);
                context.Response.StatusCode = StatusCodes.Status201Created;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
