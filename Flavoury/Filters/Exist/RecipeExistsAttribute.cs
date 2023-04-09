using Flavoury.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flavoury.Filters.Exist
{
    public class RecipeExistsAttribute : Attribute, IActionFilter
    {
        private readonly string _id;

        public RecipeExistsAttribute(string id = "id")
        {
            _id = id;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var service = (RecipeService)context.HttpContext
                .RequestServices.GetService(typeof(RecipeService))!;

            int recipeId = 0;

            if (_id.Contains('.'))
            {
                var properties = _id.Split('.');
                var obj = context.ActionArguments[properties[0]];
                if (obj!.GetType().GetProperty(properties[1]) != null)
                    recipeId = (int) obj.GetType().GetProperty(properties[1])!.GetValue(obj)!;
            }
            else
                recipeId = (int) context.ActionArguments[_id]!;

            if (!service.DoesRecipeExistAsync(recipeId).GetAwaiter().GetResult())
                context.Result = new NotFoundObjectResult($"Рецепт с id {recipeId} не найден");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
