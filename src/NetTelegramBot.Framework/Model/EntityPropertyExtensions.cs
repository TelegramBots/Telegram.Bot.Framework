namespace Microsoft.WindowsAzure.Storage.Table
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Newtonsoft.Json;

    public static class EntityPropertyExtensions
    {
        public const string SerializationVersionPropertyName = "SerializationVersion";
        public const string EntityTypePropertyName = "EntityType";

        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture
        };

        public static T Deserialize<T>(this IDictionary<string, EntityProperty> properties, string name)
            where T : class
        {
            EntityProperty prop;

            if (!properties.TryGetValue(name, out prop))
            {
                return default(T);
            }

            var val = prop.StringValue;

            if (string.IsNullOrEmpty(val))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(val, JsonSettings);
        }

        public static void AddSerialized<T>(this IDictionary<string, EntityProperty> properties, string name, T value)
            where T : class
        {
            if (value != null)
            {
                properties.Add(name, new EntityProperty(JsonConvert.SerializeObject(value, JsonSettings)));
            }
        }
    }
}
