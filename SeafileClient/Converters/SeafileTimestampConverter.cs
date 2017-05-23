﻿using Newtonsoft.Json;
using SeafileClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeafileClient.Converters
{
    /// <summary>
    /// JsonConverter for converting between dotnet datetimes and unix timestamps
    /// </summary>
    class SeafileTimestampConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime)) || (objectType == typeof(DateTime?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var timestamp = serializer.Deserialize<long>(reader);
                return SeafileDateUtils.SeafileTimeToDateTime(timestamp);
            } catch (JsonSerializationException)
            {
                // value is probably null
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
                serializer.Serialize(writer, SeafileDateUtils.DateTimeToSeafileTime((DateTime)value));
            else
                throw new InvalidOperationException("SeafTimestampConverter can only serialize datetime objects.");
        }
    }
}
