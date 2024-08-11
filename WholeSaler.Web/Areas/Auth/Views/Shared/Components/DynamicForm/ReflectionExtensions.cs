using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace WholeSaler.Web.Areas.Auth.Views.Shared.Components.DynamicForm
{
    public static class ReflectionExtensions
    {
        public static string GetDisplayName(this PropertyInfo property)
        {
            var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                            ?? property.GetCustomAttribute<DisplayAttribute>()?.Name
                            ?? property.Name;
            return displayName;
        }

        public static string GetNestedDisplayName(this Type type, string propertyPath)
        {
            var properties = propertyPath.Split('.');
            PropertyInfo property = null;
            foreach (var propName in properties)
            {
                property = property == null
                    ? type.GetProperty(propName)
                    : property.PropertyType.GetProperty(propName);
                if (property == null)
                {
                    return propertyPath;
                }
            }
            return property.GetDisplayName();
        }
    }
}
