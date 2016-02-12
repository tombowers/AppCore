using Newtonsoft.Json;
using Invisual.Serialization.Json.Net.Converters;
using System;

namespace Invisual.Serialization.Json.Net
{
	public class JsonSerializer : ISerializer
	{
		private readonly JsonSerializerSettings _serializerSettings;

		public JsonSerializer(SerializationSettings settings = null)
		{
			settings = settings ?? new SerializationSettings();
			_serializerSettings = settings.ToJsonSerializerSettings();
		}

		public string Serialize(object data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			return JsonConvert.SerializeObject(data, _serializerSettings);
		}

		public T Deserialize<T>(string data)
		{
			if (string.IsNullOrWhiteSpace(data))
				throw new ArgumentException("data must not be null, empty, or whitespace");

			return JsonConvert.DeserializeObject<T>(data, _serializerSettings);
		}
	}
}
