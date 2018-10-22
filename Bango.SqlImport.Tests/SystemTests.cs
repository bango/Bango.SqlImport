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
    [Ignore("These are end to end tests dependent on files and database being accessible.")]
    public class SystemTests
    {
        private BulkCopyUtility bulkCopyUtility;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.bulkCopyUtility = new BulkCopyUtility("{yourConnectionString}");
        }
        
        [Test]
        public void BulkCopy_ManualReaderCreation()
        {
            //Act
            var dataReader = new CsvDataReaderExtraColumns("{yourFile}",
                new List<TypeCode>(5)
                {
                    TypeCode.String,
                    TypeCode.Decimal,
                    TypeCode.String,
                    TypeCode.Boolean,
                    TypeCode.DateTime
                });
            dataReader.AddExtraColumn("{ExtraColumnName}", -1);
            this.bulkCopyUtility.BulkCopy("{YourTableName}", dataReader);

            //Assert - you can write your asserts on the table here
        }

        [Test]
        public void BulkCopy_ConfigurationDrivenReaderCreation()
        {
            //Arrange
            string testConfig = File.ReadAllText("{YourConfigJsonFilePath}");
            var config = JsonConvert.DeserializeObject<CsvDataReaderConfiguration>(testConfig);
            config.CsvFilePath = "{YourFilePath}";

            //Act
            var dataReader = new DataReaderFactory().GetCsvDataReader(config);
            this.bulkCopyUtility.BulkCopy("", dataReader);

            //Assert - you can write your asserts on the table here
        }
    }
}
