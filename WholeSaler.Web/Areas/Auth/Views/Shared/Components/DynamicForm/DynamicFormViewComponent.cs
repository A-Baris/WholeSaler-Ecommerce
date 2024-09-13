using Microsoft.AspNetCore.Mvc;

namespace WholeSaler.Web.Areas.Auth.Views.Shared.Components.DynamicForm
{
    public class DynamicFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(object model)
        {
            return View("_DynamicForm", model);
        }
        public IViewComponentResult InvokeForCreateFunction(object model)
        {
            return View("_DynamicFormForCreate", model);
        }


    }
}
