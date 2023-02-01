using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;

namespace RekrutacjaApp.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public required string DTOName { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments[DTOName] == null) context.Result = new BadRequestObjectResult("Object is null");

            if (!context.ModelState.IsValid) context.Result = new BadRequestObjectResult("Object is incorrect");
        }
    }
}
