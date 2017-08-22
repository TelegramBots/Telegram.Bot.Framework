using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Base class for bot's games
    /// </summary>
    public abstract class GameHandlerBase : UpdateHandlerBase, IGameHandler
    {
        /// <summary>
        /// Game's short name
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Game configuration options
        /// </summary>
        public BotGameOption Options { get; set; }

        private readonly IDataProtector _dataProtector;

        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of game handler
        /// </summary>
        /// <param name="protectionProvider"></param>
        /// <param name="shortName">Game's short name</param>
        /// <param name="logger">An instance of logger</param>
        protected GameHandlerBase(
            IDataProtectionProvider protectionProvider, // ToDo use Property Injection from BotManager instead
            string shortName,
            ILogger logger)
        {
            _logger = logger;
            ShortName = shortName;
            _dataProtector = protectionProvider.CreateProtector(nameof(GameHandlerBase));
        }

        /// <summary>
        /// Indicates whether this handler should receive the update for handling by quickly checking the update type such as text, photo, or etc.
        /// </summary>
        /// <param name="bot">Instance of the bot this command is operating for</param>
        /// <param name="update">Update for the bot</param>
        /// <returns><value>true</value> if this handler should get the update; otherwise <value>false</value></returns>
        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            bool canHandle = false;

            if (update.CallbackQuery?.IsGameQuery == true &&
                update.CallbackQuery.GameShortName == ShortName)
            {
                canHandle = true;
            }

            return canHandle;
        }

        /// <summary>
        /// Handles the update for bot. This method will be called only if CanHandleUpdate returns <value>true</value>
        /// </summary>
        /// <param name="bot">Instance of the bot this command is operating for</param>
        /// <param name="update">The update to be handled</param>
        /// <returns>Result of handling this update</returns>
        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            string protectedPlayerid = EncodePlayerId(
                update.CallbackQuery.From.Id,
                update.CallbackQuery.InlineMessageId,
                update.CallbackQuery.Message?.Chat?.Id,
                update.CallbackQuery.Message?.MessageId ?? default(int)
            );

            string callbackUrl = WebUtility.UrlEncode(Options.ScoresUrl);

            string url = string.Format(Constants.UrlFormat, Options.Url, protectedPlayerid, callbackUrl);
            await bot.Client.AnswerCallbackQueryAsync(update.CallbackQuery.Id, url: url);

            return UpdateHandlingResult.Handled;
        }

        /// <summary>
        /// Set game score for user based on encrypted playerid
        /// </summary>
        /// <param name="bot">Instance of the bot</param>
        /// <param name="playerid">Encoded and protected player id</param>
        /// <param name="score">User's score</param>
        public virtual async Task SetGameScoreAsync(IBot bot, string playerid, int score)
        {
            Task<Message> setScoreTask;
            var ids = DecodePlayerId(playerid);
            if (ids.Item2.InlineMessageId != null)
            {
                setScoreTask = bot.Client.SetGameScoreAsync(ids.UserId, score, ids.Item2.InlineMessageId);
            }
            else
            {
                setScoreTask = bot.Client.SetGameScoreAsync(ids.UserId, score,
                    ids.Item2.Item2.ChatId,
                    ids.Item2.Item2.MessageId);
            }

            try
            {
                await setScoreTask;
            }
            catch (ApiRequestException e) when (e.Message.Contains("SCORE_INVALID"))
            {
                _logger.LogDebug("Failed to set invalid game score of {0} for player `{1}`",
                    score, ids);
            }
            catch (ApiRequestException e) when (e.Message.Contains("BOT_SCORE_NOT_MODIFIED"))
            {
                _logger.LogDebug("Score ({0}) not modified for player `{1}`",
                    score, ids);
            }
        }

        /// <summary>
        /// Get game scores for chat based on encrypted playerid
        /// </summary>
        /// <param name="bot">Instance of the bot</param>
        /// <param name="playerid">Encoded and protected player id</param>
        /// <returns>Array of scores for chat</returns>
        public virtual Task<GameHighScore[]> GetHighestScoresAsync(IBot bot, string playerid)
        {
            Task<GameHighScore[]> highScoresTask;
            var ids = DecodePlayerId(playerid);

            if (ids.Item2.InlineMessageId != null)
            {
                highScoresTask = bot.Client.GetGameHighScoresAsync(ids.UserId, ids.Item2.InlineMessageId);
            }
            else
            {
                highScoresTask = bot.Client.GetGameHighScoresAsync(ids.UserId,
                    ids.Item2.Item2.ChatId,
                    ids.Item2.Item2.MessageId);
            }

            return highScoresTask;
        }

        private string EncodePlayerId(int userid, string inlineMsgId, ChatId chatid, int msgId)
        {
            var values = new List<string> {userid.ToString()};

            if (inlineMsgId != null)
            {
                values.Add(inlineMsgId);
            }
            else if (chatid != null && msgId != default(int))
            {
                values.Add(chatid);
                values.Add(msgId.ToString());
            }
            else
            {
                throw new ArgumentException();
            }

            string playerid = string.Join(Constants.PlayerIdSeparator.ToString(), values);
            playerid = _dataProtector.Protect(playerid);
            playerid = WebUtility.UrlEncode(playerid);

            return playerid;
        }

        private (int UserId, (string InlineMessageId, (ChatId ChatId, int MessageId))) DecodePlayerId(
            string encodedPlayerid)
        {
            encodedPlayerid = WebUtility.UrlDecode(encodedPlayerid);
            encodedPlayerid = _dataProtector.Unprotect(encodedPlayerid);

            string[] tokens = encodedPlayerid
                .Split(Constants.PlayerIdSeparator);

            int userid = int.Parse(tokens[0]);
            if (tokens.Length == 2)
            {
                string inlineMsgId = tokens[1];
                return (userid, (inlineMsgId, default((ChatId ChatId, int MessageId))));
            }
            else if (tokens.Length == 3)
            {
                ChatId chatid = new ChatId(tokens[1]);
                int msgId = int.Parse(tokens[2]);
                return (userid, (null, (chatid, msgId)));
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private static class Constants
        {
            public const char PlayerIdSeparator = ':';

            public const string UrlFormat = "{0}#id={1}&gameScoreUrl={2}";
        }
    }
}