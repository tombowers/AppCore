namespace Invisual.Serialization
{
	public class SerializationSettings
	{
		public SerializationSettings()
		{
			NullValueHandling = NullValues.Include;
			ConvertEnumsUsingCustomNameAttribute = false;
			PropertyNameResolution = PropertyNameResolutionType.Native;
			DateTimeHandling = DateTimeType.Iso;
		}

		public NullValues NullValueHandling { get; set; }
		public bool ConvertEnumsUsingCustomNameAttribute { get; set; }
		public PropertyNameResolutionType PropertyNameResolution { get; set; }
		public DateTimeType DateTimeHandling { get; set; }

		public enum NullValues
		{
			Ignore,
			Include
		}

		public enum PropertyNameResolutionType
		{
			CamelCase,
			Native
		}

		public enum DateTimeType
		{
			/// <summary>
			/// JavaScript Date object format.
			/// </summary>
			/// <example>
			/// new Date(1234656000000)
			/// </example>
			JavaScript,

			/// <summary>
			/// ISO 8601 format date and time. https://en.wikipedia.org/wiki/ISO_8601
			/// </summary>
			/// <example>2009-02-15T00:00:00Z</example>
			Iso,

			/// <summary>
			/// Ticks since Unix epoch.
			/// </summary>
			/// <example>1430908860</example>
			UnixTimestamp
		}
	}
}
