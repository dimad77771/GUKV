using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace GUKV.DataMigration
{
    public class DataReaderAdapter : IDataReader
    {
        private IDataReader reader = null;

        public bool SkipUnmappedRows = true;

        private Dictionary<string, Dictionary<int, int>> mappings = new Dictionary<string, Dictionary<int, int>>();

        private Dictionary<int, Dictionary<int, int>> mappingsByColIndex = new Dictionary<int, Dictionary<int, int>>();

        public DataReaderAdapter(IDataReader source)
        {
            reader = source;
        }

        #region Interface

        public void AddColumnMapping(string srcColumnName, Dictionary<int, int> mapping)
        {
            mappings.Add(srcColumnName.ToUpper().Trim(), mapping);
        }

        #endregion (Interface)

        #region Implementation

        private void PrepareMappingsByColumnIndex()
        {
            if (mappings.Count > 0 && mappingsByColIndex.Count == 0)
            {
                for (int i = 0; i < FieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
                    fieldName = fieldName.ToUpper().Trim();

                    Dictionary<int, int> mapping = null;

                    if (mappings.TryGetValue(fieldName, out mapping))
                    {
                        mappingsByColIndex.Add(i, mapping);
                    }
                }
            }
        }

        private bool RowIsUnmapped()
        {
            foreach (KeyValuePair<int, Dictionary<int, int>> pair in mappingsByColIndex)
            {
                if (!reader.IsDBNull(pair.Key))
                {
                    object curValue = reader.GetValue(pair.Key);

                    if (!pair.Value.ContainsKey((int)curValue))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion (Implementation)

        #region IDataReader interface implementation

        public void Close()
        {
            reader.Close();
        }

        public int Depth
        {
            get
            {
                return reader.Depth;
            }
        }

        public DataTable GetSchemaTable()
        {
            return reader.GetSchemaTable();
        }

        public bool IsClosed
        {
            get
            {
                return reader.IsClosed;
            }
        }

        public bool NextResult()
        {
            PrepareMappingsByColumnIndex();

            bool bResult = reader.NextResult();

            while (bResult && SkipUnmappedRows)
            {
                if (RowIsUnmapped())
                {
                    bResult = reader.NextResult();
                }
                else
                {
                    break;
                }
            }

            return bResult;
        }

        public bool Read()
        {
            PrepareMappingsByColumnIndex();

            bool bResult = reader.Read();

            while (bResult && SkipUnmappedRows)
            {
                if (RowIsUnmapped())
                {
                    bResult = reader.Read();
                }
                else
                {
                    break;
                }
            }

            return bResult;
        }

        public int RecordsAffected
        {
            get
            {
                return reader.RecordsAffected;
            }
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        public int FieldCount
        {
            get
            {
                return reader.FieldCount;
            }
        }

        public bool GetBoolean(int i)
        {
            return reader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return reader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return reader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return reader.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return reader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return reader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return reader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return reader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return reader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return reader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return reader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return reader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            int oldValue = reader.GetInt32(i);

            Dictionary<int, int> mapping = null;

            if (mappingsByColIndex.TryGetValue(i, out mapping))
            {
                int newValue = 0;

                if (mapping.TryGetValue(oldValue, out newValue))
                {
                    oldValue = newValue;
                }
                else
                {
                    throw new ArgumentException("DataReaderAdapter: value " + oldValue.ToString() + " is not mapped");
                }
            }

            return oldValue;
        }

        public long GetInt64(int i)
        {
            return reader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return reader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return reader.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return reader.GetString(i);
        }

        public object GetValue(int i)
        {
            object oldValue = reader.GetValue(i);

            Dictionary<int, int> mapping = null;

            if (mappingsByColIndex.TryGetValue(i, out mapping) && oldValue is int)
            {
                int newValue = 0;

                if (mapping.TryGetValue((int)oldValue, out newValue))
                {
                    oldValue = newValue;
                }
                else
                {
                    // throw new ArgumentException("DataReaderAdapter: value " + oldValue.ToString() + " is not mapped");

                    oldValue = System.DBNull.Value;
                }
            }

            return oldValue;
        }

        public int GetValues(object[] values)
        {
            int count = reader.GetValues(values);

            foreach (KeyValuePair<int, Dictionary<int, int>> pair in mappingsByColIndex)
            {
                if (pair.Key < count && values[pair.Key] is int)
                {
                    int newValue = 0;

                    if (pair.Value.TryGetValue((int)values[pair.Key], out newValue))
                    {
                        values[pair.Key] = newValue;
                    }
                    else
                    {
                        // throw new ArgumentException("DataReaderAdapter: value " + values[pair.Key].ToString() + " is not mapped");

                        values[pair.Key] = System.DBNull.Value;
                    }
                }
            }

            return count;
        }

        public bool IsDBNull(int i)
        {
            Dictionary<int, int> mapping = null;

            if (mappingsByColIndex.TryGetValue(i, out mapping))
            {
                object value = GetValue(i);

                if (value is int && !mapping.ContainsKey((int)value))
                {
                    return true;
                }
            }

            return reader.IsDBNull(i);
        }

        public object this[string name]
        {
            get
            {
                return GetValue(reader.GetOrdinal(name));
            }
        }

        public object this[int i]
        {
            get
            {
                return GetValue(i);
            }
        }

        #endregion (IDataReader interface implementation)
    }
}
