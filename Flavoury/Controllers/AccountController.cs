using System.Security.Claims;
using AutoMapper;
using Entities.Models;
using Flavoury.Filters;
using Flavoury.Filters.CanManage;
using Flavoury.Filters.Exist;
using Flavoury.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flavoury.Controllers
{
    [Route("[action]")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _mapper.Map<User>(registerViewModel);
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded) return BadRequest();
            await _userManager.AddToRoleAsync(user, "user");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            var result = await _signInManager.PasswordSignInAsync(
                userName: loginViewModel.Login,
                password: loginViewModel.Password,
                isPersistent: loginViewModel.RememberMe,
                lockoutOnFailure: false);
            return result.Succeeded ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var editUserViewModel = _mapper.Map<EditUserViewModel>(user);
            return Ok(editUserViewModel);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditUserViewModel editUserViewModel)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.Values);
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _mapper.Map(editUserViewModel, user);
            var result = await _userManager.UpdateAsync(user!);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user!);
                return Ok(editUserViewModel);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> About()
        {
            var userViewModel = new UserViewModel();
            userViewModel.UserName = User.FindFirstValue(ClaimTypes.Name)!;
            userViewModel.Role = await _userManager.GetRolesAsync(
                (await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email)!))!);

            return Ok(userViewModel);
        }
    }
}
