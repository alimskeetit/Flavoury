using Flavoury.Services;
using Flavoury.ViewModels.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flavoury.Filters
{
    public class CanManageRecipeAttribute: Attribute, IActionFilter
    {
        private readonly string _id;

        public CanManageRecipeAttribute(string id = "id")
        {
            _id = id;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var recipeService = (RecipeService) context.HttpContext.RequestServices.GetService(typeof(RecipeService))!;
            var authService = (IAuthorizationService) context.HttpContext.RequestServices.GetRequiredService(typeof(IAuthorizationService));
            var user = context.HttpContext.User;

            var recipeId = 0;
            if (_id.Contains('.'))
            {
                var properties = _id.Split('.');
                var obj = context.ActionArguments[properties[0]];
                if (obj!.GetType().GetProperty(properties[1]) != null)
                    recipeId = (int)obj.GetType().GetProperty(properties[1])!.GetValue(obj)!;
            }
            else
                recipeId = (int)context.ActionArguments[_id]!;

            var recipe = recipeService.GetAsync(recipeId).GetAwaiter().GetResult();
            var authResult = authService.AuthorizeAsync(
                user: user,
                resource: recipe,
                policyName: "CanManageRecipe").GetAwaiter().GetResult();

            if (!authResult.Succeeded)
                context.Result = new ForbidResult();
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
