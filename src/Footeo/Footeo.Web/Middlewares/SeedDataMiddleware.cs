namespace Footeo.Web.Middlewares
{
    using Footeo.Common;
    using Footeo.Data;
    using Footeo.Models;

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

            if (!dbContext.Referees.Any())
            {
                await this.SeedReferees(dbContext, userManager);
            }

            if (!dbContext.Fields.Any())
            {
                this.SeedFields(dbContext);
            }

            await this.next(context);
        }

        private void SeedFields(FooteoDbContext dbContext)
        {
            for (int i = 1; i <= 20; i++)
            {
                var field = new Field
                {
                    Name = $"Footeo Field{i}",
                    Address = $"Addres{i}",
                    Town = dbContext.Towns.FirstOrDefault(t => t.Name == "Sofia"),
                    IsIndoors = i % 2 == 0 ? true : false
                };

                dbContext.Fields.Add(field);
                dbContext.SaveChanges();
            }
        }

        private async Task SeedReferees(FooteoDbContext dbContext, UserManager<FooteoUser> userManager)
        {
            for (int i = 1; i <= 20; i++)
            {
                var user = new FooteoUser
                {
                    Age = new Random().Next(20, 30),
                    Email = $"footeoReferee{i}@mail.bg",
                    FirstName = "Footeo",
                    LastName = "Referee",
                    UserName = $"footeoReferee{i}",
                    Town = dbContext.Towns.FirstOrDefault(t => t.Name == "Sofia"),
                    PasswordHash = "123123",
                    Referee = new Referee
                    {
                        FullName = $"Footeo Referee{i}"
                    }
                };

                var result = await userManager.CreateAsync(user, "123123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.RefereeRoleName);
                }
            }
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