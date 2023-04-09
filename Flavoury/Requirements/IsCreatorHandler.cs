using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Flavoury.Requirements
{
    public class IsCreatorHandler : AuthorizationHandler<IsCreatorRequirement, Recipe>
    {
        private readonly UserManager<User> _userManager;

        public IsCreatorHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsCreatorRequirement requirement,
            Recipe resource)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
                return;
            if (resource.UserId == user.Id)
                context.Succeed(requirement);
        }
    }

    public class IsCreatorRequirement : IAuthorizationRequirement
    {
    }
}
