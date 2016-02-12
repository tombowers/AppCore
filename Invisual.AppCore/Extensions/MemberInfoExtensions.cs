using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Invisual.AppCore.Extensions
{
	public static class MemberInfoExtensions
	{
		public static IEnumerable<T> GetAttributes<T>(this MemberInfo value)
		{
			return value.GetCustomAttributes(
				typeof(T),
				false
				).Cast<T>();
		}
	}
}
