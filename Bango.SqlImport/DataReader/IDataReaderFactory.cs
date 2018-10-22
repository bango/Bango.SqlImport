using System.Data;
using Bango.Data.SqlLink.Import.DataReader.Configuration;

namespace Bango.Data.SqlLink.Import.DataReader
{
    public interface IDataReaderFactory
    {
        IDataReader GetCsvDataReader(CsvDataReaderConfiguration configuration);
    }
}
