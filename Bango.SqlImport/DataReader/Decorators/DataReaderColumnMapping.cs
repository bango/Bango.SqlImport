using System.Collections.Generic;
using System.Data;

namespace Bango.SqlImport.DataReader.Decorators
{
    public class DataReaderColumnMapping : DataReaderDecoratorBase
    {
        private readonly Dictionary<string, int> columnMappings;

        /// <summary>
        /// Creates the decorator which exposes only the columns that are configured.
        /// </summary>
        /// <param name="columnMappings">Dictionary mapping column name to column index</param>
        /// <param name="dataReaderInner">DataReader being decorated</param>
        public DataReaderColumnMapping(Dictionary<string, int> columnMappings, IDataReader dataReaderInner) : base(dataReaderInner)
        {
            this.columnMappings = columnMappings;
        }

        public override int GetOrdinal(string name)
        {
            if (columnMappings.ContainsKey(name))
            {
                return columnMappings[name];
            }

            return -1;
        }

        public override DataTable GetSchemaTable()
        {
            DataTable t = new DataTable();
            foreach (var column in columnMappings)
            {
                t.Columns.Add(column.Key);
            }
            return t;
        }
    }
}
