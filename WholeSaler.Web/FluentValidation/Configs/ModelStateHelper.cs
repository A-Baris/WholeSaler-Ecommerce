using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WholeSaler.Web.FluentValidation.Configs
{
    public static class ModelStateHelper
    {
        public static void AddErrorsToModelState(ModelStateDictionary modelState, IEnumerable<ValidationError> errors)
        {
            modelState.Clear();
            foreach (var error in errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
