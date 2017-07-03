using System;
using System.Collections.Generic;
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
    public abstract class GameHandlerBase : UpdateHandlerBase, IGameHandler
    {
        public string ShortName { get; }

        public string BotBaseUrl
        {
            get { return _botBaseUrl; }
            set
            {
                _botBaseUrl = value;

                if (value.StartsWith("https://"))
                    value = "http://" + value.Substring(8);

                _scoresCallbackUrl = value + $"games/{ShortName}/scores";
            }
        }

        public string GamePageUrl
        {
            get { return _gamePageUrl; }
            set
            {
                if (value.StartsWith("https://"))
                    value = "http://" + value.Substring(8);

                _gamePageUrl = value
                    .Replace("{game}", ShortName);
            }
        }

        protected string ScoresCallbackUrl => _scoresCallbackUrl;

        private readonly IDataProtector _dataProtector;

        private string _scoresCallbackUrl;

        private string _botBaseUrl;

        private string _gamePageUrl;

        protected GameHandlerBase(IDataProtectionProvider protectionProvider,
            string shortName)
        {
            ShortName = shortName;
            _dataProtector = protectionProvider.CreateProtector(nameof(GameHandlerBase));
        }

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

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            if (update.Type == UpdateType.CallbackQueryUpdate)
            {
                string protectedPlayerid = EncodePlayerId(
                    int.Parse(update.CallbackQuery.From.Id),
                    update.CallbackQuery.InlineMessageId,
                    update.CallbackQuery.Message?.Chat?.Id,
                    update.CallbackQuery.Message?.MessageId ?? default(int)
                    );

                string callbackUrl = WebUtility.UrlEncode(ScoresCallbackUrl);

                string url = string.Format(Constants.UrlFormat, GamePageUrl, protectedPlayerid, callbackUrl);
                await bot.Client.AnswerCallbackQueryAsync(update.CallbackQuery.Id, url: url);
            }

            return UpdateHandlingResult.Handled;
        }

        public virtual async Task SetGameScoreAsync(IBot bot, string playerid, int score)
        {
            var ids = DecodePlayerId(playerid);
            try
            {
                if (ids.Item2.InlineMessageId != null)
                {
                    await bot.Client.SetGameScoreAsync(ids.UserId, score, ids.Item2.InlineMessageId);
                }
                else
                {
                    await bot.Client.SetGameScoreAsync(ids.UserId, score,
                        ids.Item2.Item2.ChatId,
                        ids.Item2.Item2.MessageId);
                }

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
            var ids = DecodePlayerId(playerid);

            if (ids.Item2.InlineMessageId != null)
            {
                return bot.Client.GetGameHighScoresAsync(ids.UserId, ids.Item2.InlineMessageId);
            }
            else
            {
                return bot.Client.GetGameHighScoresAsync(ids.UserId,
                    ids.Item2.Item2.ChatId,
                    ids.Item2.Item2.MessageId);
            }
        }

        private string EncodePlayerId(int userid, string inlineMsgId, ChatId chatid, int msgId)
        {
            var values = new List<string> { userid.ToString() };

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

        private (int UserId, (string InlineMessageId, (ChatId ChatId, int MessageId))) DecodePlayerId(string encodedPlayerid)
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

            // todo use this maybe
            public const string PlayerIdFormat = "{0}:{1}"; // {userId}:{inlineMessageId} such as "1234:-324431213435"

            public const string UrlFormat = "{0}#id={1}&gameScoreUrl={2}";
        }
    }
}
