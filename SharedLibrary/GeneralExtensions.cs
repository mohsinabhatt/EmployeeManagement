using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SharedLibrary
{
    public static class GeneralExtensions
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrWhiteSpace(value);


        public static bool IsNotNullOrEmpty(this string value) => !value.IsNullOrEmpty();


        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list) => list == null || !list.Any();


        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> list) => !list.IsNullOrEmpty();


        public static bool IsEmpty(this Guid value) => value == default;


        public static bool IsNotEmpty(this Guid value) => !value.IsEmpty();


        public static bool IsNullOrEmpty(this Guid? value) => value == default || value == default(Guid);


        public static bool IsNotNullOrEmpty(this Guid? value) => !value.IsNullOrEmpty();


        public static string ToValueString(this Enum value) => Convert.ToInt16(value).ToString();


        public static string DisplayName(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttribute<DisplayAttribute>();
            return attr != null ? attr.Name : value.ToString();
        }


        public static IEnumerable<T> SafeAccess<T>(this IEnumerable<T> list)
        {
            return list ?? Enumerable.Empty<T>();
        }


        public static bool IsIn(this string value, params string[] list) => list.Contains(value);


        public static bool IsIn<T>(this T value, params T[] list) where T : Enum => list.Contains(value);


        public static string Serialize(this object value, JsonSerializerSettings options = null)
        {
            options ??= new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(value, options);
        }


        public static string Concat(this IEnumerable<string> list, string separator) =>
            string.Join(separator, list);


        //public static string Concat(this IEnumerable<string> list, Func<string, string> expression, string separator) =>
        //    string.Join(separator, list.Select(expression));


        public static string Concat(this IEnumerable<string> list, Func<string, string> expression, string separator)
        {
            if (list.IsNullOrEmpty())
                return null;

            string query = string.Empty;
            foreach (var item in list)
            {
                if (item.IsNullOrEmpty()) continue;
                query += separator + expression(item);
            }

            return query.Remove(0, separator.Length);
        }


        public static string Concat(this IEnumerable<Guid> list, Func<string, string> expression, string separator)
        {
            if (list.IsNullOrEmpty())
                return null;

            string query = string.Empty;
            foreach (var item in list)
            {
                if (item.IsEmpty()) continue;
                query += separator + expression(item.ToString());
            }

            return query.Remove(0, separator.Length);
        }


        public static string AddEscapeCharacters(this string value)
        {
            if (value.IsNullOrEmpty()) return value;
            return value.Replace("'", "''");
        }
    }
}
