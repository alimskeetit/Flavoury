using Entities;
using Flavoury.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Flavoury.Filters.Extensions.Functions;
namespace Flavoury.Filters.CanManage
{
    public class CanManageAttribute<T>: ActionFilterAttribute where T : class
    {
        private readonly string _pathToId;

        public CanManageAttribute(string pathToId = "id")
        {
            _pathToId = pathToId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dbContext = (AppDbContext)context.HttpContext.RequestServices.GetRequiredService(typeof(AppDbContext));
            var authService =
                (IAuthorizationService)context.HttpContext.RequestServices.GetRequiredService(
                    typeof(IAuthorizationService));
            var id = IdFrom(_pathToId, context.ActionArguments);
            var resource = dbContext.Set<T>().Find(int.TryParse(id, out var intIdResult) ? intIdResult : id);
            var authResult = authService
                .AuthorizeAsync(
                    user: context.HttpContext.User,
                    resource: resource,
                    policyName: $"CanManage{typeof(T).Name}").GetAwaiter().GetResult();

            if (!authResult.Succeeded)
                context.Result = new ForbidResult();
        }
    }
}
