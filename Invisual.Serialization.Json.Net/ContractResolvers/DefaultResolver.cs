using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Invisual.AppCore.Extensions;
using Invisual.Serialization.Attributes;
using System.Linq;
using System.Reflection;

namespace Invisual.Serialization.Json.Net.ContractResolvers
{
	public class DefaultResolver : DefaultContractResolver
	{
		public string GetCustomName(MemberInfo member)
		{
			var customNameAttribute = member.GetAttributes<CustomNameAttribute>().FirstOrDefault();

			return customNameAttribute != null ? customNameAttribute.Name : null;
		}

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var property = base.CreateProperty(member, memberSerialization);

			var customName = GetCustomName(member);
			if (customName != null)
				property.PropertyName = customName;

			return property;
		}
	}
}
