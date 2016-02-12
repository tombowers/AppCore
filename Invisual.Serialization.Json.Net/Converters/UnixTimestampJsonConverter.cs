using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Invisual.Serialization.Json.Net.Converters
{
	public class UnixTimestampJsonConverter : DateTimeConverterBase
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Integer)
				throw new FormatException(string.Format("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));

			var ticks = (long)reader.Value;

			var date = new DateTime(1970, 1, 1);
			date = date.AddSeconds(ticks);

			return date;
		}

		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			long ticks;

			if (!(value is DateTime))
				throw new ArgumentException("Expected date object value.");

			var epoc = new DateTime(1970, 1, 1);
			var delta = ((DateTime)value) - epoc;

			if (delta.TotalSeconds < 0)
				throw new ArgumentOutOfRangeException("Unix epoch starts January 1st, 1970");

			ticks = (long)delta.TotalSeconds;

			writer.WriteValue(ticks);
		}
	}
}
