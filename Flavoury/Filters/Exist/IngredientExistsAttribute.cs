using Flavoury.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flavoury.Filters.Exist
{
    public class IngredientExistsAttribute : ActionFilterAttribute
    {
        private readonly string _id;

        public IngredientExistsAttribute(string id = "id")
        {
            _id = id;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var service = context.HttpContext
                .RequestServices.GetService(typeof(IngredientService))! as IngredientService;
            var ingredientId = (int)context.ActionArguments[_id]!;

            if (service.GetAsync(ingredientId).GetAwaiter().GetResult() == null)
                context.Result = new NotFoundObjectResult($"Ингредиент с id {ingredientId} не найден");
        }
    }
}
