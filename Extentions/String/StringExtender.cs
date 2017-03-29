using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace CB.CSharp.Extentions
{
    public static class StringExtender
    {
        public static string ToJoinedString(this IEnumerable<string> StringList, string Delimiter = ",") =>
            string.Join(Delimiter, StringList);

        public static DateTime? TryParse(this string stringDate)
        {
            DateTime date;

            return DateTime.TryParseExact(stringDate, "dd'/'MM'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ? date : (DateTime?)null;
        }

        public static double TryParseToDouble(this string StringValue) =>
            Convert.ToDouble(StringValue, CultureInfo.GetCultureInfo("nl-BE").NumberFormat);

        public static string UntilChar(this string String, char Delimiter)
        {
            int i = String.IndexOf(Delimiter);
            if (i > -1)
                return String.Substring(0, i - 1);
            return String;
        }

        public static Stream GetStreamFromUrl(this string stringUrl) =>
          WebRequest
            .Create(stringUrl)
            .GetResponse()
            .GetResponseStream();

        public static string CutIfNotNull(this string value, int lenght) =>
            value?.Substring(0, lenght);

        public static string IfNull(this string String, string NullValue = "") =>
             string.IsNullOrEmpty(String) ? NullValue : String;

        public static string IfNotNull(this string String, string NotNullValue) =>
            !string.IsNullOrEmpty(String) ? NotNullValue : String;

        public static string IfNotNull(this object Obj, string NotNullValue) =>
          Obj != null ? NotNullValue : null;

        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
}