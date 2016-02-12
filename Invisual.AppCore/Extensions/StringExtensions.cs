using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Invisual.AppCore.Extensions
{
	public static class StringExtensions
	{
		public static char ToCharFromUnicode(this string codePoint)
		{
			return (char)int.Parse(codePoint.Substring(2), System.Globalization.NumberStyles.HexNumber);
		}

		public static T ToEnum<T>(this string description)
		{
			if (!typeof(T).IsEnum && !typeof(T).IsNullableEnum())
				throw new ArgumentException("T must be an enum, or nullable enum type");
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("description must not be null, empty, or whitespace");

			var enumType = typeof(T).IsNullableEnum() ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T);

			return Enum.GetValues(enumType).Cast<T>().FirstOrDefault(v => v.GetDescription() == description);
		}

		/// <summary>
		/// Computes an SHA1 hash and converts it to Base 64
		/// </summary>
		public static string Sha1Base64Hash(this string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentException("input must not be null, empty, or whitespace");

			var bytes = Encoding.UTF8.GetBytes(input);

			var sha1 = SHA1.Create();
			var hashBytes = sha1.ComputeHash(bytes);

			return Convert.ToBase64String(hashBytes);
		}
	}
}
