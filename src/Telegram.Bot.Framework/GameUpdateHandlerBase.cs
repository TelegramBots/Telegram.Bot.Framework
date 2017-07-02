using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public abstract class GameUpdateHandlerBase : UpdateHandlerBase
    {
        private readonly IDataProtector _dataProtector;

        protected readonly string GameBaseUrl;

        protected readonly string GameShortname;

        protected readonly string BotBaseUrl;

        protected GameUpdateHandlerBase(
            IDataProtectionProvider protectionProvider,
            string gameBaseUrl,
            string gameShortname,
            string botBaseUrl // todo Use bot options instead
            )
        {
            _dataProtector = protectionProvider.CreateProtector(nameof(GameUpdateHandlerBase));
            GameBaseUrl = gameBaseUrl;
            GameShortname = gameShortname;
            BotBaseUrl = botBaseUrl;
        }

        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            bool canHandle = false;

            if (update.CallbackQuery?.IsGameQuery == true &&
                update.CallbackQuery.GameShortName.Equals(GameShortname, StringComparison.OrdinalIgnoreCase))
            {
                canHandle = true;
            }

            return canHandle;
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            if (update.Type == UpdateType.CallbackQueryUpdate)
            {
                string protectedPlayerid = ProtectPlayerId(
                    long.Parse(update.CallbackQuery.From.Id), update.CallbackQuery.InlineMessageId);

                string callbackUrl = BotBaseUrl + GameShortname;
                callbackUrl = WebUtility.UrlEncode(callbackUrl);

                string url = string.Format(Constants.UrlFormat, GameBaseUrl, protectedPlayerid, callbackUrl);
                await bot.Client.AnswerCallbackQueryAsync(update.CallbackQuery.Id, url: url);
            }

            return UpdateHandlingResult.Handled;
        }

        public virtual async Task SetGameScoreAsync(IBot bot, string playerid, int score)
        {
            var userMessageId = UnprotectPlayerId(playerid);
            try
            {
                await bot.Client.SetGameScoreAsync((int) userMessageId.UserId, score, userMessageId.InlineMessageId);
            }
            catch (JsonException e)
            {
                // todo remove this try-catch after issue with deserializing response is resolved
                Debug.WriteLine(e);
            }
            catch (ApiRequestException e)
            {
                Debug.WriteLine(e);
            }
        }

        public virtual Task<GameHighScore[]> GetHighestScoresAsync(IBot bot, string playerid)
        {
            var userMessageId = UnprotectPlayerId(playerid);
            return bot.Client.GetGameHighScoresAsync((int)userMessageId.UserId, userMessageId.InlineMessageId);
        }

        private string ProtectPlayerId(long userid, string chatInstance)
        {
            string playerid = string.Format(Constants.PlayerIdFormat, userid, chatInstance);
            playerid = _dataProtector.Protect(playerid);
            playerid = WebUtility.UrlEncode(playerid);

            return playerid;
        }

        private (long UserId, string InlineMessageId) UnprotectPlayerId(string playerid)
        {
            playerid = WebUtility.UrlDecode(playerid);
            playerid = _dataProtector.Unprotect(playerid);

            string[] tokens = playerid
                .Split(Constants.PlayerIdSeparator);

            return (long.Parse(tokens[0]), tokens[1]);
        }

        private static class Constants
        {
            public const char PlayerIdSeparator = ':';

            public const string PlayerIdFormat = "{0}:{1}"; // {userId}:{inlineMessageId} such as "1234:-324431213435"

            public const string UrlFormat = "{0}#id={1}&gameScoreUrl={2}";
        }
    }
}
