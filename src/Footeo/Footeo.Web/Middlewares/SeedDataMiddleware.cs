namespace Footeo.Web.Middlewares
{
    using Footeo.Common;
    using Footeo.Data;
    using Footeo.Models;

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
            if (!dbContext.Roles.Any())
            {
                await this.SeedRoles(userManager, roleManager);
            }

            await this.next(context);
        }

        private async Task SeedRoles(UserManager<FooteoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRoleExists = await roleManager.RoleExistsAsync(GlobalConstants.AdminRoleName);
            var playerRoleExist = await roleManager.RoleExistsAsync(GlobalConstants.PlayerRoleName);
            var refereeRoleExists = await roleManager.RoleExistsAsync(GlobalConstants.RefereeRoleName);
            var playerInTeamRoleExists = await roleManager.RoleExistsAsync(GlobalConstants.PlayerInTeamRoleName);
            var captainRoleExists = await roleManager.RoleExistsAsync(GlobalConstants.CaptainRoleName);

            if (!playerRoleExist || !refereeRoleExists || !adminRoleExists || !playerInTeamRoleExists || !captainRoleExists)
            {
                var adminRoleResult = await roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdminRoleName));
                var playerRoleResult = await roleManager.CreateAsync(new IdentityRole(GlobalConstants.PlayerRoleName));
                var refereeRoleResult = await roleManager.CreateAsync(new IdentityRole(GlobalConstants.RefereeRoleName));
                var playerInTeamRoleResult = await roleManager.CreateAsync(new IdentityRole(GlobalConstants.PlayerInTeamRoleName));
                var captainRoleResult = await roleManager.CreateAsync(new IdentityRole(GlobalConstants.CaptainRoleName));
            }
        }
    }
}