using Newtonsoft.Json;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Data transfer object containing user's score
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SetGameScoreDto // todo make it internal
    {
        /// <summary>
        /// Encoded and encrypted player id
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string PlayerId { get; set; }

        /// <summary>
        /// User's score
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Score { get; set; } // todo make nullable. and add validations
    }
}
