using System.Collections.Generic;

namespace Bango.SqlImport.DataReader.Configuration
{
    public class ColumnDefinition
    {
        public int ColumnIndex { get; set; }

        public string ColumnName { get; set; }

        public string ColumnDataType { get; set; }

        public ValueSource ValueSource { get; set; }

        public Dictionary<string, object> Params { get; set; }
    }
}