using Microsoft.AspNetCore.WebUtilities;

namespace WholeSaler.Web.Helpers.QueryHelper
{
    public class QueryHelper
    {
        public static T DeserializeQueryString<T>(string queryString) where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();

            var queryDictionary = QueryHelpers.ParseQuery(queryString);

            foreach (var property in properties)
            {
                if (queryDictionary.TryGetValue(property.Name, out var value))
                {
                    var convertedValue = Convert.ChangeType(value.ToString(), property.PropertyType);
                    property.SetValue(obj, convertedValue);
                }
            }

            return obj;
        }
    }
}
