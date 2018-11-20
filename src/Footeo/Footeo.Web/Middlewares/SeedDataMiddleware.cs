namespace Footeo.Web.Middlewares
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Web.Utilities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class SeedDataMiddleware
    {
        private readonly RequestDelegate next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, FooteoDbContext dbContext,
           UserManager<FooteoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!dbContext.Roles.Any())
            {
                await this.SeedRoles(userManager, roleManager);
            }

            await this.next(context);
        }

        private async Task SeedRoles(UserManager<FooteoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRoleExists = await roleManager.RoleExistsAsync(Constants.AdminRoleName);
            var playerRoleExist = await roleManager.RoleExistsAsync(Constants.PlayerRoleName);
            var refereeRoleExists = await roleManager.RoleExistsAsync(Constants.RefereeRoleName);

            if (!playerRoleExist || !refereeRoleExists || !adminRoleExists)
            {
                var adminRoleResult = await roleManager.CreateAsync(new IdentityRole(Constants.AdminRoleName));
                var playerRoleResult = await roleManager.CreateAsync(new IdentityRole(Constants.PlayerRoleName));
                var refereeRoleResult = await roleManager.CreateAsync(new IdentityRole(Constants.RefereeRoleName));
            }

        }
    }
}