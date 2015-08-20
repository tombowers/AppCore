using System;
using System.Collections.Generic;
using System.Data;

namespace Invisual.AppCore.Extensions
{
	public static class TypeExtensions
	{
		public static bool IsNullableEnum(this Type T)
		{
			var u = Nullable.GetUnderlyingType(T);
			return (u != null) && u.IsEnum;
		}

		public static SqlDbType ToSqlDbType(this Type T)
		{
			return SqlTypeMap[T];
		}

		private static readonly Dictionary<Type, SqlDbType> SqlTypeMap = new Dictionary<Type, SqlDbType>
		{
			{ typeof(Int16), SqlDbType.SmallInt },
			{ typeof(Int32), SqlDbType.Int },
			{ typeof(Int64), SqlDbType.BigInt },
			{ typeof(Byte[]), SqlDbType.Binary },
			{ typeof(Boolean), SqlDbType.Bit },
			{ typeof(String), SqlDbType.VarChar },
			{ typeof(DateTime), SqlDbType.Date },
			{ typeof(Decimal), SqlDbType.Decimal },
			{ typeof(Double), SqlDbType.Float },
			{ typeof(Guid), SqlDbType.UniqueIdentifier }
		};
	}
}
