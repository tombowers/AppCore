using System;
using System.IO;
using System.Xml.Serialization;

namespace Invisual.Serialization.Xml
{
	public class BasicXmlSerializer : ISerializer
	{
		public string Serialize(object data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			var serializer = new XmlSerializer(data.GetType());

			using (var writer = new StringWriter())
			{
				serializer.Serialize(writer, data);
				return writer.ToString();
			}
		}

		public T Deserialize<T>(string data)
		{
			if (string.IsNullOrWhiteSpace(data))
				throw new ArgumentException("data must not be null, empty, or whitespace");

			var serializer = new XmlSerializer(typeof(T));

			using (var reader = new StringReader(data))
				return (T)serializer.Deserialize(reader);
		}
	}
}
