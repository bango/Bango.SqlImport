using System;
using System.Collections.Generic;
using System.IO;
using Bango.SqlImport.DataReader;
using Bango.SqlImport.DataReader.Configuration;
using Bango.SqlImport.DataReader.Csv;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bango.SqlImport.Tests
{
    [Ignore("These are end to end tests. They rely on the SQL connectivity and the test table to run. Please, refer to ReadMe.md for details.")]
    public class SystemTests
    {
        private BulkCopyUtility bulkCopyUtility;
        private string testTableName;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.bulkCopyUtility = new BulkCopyUtility("{yourConnectionString}");
            testTableName = "TransactionImport";
        }
        
        [TestCase(@"Files\CsvFileToImport_NoTransformation.csv")]
        public void BulkCopy_ManualReaderCreation(string csvToImportFilePath)
        {
            var dataReader = new CsvDataReaderExtraColumns(csvToImportFilePath,
                new List<TypeCode>(5)
                {
                    TypeCode.String,
                    TypeCode.Decimal,
                    TypeCode.DateTime,
                    TypeCode.String,
                    TypeCode.Boolean
                });
            dataReader.AddExtraColumn("ImportId", -1);
            this.bulkCopyUtility.BulkCopy(testTableName, dataReader);
        }

        [TestCase(@"Files\CsvFileToImport_ColumnMapping.csv", @"Files\CsvDataReaderConfig_ColumnMapping.json")]
        [TestCase(@"Files\CsvFileToImport_ValueTransformation.csv", @"Files\CsvDataReaderConfig_ValueTransformation.json")]
        public void BulkCopy_ConfigurationDrivenReaderCreation(string csvToImportFilePath, string csvConfigurationFilePath)
        {
            string testConfig = File.ReadAllText(csvConfigurationFilePath);
            var config = JsonConvert.DeserializeObject<CsvDataReaderConfiguration>(testConfig);
            config.CsvFilePath = csvToImportFilePath;

            var dataReader = new DataReaderFactory().GetCsvDataReader(config);
            this.bulkCopyUtility.BulkCopy(testTableName, dataReader);
        }
    }
}
