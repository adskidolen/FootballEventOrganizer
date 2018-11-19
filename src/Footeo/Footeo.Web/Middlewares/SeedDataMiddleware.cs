namespace Footeo.Web.Middlewares
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Web.Utilities;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

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
            if (!dbContext.Roles.Any() || dbContext.Roles.Count() == 1)
            {
                await this.SeedRoles(userManager, roleManager);
            }

            await this.next(context);
        }

        private async Task SeedRoles(UserManager<FooteoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminResult = await roleManager.CreateAsync(new IdentityRole(Constants.AdminRoleName));

            if (adminResult.Succeeded && userManager.Users.Count() == 1)
            {
                var firstUser = userManager.Users.FirstOrDefault();

                await userManager.AddToRoleAsync(firstUser, Constants.AdminRoleName);
            }

            var playerRoleExist = await roleManager.RoleExistsAsync(Constants.PlayerRoleName);
            var refereeRoleExists = await roleManager.RoleExistsAsync(Constants.RefereeRoleName);
            if (!playerRoleExist || !refereeRoleExists)
            {
                var playerRoleResult = await roleManager.CreateAsync(new IdentityRole(Constants.PlayerRoleName));
                var refereeRoleResult = await roleManager.CreateAsync(new IdentityRole(Constants.RefereeRoleName));
            }
        }
    }
}