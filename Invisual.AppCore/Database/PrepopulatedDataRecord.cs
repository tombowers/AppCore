using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace Invisual.AppCore.Database
{
	public class PrepopulatedDataRecord : IDataRecord
	{
		private readonly OrderedDictionary _recordData = new OrderedDictionary(); 

		/// <summary>
		/// This class provides a translation and storage mechanism for IDataRecord instances.
		/// The supplied record's fields are enumerated and stored, allowing the data to be used outside of its provider.
		/// Duplicate field names are not permitted. If found, only the first instance of the field will be saved.
		/// </summary>
		public PrepopulatedDataRecord(IDataRecord dataRecord)
		{
			Enumerable.Range(0, dataRecord.FieldCount)
				.ToList()
				.ForEach(i =>
				{
					var key = dataRecord.GetName(i);
					if (!_recordData.Contains(key))
						_recordData.Add(key, dataRecord[i]);
				}
				);
		}

		public int FieldCount
		{
			get { return _recordData.Count; }
		}

		public bool GetBoolean(int i)
		{
			return (bool)_recordData[i];
		}

		public byte GetByte(int i)
		{
			return (byte)_recordData[i];
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotSupportedException();
		}

		public char GetChar(int i)
		{
			return (char)_recordData[i];
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotSupportedException();
		}

		public IDataReader GetData(int i)
		{
			throw new NotSupportedException();
		}

		public string GetDataTypeName(int i)
		{
			return GetFieldType(i).Name;
		}

		public DateTime GetDateTime(int i)
		{
			return (DateTime)_recordData[i];
		}

		public decimal GetDecimal(int i)
		{
			return (decimal)_recordData[i];
		}

		public double GetDouble(int i)
		{
			return (double)_recordData[i];
		}

		public Type GetFieldType(int i)
		{
			return _recordData[i].GetType();
		}

		public float GetFloat(int i)
		{
			return (float)_recordData[i];
		}

		public Guid GetGuid(int i)
		{
			return (Guid)_recordData[i];
		}

		public short GetInt16(int i)
		{
			return (short)_recordData[i];
		}

		public int GetInt32(int i)
		{
			return (int)_recordData[i];
		}

		public long GetInt64(int i)
		{
			return (long)_recordData[i];
		}

		public string GetName(int i)
		{
			return _recordData.Cast<DictionaryEntry>().ElementAt(i).Key.ToString();
		}

		public int GetOrdinal(string name)
		{
			throw new NotSupportedException();
		}

		public string GetString(int i)
		{
			return (string)_recordData[i];
		}

		public object GetValue(int i)
		{
			return this[i];
		}

		public int GetValues(object[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			var num = values.Length < FieldCount ? values.Length : FieldCount;
			for (var ordinal = 0; ordinal < num; ++ordinal)
				values[ordinal] = this[ordinal];

			return num;
		}

		public bool IsDBNull(int i)
		{
			return this[i] == DBNull.Value;
		}

		public object this[string name]
		{
			get { return _recordData[name]; }
		}

		public object this[int i]
		{
			get { return _recordData[i]; }
		}
	}
}
