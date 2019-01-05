namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Referees.Output;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using NUnit.Framework;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class RefereesServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateRefereeShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "CreateReferee_Referees_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Haskovo");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoReferee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"footeoReferee",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var refereesService = new RefereesService(dbContext, null, null);

            var referee = new Referee
            {
                FullName = $"Footeo Referee"
            };

            refereesService.CreateReferee(user, referee);

            Assert.NotNull(user.Referee);
        }

        [Test]
        public void CreateRefereeShouldReturnCorrectRefereeCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "RefereesCount_Referees_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);

            for (int i = 1; i <= 10; i++)
            {
                var user = new FooteoUser
                {
                    Age = new Random().Next(20, 30),
                    Email = $"footeoReferee{i}@mail.bg",
                    FirstName = "Footeo",
                    LastName = "Referee",
                    UserName = $"footeoReferee{i}",
                    TownId = new Random().Next(1, 20),
                    PasswordHash = "123123",
                    Referee = new Referee
                    {
                        FullName = $"Footeo Referee{i}"
                    }
                };

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }

            var refereesService = new RefereesService(dbContext, null, null);

            var referees = refereesService.Referees<RefereeViewModel>().ToList();

            var expectedRefereesCount = 10;

            Assert.AreEqual(expectedRefereesCount, referees.Count);
        }

        [Test]
        public void RefereeAttendToMatchShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "AttendToMatch_Referees_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);

            var town = townsService.CreateTown("Burgas");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoReferee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"footeoReferee",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
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

            var fieldsService = new FieldsService(dbContext, townsService);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);
            var fixturesService = new FixturesService(dbContext, leaguesService);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);
            var refereesService = new RefereesService(dbContext, matchesService, teamLeaguesService);

            var referee = new Referee
            {
                FullName = $"Footeo Referee"
            };

            refereesService.CreateReferee(user, referee);

            townsService.CreateTown("Sofia");
            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(3), "Sofia");

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            teamLeaguesService.JoinLeague(userHT.UserName, league.Id);
            teamLeaguesService.JoinLeague(userAT.UserName, league.Id);

            fixturesService.CreateFixture("Matchday", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday");

            fieldsService.CreateField("Field", "Address", true, "Sofia");
            var field = dbContext.Fields.FirstOrDefault(n => n.Name == "Field");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, field.Id, fixture.Id);
            var match = dbContext.Matches.FirstOrDefault();

            refereesService.AttendAMatch(user.UserName, match.Id);

            Assert.NotNull(match.Referee);
        }

        [Test]
        public void AddResultToMatchTeamDrawShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "AddResultToMatchTeamDraw_Referees_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);

            var town = townsService.CreateTown("Burgas");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoReferee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"footeoReferee",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
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

            var fieldsService = new FieldsService(dbContext, townsService);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);
            var fixturesService = new FixturesService(dbContext, leaguesService);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);
            var refereesService = new RefereesService(dbContext, matchesService, teamLeaguesService);

            var referee = new Referee
            {
                FullName = $"Footeo Referee"
            };

            refereesService.CreateReferee(user, referee);

            townsService.CreateTown("Sofia");
            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(3), "Sofia");

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            teamLeaguesService.JoinLeague(userHT.UserName, league.Id);
            teamLeaguesService.JoinLeague(userAT.UserName, league.Id);

            fixturesService.CreateFixture("Matchday", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday");

            fieldsService.CreateField("Field", "Address", true, "Sofia");
            var field = dbContext.Fields.FirstOrDefault(n => n.Name == "Field");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, field.Id, fixture.Id);
            var match = dbContext.Matches.FirstOrDefault();

            refereesService.AttendAMatch(user.UserName, match.Id);

            refereesService.AddResultToMatch(match.Id, 2, 2);

            var expectedResult = "Home Team 2 : 2 Away Team";

            Assert.AreEqual(expectedResult, match.Result);
        }

        [Test]
        public void AddResultToMatchTeamWinShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "AddResultToMatchTeamWin_Referees_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);

            var town = townsService.CreateTown("Burgas");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoReferee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"footeoReferee",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
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

            var fieldsService = new FieldsService(dbContext, townsService);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);
            var fixturesService = new FixturesService(dbContext, leaguesService);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);
            var refereesService = new RefereesService(dbContext, matchesService, teamLeaguesService);

            var referee = new Referee
            {
                FullName = $"Footeo Referee"
            };

            refereesService.CreateReferee(user, referee);

            townsService.CreateTown("Sofia");
            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(3), "Sofia");

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            teamLeaguesService.JoinLeague(userHT.UserName, league.Id);
            teamLeaguesService.JoinLeague(userAT.UserName, league.Id);

            fixturesService.CreateFixture("Matchday", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday");

            fieldsService.CreateField("Field", "Address", true, "Sofia");
            var field = dbContext.Fields.FirstOrDefault(n => n.Name == "Field");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, field.Id, fixture.Id);
            var match = dbContext.Matches.FirstOrDefault();

            refereesService.AttendAMatch(user.UserName, match.Id);

            refereesService.AddResultToMatch(match.Id, 3, 1);

            var expectedResult = "Home Team 3 : 1 Away Team";

            Assert.AreEqual(expectedResult, match.Result);
        }

        [Test]
        public void AddResultToMatchTeamLoseShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "AddWinResultToMatch_Referees_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);

            var town = townsService.CreateTown("Burgas");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoReferee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"footeoReferee",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
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

            var fieldsService = new FieldsService(dbContext, townsService);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);
            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);
            var fixturesService = new FixturesService(dbContext, leaguesService);
            var matchesService = new MatchesService(dbContext, fixturesService, teamsService);
            var refereesService = new RefereesService(dbContext, matchesService, teamLeaguesService);

            var referee = new Referee
            {
                FullName = $"Footeo Referee"
            };

            refereesService.CreateReferee(user, referee);

            townsService.CreateTown("Sofia");
            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(3), "Sofia");

            teamsService.CreateTeam("Home Team", "HT", userHT.UserName);
            teamsService.CreateTeam("Away Team", "AT", userAT.UserName);

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            teamLeaguesService.JoinLeague(userHT.UserName, league.Id);
            teamLeaguesService.JoinLeague(userAT.UserName, league.Id);

            fixturesService.CreateFixture("Matchday", DateTime.UtcNow.AddDays(7), league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday");

            fieldsService.CreateField("Field", "Address", true, "Sofia");
            var field = dbContext.Fields.FirstOrDefault(n => n.Name == "Field");

            matchesService.CreateMatch(userHT.Player.Team.Id, userAT.Player.Team.Id, field.Id, fixture.Id);
            var match = dbContext.Matches.FirstOrDefault();

            refereesService.AttendAMatch(user.UserName, match.Id);

            refereesService.AddResultToMatch(match.Id, 1, 2);

            var expectedResult = "Home Team 1 : 2 Away Team";

            Assert.AreEqual(expectedResult, match.Result);
        }
    }
}