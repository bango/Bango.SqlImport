using System.Collections.Generic;

namespace Bango.SqlImport.DataReader.Configuration
{
    public class CsvDataReaderConfiguration
    {
        public string CsvFilePath { get; set; }

        public bool CsvHasHeaderRow { get; set; }

        public int CsvColumnsCount { get; set; }

        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
