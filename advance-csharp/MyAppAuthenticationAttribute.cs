using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace advance_csharp
{
    public class MyAppAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Role
        /// </summary>
        public string Role;

        /// <summary>
        /// MyAppAuthenticationAttribute
        /// </summary>
        /// <param name="role"></param>
        public MyAppAuthenticationAttribute(string role)
        {
            Role = role;
        }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!string.IsNullOrEmpty(Role))
            {
                if (Role != "Admin")
                {
                    context.Result = new UnauthorizedObjectResult("user is unauthorized");
                }
            }
        }
    }
}