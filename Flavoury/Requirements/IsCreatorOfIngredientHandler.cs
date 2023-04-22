using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Flavoury.Requirements
{
    public class IsCreatorOfIngredientHandler: AuthorizationHandler<IsCreatorOfIngredient, Ingredient>, IAuthorizationRequirement
    {
        private readonly UserManager<User> _userManager;

        public IsCreatorOfIngredientHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsCreatorOfIngredient requirement,
            Ingredient resource)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (resource.Recipe!.UserId == user!.Id || await _userManager.IsInRoleAsync(user, "admin"))
                context.Succeed(requirement);
        }
    }

    public class IsCreatorOfIngredient : IAuthorizationRequirement
    {
    }
}
