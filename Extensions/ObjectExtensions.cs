using System.ComponentModel;
using System.Linq;

namespace AppCore.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Retrieve the value of the description attribute for this object.
		/// Returns the name if not present.
		/// </summary>
		public static string GetDescription(this object value)
		{
			var fi = value.GetType().GetField(value.ToString());

			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
				typeof(DescriptionAttribute),
				false
				);

			return attributes.Any() ? attributes[0].Description : value.ToString();
		}
	}
}
