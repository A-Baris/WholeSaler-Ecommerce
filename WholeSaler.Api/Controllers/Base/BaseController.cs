using Microsoft.AspNetCore.Mvc;

namespace WholeSaler.Api.Controllers.Base;
    public abstract class BaseController : ControllerBase
    {

        protected async Task<IActionResult> ValidateAndExecute<TModel, TResult>(
       TModel model,
       Func<TModel, Task<TResult>> action,
       Func<TResult, IActionResult> onSuccess)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await action(model);

            if (result == null)
            {
                return BadRequest("Operation failed.");
            }

            return onSuccess(result);
        }



    protected async Task<IActionResult> ValidateAndExecute<TModel, TResult>(
      TModel model,
      Func<TModel, Task<TResult>> action,
      Func<TResult, Task<IActionResult>> onSuccess)
    {
        if (model == null)
        {
            return BadRequest("Model cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await action(model);

        if (result == null)
        {
            return BadRequest("Operation failed.");
        }

        return await onSuccess(result);
    }
//    protected async Task<IActionResult> ValidateAndExecute<TModel, TResult>(
//    string id,
//    Func<string, Task<TModel>> getModel,
//    Func<TModel, Task<TResult>> action,
//    Func<TResult, IActionResult> onSuccess)
//    {
//        if (string.IsNullOrEmpty(id))
//        {
//            return BadRequest("Id cannot be null or empty.");
//        }

//        var model = await getModel(id);

//        if (model == null)
//        {
//            return NotFound($"Model with Id {id} not found.");
//        }

//        if (!ModelState.IsValid)
//        {
//            return BadRequest(ModelState);
//        }

//        var result = await action(model);

//        if (result == null)
//        {
//            return BadRequest("Operation failed.");
//        }

//        return onSuccess(result);
//    }
}

