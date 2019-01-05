namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services;
    using Footeo.Tests.Base;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using NUnit.Framework;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class TrophiesServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateTrophyShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateTrophy_Trophies_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Sofia");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeoPlayer",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo Player"
                }
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var leaguesService = new LeaguesService(dbContext, townsService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);
            var trophiesService = new TrophiesService(dbContext, teamLeaguesService);

            teamsService.CreateTeam("TeamWinner", "TWR", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(t => t.Name == "TeamWinner");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");
            leaguesService.SetLeagueStatusToCompleted(league.Id);

            var teamLeague = new TeamLeague
            {
                Team = team,
                League = league
            };

            dbContext.TeamsLeagues.Add(teamLeague);
            dbContext.SaveChanges();

            trophiesService.CreateTrophy(league.Id);

            var hasTeamTrophy = team.Trophies.Any();

            Assert.True(hasTeamTrophy);
        }
    }
}