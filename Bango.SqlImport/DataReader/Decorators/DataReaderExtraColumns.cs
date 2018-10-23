using System;
using System.Collections.Generic;
using System.Data;

namespace Bango.SqlImport.DataReader.Decorators
{
    /// <summary>
    /// Decorator providing a way of adding additional column values to the decorated reader.
    /// </summary>
    public class DataReaderExtraColumns : DataReaderDecoratorBase
    {
        private readonly List<string> extraColumnNames;
        private readonly List<object> extraColumnValues;

        public DataReaderExtraColumns(IDataReader dataReaderInner) : base(dataReaderInner)
        {
            this.extraColumnNames = new List<string>();
            this.extraColumnValues = new List<object>();
        }

        /// <summary>
        /// Use to add an extra column from the right side of the decorated datareader, with a precalculated value.
        /// </summary>
        public void AddExtraColumn(string columnName, object columnValue)
        {
            this.extraColumnNames.Add(columnName);
            this.extraColumnValues.Add(columnValue);
        }
        
        public override string GetName(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return this.extraColumnNames[i];
            }

            return base.GetName(i);
        }

        public override Type GetFieldType(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return this.extraColumnValues[i].GetType();
            }

            return base.GetFieldType(i);
        }

        public override object GetValue(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return this.extraColumnValues[i];
            }

            return base.GetValue(i);
        }

        public override int GetValues(object[] values)
        {
            return base.GetValues(values) + extraColumnValues.Count;
        }

        public override int GetOrdinal(string name)
        {
            int result = base.GetOrdinal(name);
            if (result == -1)
            {
                result = extraColumnNames.IndexOf(name);
                if (result == -1)
                {
                    return result;
                }

                return base.FieldCount + result;
            }

            return result;
        }

        public override bool GetBoolean(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (bool) extraColumnValues[i];
            }

            return base.GetBoolean(i);
        }

        public override byte GetByte(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (byte)extraColumnValues[i];
            }

            return base.GetByte(i);
        }

        public override char GetChar(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (char)extraColumnValues[i];
            }

            return base.GetChar(i);
        }

        public override Guid GetGuid(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (Guid)extraColumnValues[i];
            }

            return base.GetGuid(i);
        }

        public override short GetInt16(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (short)extraColumnValues[i];
            }

            return base.GetInt16(i);
        }

        public override int GetInt32(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (int)extraColumnValues[i];
            }

            return base.GetInt32(i);
        }

        public override long GetInt64(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (long)extraColumnValues[i];
            }

            return base.GetInt64(i);
        }

        public override float GetFloat(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (float)extraColumnValues[i];
            }

            return base.GetFloat(i);
        }

        public override double GetDouble(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (double)extraColumnValues[i];
            }

            return base.GetDouble(i);
        }

        public override string GetString(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (string)extraColumnValues[i];
            }

            return base.GetString(i);
        }

        public override decimal GetDecimal(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (decimal)extraColumnValues[i];
            }

            return base.GetDecimal(i);
        }

        public override DateTime GetDateTime(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return (DateTime)extraColumnValues[i];
            }

            return base.GetDateTime(i);
        }

        public override bool IsDBNull(int i)
        {
            if (i >= base.FieldCount)
            {
                i -= base.FieldCount;
                return extraColumnValues[i] == null;
            }

            return base.IsDBNull(i);
        }

        public override int FieldCount => base.FieldCount + extraColumnNames.Count;

        public override object this[int i]
        {
            get
            {
                if (i >= base.FieldCount)
                {
                    i -= base.FieldCount;
                    return extraColumnValues[i];
                }

                return base[i];
            }
        }

        public override object this[string name]
        {
            get
            {
                var ordinal = base.GetOrdinal(name);
                if (ordinal != -1)
                {
                    return base[name];
                }

                return extraColumnValues[GetOrdinal(name)];
            }
        }

        public override DataTable GetSchemaTable()
        {
            var t = base.GetSchemaTable();
            foreach (var header in extraColumnNames)
            {
                if (!t.Columns.Contains(header))
                {
                    t.Columns.Add(header);
                }
            }
            return t;
        }
    }
}