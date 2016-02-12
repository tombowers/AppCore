using Newtonsoft.Json;
using Invisual.AppCore.Extensions;
using Invisual.Serialization.Attributes;
using System;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace Invisual.Serialization.Json.Net.Converters
{
	/// <summary>
	/// Custom Json.Net JsonConverter which will resolve enum values according to their DescriptionAttribute assignment.
	/// </summary>
	public class CustomValueEnumJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsEnum || objectType.IsNullableEnum();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			var objectValue = reader.Value;

			// Invoke the GetValueFromDescription method using reflection.
			// This is the only way to call a generic method with a type which isn't known at compile time.
			// BindingFlags.Instance | BindingFlags.NonPublic must be specified in order to search private methods.
			var method = GetType().GetMethod("GetValueFromCustomName", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(objectType);

			return method.Invoke(this, new[] { objectValue });
		}

		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			var fi = value.GetType().GetField(value.ToString());

			var customNameAttribute = (CustomNameAttribute)fi.GetCustomAttributes(
				typeof(CustomNameAttribute),
				false
				).FirstOrDefault();

			var name = customNameAttribute != null ? customNameAttribute.Name : value.ToString();

			serializer.Serialize(writer, name);
		}

		// Get an enum value matching the source string from the specified enum type.
		private T GetValueFromCustomName<T>(string name)
		{
			var type = typeof(T);
			if (!type.IsEnum && !type.IsNullableEnum())
				throw new InvalidOperationException("The specified generic Type argument must be an enum or a nullable enum");

			foreach (var field in type.GetFields())
			{
				var attribute = Attribute.GetCustomAttribute(field, typeof(CustomNameAttribute)) as CustomNameAttribute;

				if (attribute != null)
				{
					if (attribute.Name == name)
						return (T)field.GetValue(null);
				}
				else
				{
					if (field.Name == name)
						return (T)field.GetValue(null);
				}
			}

			if (type.IsNullableEnum())
				return default(T);

			throw new ArgumentException("Not found.", "name");
		}
	}
}