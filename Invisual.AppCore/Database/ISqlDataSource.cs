using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Invisual.AppCore.Database
{
	public interface ISqlDataSource
	{
		IEnumerable<IDataRecord> Execute(string sql);
		IEnumerable<IDataRecord> Execute(string sql, Dictionary<string, object> parameters);
		IEnumerable<IDataRecord> Execute(string sql, CommandType commandType);
		IEnumerable<IDataRecord> Execute(string sql, Dictionary<string, object> parameters, CommandType commandType);
		IEnumerable<PrepopulatedDataRecord> Execute(string sql, Dictionary<string, object> parameters, CommandType commandType, ref Dictionary<string, object> outParameters);

		T ExecuteScalar<T>(string sql);
		T ExecuteScalar<T>(string sql, Dictionary<string, object> parameters);

		int ExecuteNonQuery(string sql);
		int ExecuteNonQuery(string sql, Dictionary<string, object> parameters);
		int ExecuteNonQuery(string sql, Dictionary<string, object> parameters, CommandType commandType);
		IEnumerable<SqlParameter> ExecuteNonQuery(string sql, IEnumerable<SqlParameter> parameters, CommandType commandType);
	}
}
