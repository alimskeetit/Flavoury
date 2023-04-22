using Entities;
using Entities.Models;
using Flavoury.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Flavoury.Filters.Extensions.Functions;

namespace Flavoury.Filters.Exist
{
    public class ExistAttribute<T>: ActionFilterAttribute where T : class
    {
        private readonly string _pathToId;

        public ExistAttribute(string pathToId = "id")
        {
            _pathToId = pathToId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dbContext = (AppDbContext)context.HttpContext.RequestServices.GetRequiredService(typeof(AppDbContext));
            var id = IdFrom(_pathToId, context.ActionArguments);
            var entity = dbContext.Set<T>().Find(int.TryParse(id, out var intIdResult)? intIdResult: id);
            if (entity == null)
                context.Result = new NotFoundObjectResult($"{typeof(T).Name} с id {id} не найден");
        }
    }
}
