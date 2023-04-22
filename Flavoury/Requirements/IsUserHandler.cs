using Entities.Models;
using Flavoury.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Flavoury.Requirements
{
    public class IsUserHandler: AuthorizationHandler<IsUser, string>
    {
        private readonly UserManager<User> _userManager;

        public IsUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsUser requirement,
            string id)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (id == user!.Id)
                context.Succeed(requirement);
        }
    }

    public class IsUser : IAuthorizationRequirement
    {

    }

}

