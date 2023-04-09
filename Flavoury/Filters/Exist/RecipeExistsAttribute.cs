using Flavoury.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flavoury.Filters.Exist
{
    public class RecipeExistsAttribute : Attribute, IActionFilter
    {
        readonly string _id;

        public RecipeExistsAttribute(string id = "id")
        {
            _id = id;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var service = (RecipeService)context.HttpContext
                .RequestServices.GetService(typeof(RecipeService))!;
            var recipeId = (int)context.ActionArguments[_id]!;

            if (!service.DoesRecipeExistAsync(recipeId).GetAwaiter().GetResult())
                context.Result = new NotFoundObjectResult($"Рецепт с id {recipeId} не найден");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
