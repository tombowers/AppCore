using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Invisual.Serialization.Json.Net.ContractResolvers
{
	/// <summary>
	/// Camel case property name resolver.
	/// A CustomNameAttribute can used to override property names on an individual basis.
	/// </summary>
	public class CamelCaseResolver : CamelCasePropertyNamesContractResolver
	{
		private readonly DefaultResolver _defaultResolver;

		public CamelCaseResolver()
		{
			_defaultResolver = new DefaultResolver();
		}

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var property = base.CreateProperty(member, memberSerialization);

			var customName = _defaultResolver.GetCustomName(member);

			if (customName != null)
				property.PropertyName = customName;

			return property;
		}
	}
}
