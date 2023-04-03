using Entities.Models;
using Flavoury.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace Flavoury.Controllers
{
    public class AccountController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            User user = new()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded) return BadRequest();
            
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            
            var result = await _signInManager.PasswordSignInAsync(
                userName: loginViewModel.Email,
                password: loginViewModel.Password,
                isPersistent: loginViewModel.RememberMe,
                lockoutOnFailure: false);
            
            return result.Succeeded ? Ok() : BadRequest();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditUserViewModel editUserViewModel)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.Values); 
            
            var user = await _userManager.FindByIdAsync(editUserViewModel.Id);

            if (user == null) 
                return NotFound();

            user.Email = editUserViewModel.Email;
            user.UserName = editUserViewModel.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(editUserViewModel);
            
            return BadRequest();

        }
    }
}
