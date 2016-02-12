using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Invisual.Serialization.Json.Net.ContractResolvers;
using Invisual.Serialization.Json.Net.Converters;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Invisual.Serialization.Json.Net
{
	public static class Extensions
	{
		public static JsonSerializerSettings ToJsonSerializerSettings(this SerializationSettings settings)
		{
			var serializerSettings = new JsonSerializerSettings();

			var jsonConverters = new List<JsonConverter>();

			if (settings.ConvertEnumsUsingCustomNameAttribute)
				jsonConverters.Add(new CustomValueEnumJsonConverter());

			switch(settings.DateTimeHandling)
			{
				case SerializationSettings.DateTimeType.JavaScript:
					jsonConverters.Add(new JavaScriptDateTimeConverter());
					break;

				case SerializationSettings.DateTimeType.Iso:
					jsonConverters.Add(new IsoDateTimeConverter());
					break;

				case SerializationSettings.DateTimeType.UnixTimestamp:
					jsonConverters.Add(new UnixTimestampJsonConverter());
					break;

				default:
					throw new InvalidOperationException("Unsupported value specified for settings.DateTimeHandling");
			}

			if (jsonConverters.Any())
				serializerSettings.Converters = jsonConverters.ToArray();

			switch(settings.NullValueHandling)
			{
				case SerializationSettings.NullValues.Ignore:
					serializerSettings.NullValueHandling = NullValueHandling.Ignore;
					break;

				case SerializationSettings.NullValues.Include:
					serializerSettings.NullValueHandling = NullValueHandling.Include;
					break;

				default:
					throw new InvalidOperationException("Unsupported value specified for settings.NullValueHandling");
			}

			switch(settings.PropertyNameResolution)
			{
				case SerializationSettings.PropertyNameResolutionType.CamelCase:
					serializerSettings.ContractResolver = new CamelCaseResolver();
					break;

				case SerializationSettings.PropertyNameResolutionType.Native:
				default:
					serializerSettings.ContractResolver = new DefaultResolver();
					break;
			}

			return serializerSettings;
		}
	}
}
