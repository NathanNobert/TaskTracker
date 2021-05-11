using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TaskTrackerData
{
    public static class SqlDataReaderExtensions
    {
        public static string GetSafeString(this IDataReader rdr, string colName)
        {
            var dbVal = rdr.CheckDbNull<string>(rdr[colName]);
            return !String.IsNullOrEmpty(dbVal) ? dbVal.Trim() : String.Empty;
        }

        public static char GetSafeChar(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<char>(rdr[colName]);
        }

        public static char? GetSafeCharNullable(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<char?>(rdr[colName]);
        }

        public static string GetSafeStringNotNull(this SqlDataReader rdr, string colName)
        {
            var dbVal = rdr.CheckDbNull<string>(rdr[colName]);
            return dbVal != null ? dbVal.Trim() : "";
        }

        public static int GetSafeInt(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<int>(rdr[colName]);
        }

        public static int? GetSafeIntNullable(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<int?>(rdr[colName]);
        }

        public static bool GetSafeBool(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<bool>(rdr[colName]);
        }

        public static bool? GetSafeBoolNullable(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<bool?>(rdr[colName]);
        }

        public static DateTime GetSafeDateTime(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<DateTime>(rdr[colName]);
        }

        public static DateTime? GetSafeDateTimeNullable(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<DateTime?>(rdr[colName]);
        }

        public static decimal GetSafeDecimal(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<decimal>(rdr[colName]);
        }

        public static decimal? GetSafeDecimalNullable(this IDataReader rdr, string colName)
        {
            return rdr.CheckDbNull<decimal?>(rdr[colName]);
        }

        public static decimal GetSafeDecimalFromFloat(this IDataReader rdr, string colName)
        {
            return (decimal)rdr.CheckDbNull<double>(rdr[colName]);
        }

        public static decimal? GetSafeDecimalNullableFromFloat(this IDataReader rdr, string colName)
        {
            return (decimal?)rdr.CheckDbNull<double?>(rdr[colName]);
        }

        private static T CheckDbNull<T>(this IDataReader rdr, object obj)
        {
            return (obj == DBNull.Value ? default(T) : (T)obj);
        }

        public static bool HasColumn(this IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }



    }
}
