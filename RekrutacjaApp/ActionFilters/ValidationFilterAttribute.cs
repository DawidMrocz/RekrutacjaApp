using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;

namespace RekrutacjaApp.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(context.ActionArguments.ToString());
            Console.WriteLine(context.ActionArguments.ToString());
            Console.WriteLine(context.ActionArguments.ToString());

            Console.WriteLine(context.ActionArguments.ToString());

            Console.WriteLine(context.ActionArguments.ToString());
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is IEntityType);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult("Object is null :/");
                
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult("Object is incorrect");
            }
        }
    }
}
