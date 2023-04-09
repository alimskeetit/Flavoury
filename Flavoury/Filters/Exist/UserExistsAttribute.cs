using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flavoury.Filters.Exist
{
    public class UserExistsAttribute: Attribute, IActionFilter
    {
        private readonly string _id;

        public UserExistsAttribute(string id = "id")
        {
            _id = id;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var service = (UserManager<User>)context.HttpContext.RequestServices.GetService(typeof(UserManager<User>))!;
            var userId = (string)context.ActionArguments[_id]!;

            if (service.FindByIdAsync(userId).GetAwaiter().GetResult() == null)
                context.Result = new NotFoundObjectResult($"Пользователь с id {userId} не найден");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
