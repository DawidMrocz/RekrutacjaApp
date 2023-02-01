using Microsoft.AspNetCore.Mvc.Filters;

namespace RekrutacjaApp.ActionFilters
{
    public class CustomFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("DAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAW");
              
        }
    }
}
