using System.Data;
using Bango.SqlImport.DataReader.Configuration;

namespace Bango.SqlImport.DataReader
{
    public interface IDataReaderFactory
    {
        IDataReader GetCsvDataReader(CsvDataReaderConfiguration configuration);
    }
}
