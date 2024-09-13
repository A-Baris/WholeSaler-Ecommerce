using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WholeSaler.Web.Models.ViewModels.Attributes;

namespace WholeSaler.Web.Helpers.HtmlHelper
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent GenerateFormFields<TModel>(
            this IHtmlHelper htmlHelper,
            TModel modelInstance,
            params string[] propertiesToExclude)
        {
            var properties = modelInstance.GetType()
                .GetProperties()
                .Where(p => !propertiesToExclude.Contains(p.Name))
                .OrderBy(p => p.GetCustomAttribute<DisplayOrderAttribute>()?.Order ?? int.MaxValue)
                .ToList();

            var prefix = modelInstance.GetType().Name;

            var content = new HtmlContentBuilder();

            foreach (var property in properties)
            {
                if (propertiesToExclude.Contains(property.Name))
                {
                    continue;
                }

                var propertyName = property.Name;
                var displayName = property.GetCustomAttribute<DisplayAttribute>()?.Name ?? propertyName;
                var propertyPath = $"{prefix}.{propertyName}";
                var name = $"{prefix}.{propertyName}";

                if (property.GetCustomAttribute<HiddenInputAttribute>() != null)
                {
                    content.AppendHtml($"<input type=\"hidden\" name=\"{name}\" value=\"\" />");
                    continue;
                }

                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    content.AppendHtml($"<div class=\"form-group\"><label for=\"{name}\">{displayName}</label><input type=\"number\" name=\"{name}\" class=\"form-control\" /><span data-valmsg-for=\"{name}\" class=\"text-danger\"></span></div>");
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    content.AppendHtml($"<div class=\"form-group col-md-2\"><label for=\"{propertyPath}\">{displayName}</label><select name=\"{propertyPath}\" class=\"form-control form-control-sm\"><option value=\"false\">No</option><option value=\"true\">Yes</option></select></div>");
                }
                else
                {
                    content.AppendHtml($"<div class=\"form-group\"><label for=\"{name}\">{displayName}</label><input type=\"text\" name=\"{name}\" class=\"form-control\" /><span data-valmsg-for=\"{name}\" class=\"text-danger\"></span></div>");
                }
            }

            return content;
        }
    }
}
