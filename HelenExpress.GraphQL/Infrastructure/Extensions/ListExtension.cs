using System;
using System.Collections.Generic;
using System.Text;

namespace HelenExpress.GraphQL.Infrastructure.Extensions
{
    public static class ListExtension
    {
        public static string ToCsv<T>(this List<T> genericList, IDictionary<string, string> headerMappings, char separator = ';')
        {
            var sb = new StringBuilder();
            
            // create header
            var info = typeof(T).GetProperties();
            var header = new List<string>();
            foreach (var prop in typeof(T).GetProperties())
            {
                var headerName = prop.Name;
                if (headerMappings.ContainsKey(prop.Name)) {
                    headerName = headerMappings[prop.Name];
                }

                header.Add(headerName);
            }
            sb.AppendLine(string.Join(separator, header));
            
            // create row
            foreach (var obj in genericList)
            {
                var values = new List<string>();
                foreach (var prop in info)
                {
                    var value = prop.GetValue(obj, null);
                    values.Add(Convert.ToString(value));
                }
                sb.AppendLine(string.Join(separator, values));
            }

            return sb.ToString();
        }
    }
}