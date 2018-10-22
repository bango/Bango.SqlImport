using System;
using System.Data;

namespace Bango.SqlImport.DataReader.Decorators
{
    public class DataReaderDecoratorBase : IDataReader
    {
        private readonly IDataReader dataReaderInner;

        public DataReaderDecoratorBase(IDataReader dataReaderInner)
        {
            this.dataReaderInner = dataReaderInner;
        }

        public void Dispose()
        {
            dataReaderInner.Dispose();
        }

        public virtual string GetName(int i)
        {
            return dataReaderInner.GetName(i);
        }

        public virtual string GetDataTypeName(int i)
        {
            return dataReaderInner.GetDataTypeName(i);
        }

        public virtual Type GetFieldType(int i)
        {
            return dataReaderInner.GetFieldType(i);
        }

        public virtual object GetValue(int i)
        {
            return dataReaderInner.GetValue(i);
        }

        public virtual int GetValues(object[] values)
        {
            return dataReaderInner.GetValues(values);
        }

        public virtual int GetOrdinal(string name)
        {
            return dataReaderInner.GetOrdinal(name);
        }

        public virtual bool GetBoolean(int i)
        {
            return dataReaderInner.GetBoolean(i);
        }

        public virtual byte GetByte(int i)
        {
            return dataReaderInner.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return dataReaderInner.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public virtual char GetChar(int i)
        {
            return dataReaderInner.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return dataReaderInner.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public virtual Guid GetGuid(int i)
        {
            return dataReaderInner.GetGuid(i);
        }

        public virtual short GetInt16(int i)
        {
            return dataReaderInner.GetInt16(i);
        }

        public virtual int GetInt32(int i)
        {
            return dataReaderInner.GetInt32(i);
        }

        public virtual long GetInt64(int i)
        {
            return dataReaderInner.GetInt64(i);
        }

        public virtual float GetFloat(int i)
        {
            return dataReaderInner.GetFloat(i);
        }

        public virtual double GetDouble(int i)
        {
            return dataReaderInner.GetDouble(i);
        }

        public virtual string GetString(int i)
        {
            return dataReaderInner.GetString(i);
        }

        public virtual decimal GetDecimal(int i)
        {
            return dataReaderInner.GetDecimal(i);
        }

        public virtual DateTime GetDateTime(int i)
        {
            return dataReaderInner.GetDateTime(i);
        }

        public IDataReader GetData(int i)
        {
            return dataReaderInner.GetData(i);
        }

        public virtual bool IsDBNull(int i)
        {
            return dataReaderInner.IsDBNull(i);
        }

        public virtual int FieldCount
        {
            get { return dataReaderInner.FieldCount; }
        }

        public virtual object this[int i]
        {
            get { return dataReaderInner[i]; }
        }

        public virtual object this[string name]
        {
            get { return dataReaderInner[name]; }
        }

        public void Close()
        {
            dataReaderInner.Close();
        }

        public virtual DataTable GetSchemaTable()
        {
            return dataReaderInner.GetSchemaTable();
        }

        public bool NextResult()
        {
            return dataReaderInner.NextResult();
        }

        public bool Read()
        {
            return dataReaderInner.Read();
        }

        public int Depth
        {
            get { return dataReaderInner.Depth; }
        }

        public bool IsClosed
        {
            get { return dataReaderInner.IsClosed; }
        }

        public int RecordsAffected
        {
            get { return dataReaderInner.RecordsAffected; }
        }
    }
}
