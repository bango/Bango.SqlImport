using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bango.SqlImport.DataReader.Configuration;
using Bango.SqlImport.DataReader.Csv;
using Bango.SqlImport.DataReader.Decorators;

namespace Bango.SqlImport.DataReader
{
    public class DataReaderFactory : IDataReaderFactory
    {
        public IDataReader GetCsvDataReader(CsvDataReaderConfiguration configuration)
        {
            ValidateConfiguration(configuration);
            SanitizeConfiguration(configuration);

            //Create the base CSV data reader
            IDataReader dataReader = new CsvDataReader(configuration.CsvFilePath, GetTypeCodes(configuration));

            //Add decorators
            dataReader = AddColumnMapping(configuration, dataReader);
            dataReader = AddValueTransformation(configuration, dataReader);
            dataReader = AddExtraColumns(configuration, dataReader);

            return dataReader;
        }

        private static IDataReader AddExtraColumns(CsvDataReaderConfiguration configuration, IDataReader dataReader)
        {
            var extraColumns = configuration.ColumnDefinitions.Where(cd => cd.ValueSource == ValueSource.StaticValue).ToList();

            //Decorate with extra columns if the are any columns with static values
            if (extraColumns.Count > 0)
            {
                var extraColumnsDataReader = new DataReaderExtraColumns(dataReader);
                foreach (var extraColumn in extraColumns)
                {
                    extraColumnsDataReader.AddExtraColumn(extraColumn.ColumnName, extraColumn.Params[ParamKeys.Value]);
                }

                dataReader = extraColumnsDataReader;
            }

            return dataReader;
        }

        private static IDataReader AddValueTransformation(CsvDataReaderConfiguration configuration, IDataReader dataReader)
        {
            var transformationParamsMappings = new Dictionary<int, Dictionary<string, object>>(configuration.ColumnDefinitions.Count);
            configuration.ColumnDefinitions.Where(cd => cd.ValueSource == ValueSource.ColumnValue && cd.Params != null && cd.Params.Count > 0).ToList()
                .ForEach(cd => transformationParamsMappings.Add(cd.ColumnIndex, cd.Params));

            //Decorate with value transformation if there are any columns with custom parameters
            if (transformationParamsMappings.Count > 0)
            {
                dataReader = new DataReaderValueTransformation(transformationParamsMappings, dataReader);
            }

            return dataReader;
        }

        private static IDataReader AddColumnMapping(CsvDataReaderConfiguration configuration, IDataReader dataReader)
        {
            //Decorate with column mapping functionality - only for columns that require the mapping
            var columnValueMappings = new Dictionary<string, int>(configuration.ColumnDefinitions.Count);
            configuration.ColumnDefinitions.Where(cd => cd.ColumnIndex >= 0).ToList()
                .ForEach(cd => columnValueMappings.Add(cd.ColumnName, cd.ColumnIndex));
            if (columnValueMappings.Count > 0)
            {
                dataReader = new DataReaderColumnMapping(columnValueMappings, dataReader);
            }

            return dataReader;
        }

        private void ValidateConfiguration(CsvDataReaderConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration?.CsvFilePath))
            {
                throw new ArgumentException("Configuration invalid. FilePath is required.");
            }
            if (configuration.CsvColumnsCount <= 0)
            {
                throw new ArgumentException("Configuration invalid. ColumnsCount needs to be positive.");
            }
            if (configuration.ColumnDefinitions == null || !configuration.ColumnDefinitions.Any())
            {
                throw new ArgumentException("Configuration invalid. CsvColumnDefinitions are required.");
            }
            if (configuration.ColumnDefinitions.Any(d => string.IsNullOrWhiteSpace(d.ColumnName) || string.IsNullOrWhiteSpace(d.ColumnDataType)))
            {
                throw new ArgumentException("Configuration invalid. ColumnName and ColumnDataType fields are required.");
            }
            if (configuration.ColumnDefinitions.Where(cd => cd.ValueSource != ValueSource.StaticValue).Select(cd => cd.ColumnIndex).Distinct().Count() !=
                configuration.ColumnDefinitions.Count(cd => cd.ValueSource != ValueSource.StaticValue))
            {
                throw new ArgumentException("Configuration invalid. ColumnIndex field has to have a unique value for non-static value sources.");
            }
            if (configuration.ColumnDefinitions.Select(cd => cd.ColumnName).Distinct().Count() !=
                configuration.ColumnDefinitions.Count)
            {
                throw new ArgumentException("Configuration invalid. ColumnName field has to have a unique value.");
            }
            var staticValueColumns = configuration.ColumnDefinitions
                .Where(cd => cd.ValueSource == ValueSource.StaticValue).ToList();
            staticValueColumns.ForEach(cd =>
            {
                if (cd.Params == null || !cd.Params.ContainsKey(ParamKeys.Value))
                {
                    throw new ArgumentException("Configuration invalid. Static value columns have to have a value parameter.");
                }
            });
        }

        private static void SanitizeConfiguration(CsvDataReaderConfiguration configuration)
        {
            //Set all the static value indexes to -1 as the index does not matter for static values.
            var staticValueColumns = configuration.ColumnDefinitions.Where(cd => cd.ValueSource == ValueSource.StaticValue).ToList();
            if (staticValueColumns.Any())
            {
                staticValueColumns.ForEach(cd => cd.ColumnIndex = -1);
            }
        }

        private static List<TypeCode> GetTypeCodes(CsvDataReaderConfiguration configuration)
        {
            var typeCodes = new List<TypeCode>(configuration.CsvColumnsCount);
            for (int i = 0; i < configuration.CsvColumnsCount; i++)
            {
                var csvColumn = configuration.ColumnDefinitions.SingleOrDefault(c => c.ColumnIndex == i);

                //If a column is not required, then default to empty data type.
                typeCodes.Add(csvColumn == null ? TypeCode.Empty : GetTypeCode(csvColumn.ColumnDataType));
            }
            return typeCodes;
        }

        private static TypeCode GetTypeCode(string typeName)
        {
            TypeCode typeCode;

            if (Enum.TryParse(typeName, out typeCode))
            {
                return typeCode;
            }

            throw new ArgumentException("Configuration invalid. TypeName field has an invalid value. For valid values look at System.TypeCode implementation.");
        }
    }
}