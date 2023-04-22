using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;

namespace Entities
{
    public  class DataSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {

            var roles = new[] { "admin", "user" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role)) 
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var admin1 = await _userManager.FindByEmailAsync("alimskiy@mail.ru");
            if (admin1 != null)
                await _userManager.AddToRoleAsync(admin1, "admin");
        }
    }
}
