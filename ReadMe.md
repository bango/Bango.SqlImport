﻿# Bango.SqlImport

The purpose of this project is to provide a reliable, flexible and configuration driven way of importing data from data source (e.g. CSV file) to SQL database table.
One of the common problems with data import is loading the entire dataset from file into memory before making a SQL insert call (e.g. using a DataTable class).
This is dangerous because if the dataset is big the process can run out of memory on application side or SQL side (one massive insert transaction).
This can be avoided by streaming data from the source to the destination table using an implementation of IDataReader.
This project provides this functionality and adds additional configuration options on top of it (see below).

# Configuration and usage
For examples in code, go to Bango.SqlImport.Tests, SystemTests. Refer to the ReadMe file of that project.
The main problem you will face is how to create the IDataReader implementation which reads from your data source and implements the features you require.

## Supported IDataReader sources
* CSV

## Additional IDataReader features
* Column mapping: map different columns in the source (by index) to different columns in the destination (by table column name).
* Additional, static column values: enrich data from source with additional columns with static values.
* Value transformation: transform data from the source before it is inserted into the destination. Supported transformations:
	* DateTime: local timezone to UTC
	* Boolean: custom string parsing

## Creating the right IDataReader implementation
You can create the implementation explicitly in code or use a configuration driven approach.

### Create IDataReader implementation in code
#### Csv matches your table structure
If your csv file matches 1 to 1 the table in the database:

```csharp
//Instantiate the reader, providing the list of columns which matches 1 to 1 the data table structure.
var dataReader = new CsvDataReader(filePath,
                new List<TypeCode>(5)
                {
                    TypeCode.String,
                    TypeCode.Decimal,
                    TypeCode.String,
                    TypeCode.Boolean,
                    TypeCode.DateTime
                });

bulkCopyUtility.BulkCopy("TableName", dataReader);
```

#### Csv matches your table structure, additional columns needed
If your csv file matches 1 to 1 the table in the database, but you want to add some extra columns with static values:

```csharp
//Instantiate the reader with extra columns capability, providing the list of columns which matches 1 to 1 the data table structure.
var dataReader = new CsvDataReaderExtraColumns(filePath,
                new List<TypeCode>(5)
                {
                    TypeCode.String,
                    TypeCode.Decimal,
                    TypeCode.String,
                    TypeCode.Boolean,
                    TypeCode.DateTime
                });
            dataReader.AddExtraColumn("ExtraColumnName", -1); //Add the extra columns with static values by calling this method.
            bulkCopyUtility.BulkCopy("TableName", dataReader);
```

### Create IDataReader implementation based on configuration json
If your case is more complex and you want to handle a mixture of additional requirements,
(e.g.: custom mapping csv columns to data table columns, adding static values, value transformation), you can use the configuration driven approach.
The easiest way to do it, is to form a json document and deserialize it to the configuration object.
You can then for example store the configuration json in a table in a database and make changes to your import process without code deployments.
The json structure is defined as follows:

```csharp
var configurationText = "{
  "csvHasHeaderRow": true, //Required. True if there's a header row in the CSV file.
  "csvColumnsCount": 5, //Required. Has to match how many rows your CSV file has (not the destination table).
  "columnDefinitions": [ //Required. You should have as many definitions as you have columns in the destination table. You don't need the definitions for the columns in the CSV file that are not required for your table.
    {
      "columnIndex": 2, //Required if you want to read this value from csv file (see valueSource below). Should match the index of the column in the CSV file. 0 based.
      "columnName": "Date", //Required. Should match the name of the destination table's column name. CAUTION! It's case sensitive!
      "columnDataType": "DateTime", //Required. Describes the data type of the column. The value should match one of the System.TypeCode enum values.
      "valueSource": "ColumnValue", //Required. Describes where to read the data from. ColumnValue - read the value from source (e.g. csv file), StaticValue - provide the value in the params field.
      "params": { //Required if the valueSource = "StaticValue" or you want to transform the value from the source. For the list of potential keys, check ParamKeys class.
        "format": "yyyyMMddHHmm", //Describes the format of the date field in the source.
        "timeZoneId": "Tokyo Standard Time" //Describes the time zone of the date field in the source. For the list of time zones, check TimeZoneInfo.GetSystemTimeZones() or go to this website: https://stackoverflow.com/questions/7908343/list-of-timezone-ids-for-use-with-findtimezonebyid-in-c
      }
    },
    {
      "columnIndex": 3,
      "columnName": "Amount",
      "columnDataType": "Decimal",
      "valueSource": "ColumnValue"
	  //No params in this case as no transformation is required.
    },
    {
      "columnIndex": 4,
      "columnName": "IsValid",
      "columnDataType": "Boolean",
      "valueSource": "ColumnValue",
      "params": {
        "trueValue": "M" //We want to transform the value from the source: if it's 'M' -> true. Else -> false.
      }
    },
    {
      "columnIndex": 1,
      "columnName": "TransactionId",
      "columnDataType": "String",
      "valueSource": "ColumnValue"
    },
    {//Note there's no columnIndex. That's because the valueSource = "StaticValue", we won't be reading this value from the CSV file.
      "columnName": "PayeeName",
      "columnDataType": "String",
      "valueSource": "StaticValue", //We want to provide a static value for this column in the destination table. This means that there has to be a "value" param entry.
      "params": {
        "value": "Jon" //The static value we want for this column in the destination table is "Jon".
      }
    },
    {
      "columnName": "ImportId",
      "columnDataType": "Int32",
      "valueSource": "StaticValue",
      "params": {
        "value": 123 //The static value we want for this column in the destination table is 123. No quotes because the data type is Int32.
      }
    }
  ]
}";

//Deserialize to config object
var config = Newtonsoft.Json.JsonConvert.DeserializeObject<CsvDataReaderConfiguration>(configurationText);

//Get data reader using the factory
var dataReader = new DataReaderFactory().GetCsvDataReader(config);

bulkCopyUtility.BulkCopy("tableName", dataReader);
```

## Best practices
The destination table in SQL should be as minimal as possible - avoid using indexes. Primary keys should be handled fine.
If possible, the database's recovery mode should be Simple or Bulk Logged.

# Built with
* CsvHelper
* Newtonsoft.Json

# Authors
* Michal Ciesielski

# License
See LICENSE file.