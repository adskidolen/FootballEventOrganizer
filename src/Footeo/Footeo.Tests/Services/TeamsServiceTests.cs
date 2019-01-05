namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Teams.Output;
    using Footeo.Web.ViewModels.Trophies.Output;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using NUnit.Framework;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class TeamsServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateTeamShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "CreateTeam_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");

            Assert.NotNull(team);
        }

        [Test]
        public void TeamExistsByIdShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "TeamExistsByIdTrue_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");

            var teamExists = teamsService.TeamExistsById(team.Id);

            Assert.True(teamExists);
        }

        [Test]
        public void TeamExistsByIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "TeamExistsByIdFalse_Teams_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");

            var invalidTeamId = 8374;
            var teamExists = teamsService.TeamExistsById(invalidTeamId);

            Assert.False(teamExists);
        }

        [Test]
        public void GetTeamByIdShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTeamById_Teams_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Plovdiv");

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team1", "TTT", user.UserName);
            var teamId = dbContext.Teams.FirstOrDefault(n => n.Name == "Team1").Id;

            var team = teamsService.GetTeamById<TeamViewModel>(teamId);

            Assert.NotNull(team);
        }

        [Test]
        public void AllTeamsShouldReturnCorrectTeamsCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "AllTeams_Teams_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var user2 = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoPlayer2@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeoPlayer2",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo Player"
                }
            };

            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user2, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team1", "TTT", user.UserName);
            teamsService.CreateTeam("Team2", "TTT", user2.UserName);

            var teams = teamsService.AllTeams<Team>().ToList();

            var expectedTeamsCount = 2;

            Assert.AreEqual(expectedTeamsCount, teams.Count);
        }

        [Test]
        public void PlayersCountInTeamShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "PlayersCount_Teams_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");

            var player = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "player@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "player",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo Player"
                }
            };

            team.Players.Add(player.Player);
            dbContext.SaveChanges();

            var expectedPlayersCount = 2;
            var playersCount = teamsService.PlayersCount(team.Id);

            Assert.AreEqual(expectedPlayersCount, playersCount);
        }

        [Test]
        public void IsTeamInLeagueShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "IsTeamInLeagueFalse_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Stara Zagora");

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");

            var isTeamInLeague = teamsService.IsTeamInLeague(team.Id);

            Assert.False(isTeamInLeague);
        }

        [Test]
        public void IsTeamInLeagueShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "IsTeamInLeagueTrue_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Stara Zagora");

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Stara Zagora");
            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            var teamLeague = new TeamLeague
            {
                Team = team,
                League = league
            };

            dbContext.TeamsLeagues.Add(teamLeague);
            dbContext.SaveChanges();

            var isTeamInLeague = teamsService.IsTeamInLeague(team.Id);

            Assert.True(isTeamInLeague);
        }

        [Test]
        public void GetUsersTeamShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "GetUsersTeamNull_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            var team = teamsService.GetUsersTeam(user.UserName);

            Assert.Null(team);
        }

        [Test]
        public void GetUsersTeamShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "GetUsersTeam_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);

            var team = teamsService.GetUsersTeam(user.UserName);

            Assert.NotNull(team);
        }

        [Test]
        public void AllTrophiesByTeamShouldReturnNone()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "AllTrophiesByTeam_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team3", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team3");

            var expectedTrophiesCountByTeam = 0;
            var trophiesByTeam = teamsService.AllTrophiesByTeamId<TrophyViewModel>(team.Id).ToList();
            var trophiesCount = trophiesByTeam.Count;

            Assert.AreEqual(expectedTrophiesCountByTeam, trophiesCount);
        }

        [Test]
        public void AllTrophiesByTeamShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "AllTrophiesByTeamCount_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team3", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team3");

            for (int i = 1; i <= 3; i++)
            {
                var trophy = new Trophy
                {
                    Name = $"Trophy{i}",
                    Team = team
                };

                dbContext.Trophies.Add(trophy);
                dbContext.SaveChanges();
            }

            var expectedTrophiesCountByTeam = 3;
            var trophiesByTeam = teamsService.AllTrophiesByTeamId<TrophyViewModel>(team.Id).ToList();
            var trophiesCount = trophiesByTeam.Count;

            Assert.AreEqual(expectedTrophiesCountByTeam, trophiesCount);
        }

        [Test]
        public void AllHomeMatchesByTeamIdShouldReturnNone()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "AllHomeMatchesNone_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Ruse");

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team4", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team4");

            var allHomeMatches = teamsService.AllHomeMatchesByTeamId(team.Id).ToList();
            var allHomeMatchesCount = allHomeMatches.Count;
            var expectedHomeMatchesCount = 0;

            Assert.AreEqual(expectedHomeMatchesCount, allHomeMatchesCount);
        }

        [Test]
        public void AllHomeMatchesByTeamIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "AllHomeMatches_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Ruse");

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team4", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team4");

            for (int i = 1; i <= 4; i++)
            {
                var match = new Models.Match
                {
                    AwayTeam = new Team
                    {
                        Name = $"AwayTeam{i}",
                        Initials = "AT",
                        Town = town
                    },
                    AwayTeamGoals = 0,
                    HomeTeamGoals = i,
                    HomeTeam = team,
                    Result = $"Team4 {i} : 0 AwayTeam"
                };

                dbContext.Matches.Add(match);
                dbContext.SaveChanges();
            }


            var allHomeMatches = teamsService.AllHomeMatchesByTeamId(team.Id).ToList();
            var allHomeMatchesCount = allHomeMatches.Count;
            var expectedHomeMatchesCount = 4;

            Assert.AreEqual(expectedHomeMatchesCount, allHomeMatchesCount);
        }

        [Test]
        public void AllAwayMatchesByTeamIdShouldReturnNone()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "AllAwayMatchesNone_Teams_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Ruse");

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team4", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team4");

            var allAwayMatches = teamsService.AllAwayMatchesByTeamId(team.Id).ToList();
            var allAwayMatchesCount = allAwayMatches.Count;
            var expectedAwayMatchesCount = 0;

            Assert.AreEqual(expectedAwayMatchesCount, allAwayMatchesCount);
        }

        [Test]
        public void AllAwayMatchesByTeamIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "AllAwayMatches_Teams_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var town = townsService.CreateTown("Ruse");

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

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team4", "TTT", user.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team4");

            for (int i = 1; i <= 4; i++)
            {
                var match = new Models.Match
                {
                    HomeTeam = new Team
                    {
                        Name = $"HomeTeam{i}",
                        Initials = "HT",
                        Town = town
                    },
                    AwayTeamGoals = i,
                    HomeTeamGoals = 0,
                    AwayTeam = team,
                    Result = $"HomeTeam{i} 0 : {i} Team4"
                };

                dbContext.Matches.Add(match);
                dbContext.SaveChanges();
            }


            var allAwayMatches = teamsService.AllAwayMatchesByTeamId(team.Id).ToList();
            var allAwayMatchesCount = allAwayMatches.Count;
            var expectedAwayMatchesCount = 4;

            Assert.AreEqual(expectedAwayMatchesCount, allAwayMatchesCount);
        }
    }
}