namespace Bango.SqlImport.DataReader.Configuration
{
    public static class ParamKeys
    {
        /// <summary>
        /// Indicates which value is to be interpreted as true.
        /// </summary>
        public const string BoolTrueValue = "trueValue";

        /// <summary>
        /// Format of the datetime column.
        /// </summary>
        public const string DateTimeFormat = "format";

        /// <summary>
        /// Zone id of the datetime column. For the list of valid ids see: TimeZoneInfo.GetSystemTimeZones()
        /// or this website: https://stackoverflow.com/questions/7908343/list-of-timezone-ids-for-use-with-findtimezonebyid-in-c
        /// </summary>
        public const string DateTimeTimeZoneId = "timeZoneId";

        /// <summary>
        /// Static value of the column
        /// </summary>
        public const string Value = "value";
    }
}
