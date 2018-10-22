using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Bango.SqlImport.DataReader.Configuration;

namespace Bango.SqlImport.DataReader.Decorators
{
    public class DataReaderValueTransformation : DataReaderDecoratorBase
    {
        private readonly Dictionary<int, Dictionary<string, object>> transformationParameterMappings;

        /// <summary>
        /// Creates the decorator which transforms the values retrieved by index according to parameters provided.
        /// </summary>
        /// <param name="transformationParameterMappings">Dictionary mapping column index to a dictionary of transformation parameters</param>
        /// <param name="dataReaderInner">DataReader being decorated</param>
        public DataReaderValueTransformation(Dictionary<int, Dictionary<string, object>> transformationParameterMappings, IDataReader dataReaderInner) : base(dataReaderInner)
        {
            if (transformationParameterMappings.Any(m => m.Value == null || !m.Value.Any()))
            {
                throw new ArgumentException("Transformation parameters are required for transformation.");
            }
            this.transformationParameterMappings = transformationParameterMappings;
        }

        public override bool IsDBNull(int i)
        {
            if (transformationParameterMappings.ContainsKey(i))
            {
                return false;
            }

            return base.IsDBNull(i);
        }

        public override object GetValue(int i)
        {
            if (!transformationParameterMappings.ContainsKey(i))
            {
                return base.GetValue(i);
            }

            Type type = GetFieldType(i);
            if (type == typeof(bool))
            {
                return GetBoolean(i);
            }
            if (type == typeof(DateTime))
            {
                return GetDateTime(i);
            }

            throw new Exception($"No method handling data type {type.Name} has been added.");
        }

        public override DateTime GetDateTime(int i)
        {
            var transformationParams = new Dictionary<string, object>();

            if (!IsTransformationRequired(i, out transformationParams))
            {
                return base.GetDateTime(i);
            }

            var format = (string)GetParamValueOrDefault(transformationParams, ParamKeys.DateTimeFormat, string.Empty);
            var timeZoneId = (string)GetParamValueOrDefault(transformationParams, ParamKeys.DateTimeTimeZoneId, "UTC");

            var dateTimeBeforeConversion = string.IsNullOrEmpty(format)
                ? DateTime.Parse(GetString(i))
                : DateTime.ParseExact(GetString(i), format, CultureInfo.InvariantCulture);

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeBeforeConversion, timeZoneInfo);
        }

        public override bool GetBoolean(int i)
        {
            var transformationParams = new Dictionary<string, object>();

            if (!IsTransformationRequired(i, out transformationParams))
            {
                return base.GetBoolean(i);
            }

            var trueValue = (string)GetParamValueOrDefault(transformationParams, ParamKeys.BoolTrueValue, string.Empty);
            //No non-standard parameter provided for the value of 'true'. Default to base implementation.
            if (string.IsNullOrEmpty(trueValue))
            {
                return base.GetBoolean(i);
            }

            return GetString(i) == trueValue;
        }

        private static object GetParamValueOrDefault(Dictionary<string, object> transformationParams, string key, object defaultValue)
        {
            return transformationParams.ContainsKey(key) ? transformationParams[key] : defaultValue;
        }

        private bool IsTransformationRequired(int columnIndex, out Dictionary<string, object> transformationParameters)
        {
            if (transformationParameterMappings.ContainsKey(columnIndex))
            {
                transformationParameters = transformationParameterMappings[columnIndex];
                return true;
            }

            transformationParameters = null;
            return false;
        }
    }
}
