using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CsvHelper;

namespace Bango.SqlImport.DataReader.Csv
{
    /// <summary>
    /// Reader providing streaming access to a CSV file.
    /// </summary>
    public class CsvDataReader : IDataReader
    {
        private readonly List<TypeCode> typeMappings;
        private readonly CsvReader csvReader;
        private readonly TextReader textReader;
        private readonly string[] headers;

        /// <summary>
        /// Initialize CSV reader.
        /// </summary>
        /// <param name="csvFilePath">Path to the CSV file on disk</param>
        /// <param name="typeMappings">Specifies what types are stored in each column of the CSV</param>
        public CsvDataReader(string csvFilePath, List<TypeCode> typeMappings)
        {
            this.typeMappings = typeMappings;
            textReader = File.OpenText(csvFilePath);
            csvReader = new CsvReader(textReader);
            headers = csvReader.Parser.Read();
            IsClosed = false;
            RecordsAffected = 0;
        }

        public void Dispose()
        {
            Close();
            textReader.Dispose();
            csvReader.Dispose();
        }

        public string GetName(int i)
        {
            return headers[i];
        }

        public string GetDataTypeName(int i)
        {
            return GetFieldType(i).Name;
        }

        public Type GetFieldType(int i)
        {
            switch (typeMappings[i])
            {
                case TypeCode.Boolean:
                    return typeof(bool);
                case TypeCode.Byte:
                    return typeof(byte);
                case TypeCode.Char:
                    return typeof(char);
                case TypeCode.DateTime:
                    return typeof(DateTime);
                case TypeCode.DBNull:
                    return typeof(DBNull);
                case TypeCode.Decimal:
                    return typeof(decimal);
                case TypeCode.Double:
                    return typeof(double);
                case TypeCode.Empty:
                    return null;
                case TypeCode.Int16:
                    return typeof(short);
                case TypeCode.Int32:
                    return typeof(int);
                case TypeCode.Int64:
                    return typeof(long);
                case TypeCode.Object:
                    return typeof(object);
                case TypeCode.SByte:
                    return typeof(sbyte);
                case TypeCode.Single:
                    return typeof(Single);
                case TypeCode.String:
                    return typeof(string);
                case TypeCode.UInt16:
                    return typeof(UInt16);
                case TypeCode.UInt32:
                    return typeof(UInt32);
                case TypeCode.UInt64:
                    return typeof(UInt64);
            }

            return null;
        }

        public object GetValue(int i)
        {
            TypeCode typeCode = typeMappings[i];
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return GetBoolean(i);
                case TypeCode.Char:
                    return GetChar(i);
                case TypeCode.Byte:
                    return GetByte(i);
                case TypeCode.Int16:
                    return GetInt16(i);
                case TypeCode.Int32:
                    return GetInt32(i);
                case TypeCode.Int64:
                    return GetInt64(i);
                case TypeCode.Single:
                    return GetFloat(i);
                case TypeCode.Double:
                    return GetDouble(i);
                case TypeCode.Decimal:
                    return GetDecimal(i);
                case TypeCode.DateTime:
                    return GetDateTime(i);
                default:
                    return GetString(i);
            }
        }

        public int GetValues(object[] values)
        {
            CurrentRow.CopyTo(values, 0);
            return values.Length;
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool GetBoolean(int i)
        {
            var value = GetString(i);
            if (value == "1")
            {
                return true;
            }
            if (value == "0")
            {
                return false;
            }

            return Convert.ToBoolean(value);
        }

        public byte GetByte(int i)
        {
            return Byte.Parse(CurrentRow[i]);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return Char.Parse(CurrentRow[i]);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            return Guid.Parse(CurrentRow[i]);
        }

        public short GetInt16(int i)
        {
            return short.Parse(CurrentRow[i]);
        }

        public int GetInt32(int i)
        {
            return int.Parse(CurrentRow[i]);
        }

        public long GetInt64(int i)
        {
            return long.Parse(CurrentRow[i]);
        }

        public float GetFloat(int i)
        {
            return float.Parse(CurrentRow[i]);
        }

        public double GetDouble(int i)
        {
            return double.Parse(CurrentRow[i]);
        }

        public string GetString(int i)
        {
            return CurrentRow[i];
        }

        public decimal GetDecimal(int i)
        {
            return decimal.Parse(CurrentRow[i]);
        }

        public DateTime GetDateTime(int i)
        {
            return DateTime.Parse(CurrentRow[i]);
        }

        public IDataReader GetData(int i)
        {
            return this;
        }

        public bool IsDBNull(int i)
        {
            return string.IsNullOrEmpty(CurrentRow[i]);
        }

        public int FieldCount => headers.Length;

        object IDataRecord.this[int i] => CurrentRow[i];

        object IDataRecord.this[string name] => CurrentRow[GetOrdinal(name)];

        public void Close()
        {
            if (IsClosed)
            {
                return;
            }
            textReader.Close();
            IsClosed = true;
        }

        public DataTable GetSchemaTable()
        {
            DataTable t = new DataTable();
            foreach (var header in headers)
            {
                t.Columns.Add(header);
            }
            return t;
        }

        public bool NextResult()
        {
            return Read();
        }

        public bool Read()
        {
            bool isRead = csvReader.Read();
            if (isRead)
            {
                RecordsAffected++;
            }
            return isRead;
        }

        public int Depth => 0;

        public bool IsClosed { get; private set; }

        public int RecordsAffected { get; private set; }

        private string[] CurrentRow => csvReader.Parser.Context.Record;
    }
}
