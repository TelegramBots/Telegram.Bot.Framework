using Newtonsoft.Json;

namespace Telegram.Bot.Framework
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SetGameScoreDto
    {
        [JsonProperty(Required = Required.Always)]
        public string PlayerId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Score { get; set; }
    }
}
