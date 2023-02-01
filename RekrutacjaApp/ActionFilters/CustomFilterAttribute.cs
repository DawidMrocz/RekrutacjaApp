using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace RekrutacjaApp.ActionFilters
{
    public class IdProvidedValidationAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool exist = context.ActionArguments.ContainsKey("id");
            if (!exist)
            {
                context.Result = new BadRequestObjectResult("ID not found");
                return;
            }
        }
    }
}
