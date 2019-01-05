namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Matches.Output;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using NUnit.Framework;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class MatchesServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateMatchShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "CreateMatch_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);

            Assert.NotNull(match);
        }

        [Test]
        public void MatchExistsByIdShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "MatchExistsByIdTrue_Matches_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);

            var matchExists = matchesService.MatchExistsById(match.Id);

            Assert.True(matchExists);
        }

        [Test]
        public void MatchExistsByIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "MatchExistsByIdFalse_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);

            var invalidMatchId = 38;
            var matchExists = matchesService.MatchExistsById(invalidMatchId);

            Assert.False(matchExists);
        }

        [Test]
        public void GetFixturesIdByMatchShouldNotReturnZero()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "GetFixturesIdByMatch_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);

            var fixtureId = matchesService.GetFixturesIdByMatch(match.Id);

            Assert.NotZero(fixtureId);
        }

        [Test]
        public void GetMatchByIdShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "GetMatchById_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var matchId = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id).Id;

            var match = matchesService.GetMatchById<Models.Match>(matchId);

            Assert.NotNull(match);
        }

        [Test]
        public void GetMatchByIdShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "GetMatchByIdNull_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var matchId = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id).Id;

            var invalidMatchId = 483;
            var match = matchesService.GetMatchById<Models.Match>(invalidMatchId);

            Assert.Null(match);
        }

        [Test]
        public void MatchesByFixtureShouldReturnCorrectMatchesCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "MatchesByFixture_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);
            matchesService.CreateMatch(userAT.Player.Team.Id, userHT.Player.Team.Id, fieldId, fixture.Id);

            var matches = matchesService.MatchesByFixture<MatchDetailsViewModel>(fixture.Id).ToList();
            var matchesCount = matches.Count;
            var expectedMatchesCount = 2;

            Assert.AreEqual(expectedMatchesCount, matchesCount);
        }

        [Test]
        public void MatchHasCurrentRefereeShuoldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "MatchHasCurrentRefereeFalse_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var referee = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"referee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"referee",
                Town = town,
                PasswordHash = "123123",
                Referee = new Referee
                {
                    FullName = "Footeo Referee"
                }
            };

            dbContext.Users.Add(referee);
            dbContext.SaveChanges();

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);

            var matchHasCurrentRefereetch = matchesService.MatchHasCurrentReferee(match.Id, referee.UserName);

            Assert.False(matchHasCurrentRefereetch);
        }

        [Test]
        public void MatchHasCurrentRefereeShuoldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "MatchHasCurrentRefereeTrue_Matches_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var referee = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"referee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"referee",
                Town = town,
                PasswordHash = "123123",
                Referee = new Referee
                {
                    FullName = "Footeo Referee"
                }
            };

            dbContext.Users.Add(referee);
            dbContext.SaveChanges();

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);
            match.Referee = referee.Referee;
            dbContext.SaveChanges();

            var matchHasCurrentRefereetch = matchesService.MatchHasCurrentReferee(match.Id, referee.UserName);

            Assert.True(matchHasCurrentRefereetch);
        }

        [Test]
        public void MatchHasRefereeShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "MatchHasRefereeFalse_Matches_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);

            var matchHasReferee = matchesService.MatchHasReferee(match.Id);

            Assert.False(matchHasReferee);
        }

        [Test]
        public void MatchHasRefereeShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
             .UseInMemoryDatabase(databaseName: "MatchHasRefereeTrue_Matches_DB")
             .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var referee = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"referee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"referee",
                Town = town,
                PasswordHash = "123123",
                Referee = new Referee
                {
                    FullName = "Footeo Referee"
                }
            };

            dbContext.Users.Add(referee);
            dbContext.SaveChanges();

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);
            match.Referee = referee.Referee;
            dbContext.SaveChanges();

            var matchHasReferee = matchesService.MatchHasReferee(match.Id);

            Assert.True(matchHasReferee);
        }

        [Test]
        public void MatchHasResultShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "MatchHasResultFalse_Matches_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);

            var matchHasResult = matchesService.MatchHasResult(match.Id);

            Assert.False(matchHasResult);
        }

        [Test]
        public void MatchHasResultShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "MatchHasResultTrue_Matches_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            var town = townsService.CreateTown("Pleven");

            var userHT = new FooteoUser
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

            dbContext.Users.Add(userHT);
            dbContext.SaveChanges();

            var userAT = new FooteoUser
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

            dbContext.Users.Add(userAT);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(userHT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userHT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(u => u.RemoveFromRoleAsync(userAT, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(userAT, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var fixturesService = new FixturesService(dbContext, leaguesService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var field = new Field
            {
                Name = "Field",
                Address = "Address",
                IsIndoors = false,
                Town = town
            };

            dbContext.Fields.Add(field);
            dbContext.SaveChanges();

            var fieldId = dbContext.Fields.FirstOrDefault(f => f.Name == "Field").Id;

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddMonths(3), "Pleven");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(f => f.Name == "Matchday 1");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, fieldId, fixture.Id);

            var match = dbContext.Matches.FirstOrDefault(m => m.FixtureId == fixture.Id);
            match.Result = "Home Team 2 : 2 Away Team";
            dbContext.SaveChanges();

            var matchHasResult = matchesService.MatchHasResult(match.Id);

            Assert.True(matchHasResult);
        }
    }
}