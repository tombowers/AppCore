using System;
using System.Data;

namespace AppCore.Extensions
{
	public static class DataRecordExtensions
	{
		public static T GetValueOrDefault<T>(this IDataRecord dr, string name)
		{
			var value = dr[name];
			if (DBNull.Value == value) return default(T);

			if (typeof(T).IsEnum || typeof(T).IsNullableEnum())
				return ((string)value).ToEnum<T>();

			return (T)value;
		}
	}
}
