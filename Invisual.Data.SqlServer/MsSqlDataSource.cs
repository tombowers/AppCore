using Invisual.AppCore.Database;
using Invisual.AppCore.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Invisual.Data
{
	public class MsSqlDataSource : ISqlDataSource
	{
		private readonly string _connectionString;

		public MsSqlDataSource(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException("connectionString must not be null, empty, or whitespace");

			_connectionString = connectionString;
		}

		/// <summary>
		/// Executes T-SQL on the server.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{IDataRecord}"/> object.</returns>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public IEnumerable<IDataRecord> Execute(string sql)
		{
			return Execute(sql, null);
		}

		/// <summary>
		/// Executes T-SQL on the server.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{IDataRecord}"/> object.</returns><exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public IEnumerable<IDataRecord> Execute(string sql, Dictionary<string, object> parameters)
		{
			return Execute(sql, parameters, CommandType.Text);
		}

		/// <summary>
		/// Executes T-SQL on the server.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{IDataRecord}"/> object.</returns><exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public IEnumerable<IDataRecord> Execute(string sql, CommandType commandType)
		{
			return Execute(sql, null, commandType);
		}

		/// <summary>
		/// Executes T-SQL on the server.
		/// The command is executed when enumeration begins, and the data is streamed row-by-row.
		/// The connection to Sql Server will remain open until the results have been fully enumerated.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{IDataRecord}"/> object.</returns>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public IEnumerable<IDataRecord> Execute(string sql, Dictionary<string, object> parameters, CommandType commandType)
		{
			if (string.IsNullOrWhiteSpace(sql))
				throw new ArgumentException("sql must not be null, empty, or whitespace");
			if(!Enum.IsDefined(typeof(CommandType), commandType))
				throw new ArgumentOutOfRangeException("commandType");

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = commandType;

					if (parameters != null)
						foreach (var parameter in parameters)
							command.Parameters.AddWithValue(parameter.Key, parameter.Value);

					using (var reader = command.ExecuteReader())
						while (reader.Read())
							yield return reader;
				}
			}
		}

		/// <summary>
		/// Executes T-SQL on the server.
		/// The command is executed immediately, and the ref parameters assigned to.
		/// This should not be used for large result sets as all rows are read once internally in order to retrieve any out parameters.
		/// Each row is a PrepopulatedDataRecord instance, so the results can be enumerated multiple times.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{IDataRecord}"/> object.</returns>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public IEnumerable<PrepopulatedDataRecord> Execute(string sql, Dictionary<string, object> parameters, CommandType commandType, ref Dictionary<string, object> outParameters)
		{
			if (string.IsNullOrWhiteSpace(sql))
				throw new ArgumentException("sql must not be null, empty, or whitespace");
			if (!Enum.IsDefined(typeof(CommandType), commandType))
				throw new ArgumentOutOfRangeException("commandType");
			if (outParameters == null)
				throw new ArgumentNullException("outParameters");

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = commandType;

					if (parameters != null)
						foreach (var parameter in parameters)
							command.Parameters.AddWithValue(parameter.Key, parameter.Value);

					foreach (var parameter in outParameters)
					{
						command.Parameters.Add(
							parameter.Key,
							parameter.Value.GetType().ToSqlDbType()
							)
							.Direction = ParameterDirection.ReturnValue;
					}

					var rows = new List<PrepopulatedDataRecord>();

					using (var reader = command.ExecuteReader())
						while (reader.Read())
							rows.Add(new PrepopulatedDataRecord(reader));

					// Take copy of outParameters dictionary to allow mid-loop modification
					foreach (var parameter in outParameters.ToDictionary(k => k.Key, k => k.Value))
						outParameters[parameter.Key] = command.Parameters[parameter.Key].Value;

					return rows;
				}
			}
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the result set returned by the query.
		/// Additional columns or rows are ignored.
		/// Returns null (or the default for value types) if the result set is empty.
		/// </summary>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public T ExecuteScalar<T>(string sql)
		{
			return ExecuteScalar<T>(sql, null);
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the result set returned by the query.
		/// Additional columns or rows are ignored.
		/// Returns null (or the default for value types) if the result set is empty.
		/// </summary>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public T ExecuteScalar<T>(string sql, Dictionary<string, object> parameters)
		{
			if (string.IsNullOrWhiteSpace(sql))
				throw new ArgumentException("sql must not be null, empty, or whitespace");

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand(sql, connection))
				{
					if (parameters != null)
						foreach (var parameter in parameters)
							command.Parameters.AddWithValue(parameter.Key, parameter.Value);

					var result = command.ExecuteScalar();

					return result == null
						? default(T)
						: (T)result;
				}
			}
		}

		/// <summary>
		/// Executes a Transact-SQL statement against the connection.
		/// For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
		/// When a trigger exists on a table being inserted or updated, the return value includes the number of rows affected by both the insert or update operation and the number of rows affected by the trigger or triggers.
		/// For all other types of statements, the return value is -1. If a rollback occurs, the return value is also -1.
		/// </summary>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public int ExecuteNonQuery(string sql)
		{
			return ExecuteNonQuery(sql, null);
		}

		/// <summary>
		/// Executes a Transact-SQL statement against the connection.
		/// For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
		/// When a trigger exists on a table being inserted or updated, the return value includes the number of rows affected by both the insert or update operation and the number of rows affected by the trigger or triggers.
		/// For all other types of statements, the return value is -1. If a rollback occurs, the return value is also -1.
		/// </summary>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public int ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
		{
			return ExecuteNonQuery(sql, parameters, CommandType.Text);
		}

		/// <summary>
		/// Executes a Transact-SQL statement against the connection.
		/// For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
		/// When a trigger exists on a table being inserted or updated, the return value includes the number of rows affected by both the insert or update operation and the number of rows affected by the trigger or triggers.
		/// For all other types of statements, the return value is -1. If a rollback occurs, the return value is also -1.
		/// </summary>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public int ExecuteNonQuery(string sql, Dictionary<string, object> parameters, CommandType commandType)
		{
			if (string.IsNullOrWhiteSpace(sql))
				throw new ArgumentException("sql must not be null, empty, or whitespace");
			if (!Enum.IsDefined(typeof(CommandType), commandType))
				throw new ArgumentOutOfRangeException("commandType");

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = commandType;

					if (parameters == null)
						return command.ExecuteNonQuery();

					foreach (var parameter in parameters)
						command.Parameters.AddWithValue(parameter.Key, parameter.Value);

					return command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Executes a Transact-SQL statement against the connection.
		/// Fully initiated SqlParameters must be supplied.
		/// Any output or return params will be returned, otherwise the return value will be an empty collection.
		/// </summary>
		/// <exception cref="System.InvalidCastException"></exception><exception cref="System.Data.SqlClient.SqlException"></exception><exception cref="System.InvalidOperationException"></exception><exception cref="System.IO.IOException"></exception><exception cref="System.ObjectDisposedException"></exception>
		public IEnumerable<SqlParameter> ExecuteNonQuery(string sql, IEnumerable<SqlParameter> parameters, CommandType commandType)
		{
			if (string.IsNullOrWhiteSpace(sql))
				throw new ArgumentException("sql must not be null, empty, or whitespace");
			if (!Enum.IsDefined(typeof(CommandType), commandType))
				throw new ArgumentOutOfRangeException("commandType");

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = commandType;

					if (parameters != null)
						command.Parameters.AddRange(parameters.ToArray());

					command.ExecuteNonQuery();

					return command.Parameters.Cast<SqlParameter>().Where(param => new[]
						{
							ParameterDirection.Output, ParameterDirection.InputOutput, ParameterDirection.ReturnValue
						}.Contains(param.Direction)
					)
					.ToList();
				}
			}
		}
	}
}
