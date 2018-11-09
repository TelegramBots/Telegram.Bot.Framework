using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Quickstart.AspNetCore.Handlers
{
    class StickerHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message;
            Sticker incomingSticker = msg.Sticker;

            StickerSet evilMindsSet = await context.Bot.Client.GetStickerSetAsync("EvilMinds", cancellationToken);

            Sticker similarEvilMindSticker = evilMindsSet.Stickers.FirstOrDefault(
                sticker => incomingSticker.Emoji.Contains(sticker.Emoji)
            );

            Sticker replySticker = similarEvilMindSticker ?? evilMindsSet.Stickers.First();

            await context.Bot.Client.SendStickerAsync(
                msg.Chat,
                replySticker.FileId,
                replyToMessageId: msg.MessageId,
                cancellationToken: cancellationToken
            );
        }
    }
}