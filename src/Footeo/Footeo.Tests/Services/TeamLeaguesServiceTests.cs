namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Models.Enums;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.TeamLeagues.Output;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using NUnit.Framework;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class TeamLeaguesServiceTests : BaseServiceTests
    {
        [Test]
        public void GetTeamLeagueShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetTeamLeague_TeamLeagues_DB")
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

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeague = new TeamLeague
            {
                Team = team,
                League = league
            };

            dbContext.TeamsLeagues.Add(teamLeague);
            dbContext.SaveChanges();

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            var teamLeagueModel = teamLeaguesService.GetTeamLeague(team.Id, league.Id);

            Assert.NotNull(teamLeagueModel);
        }

        [Test]
        public void GetTeamLeagueWinnerShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetTeamLeagueWinner_TeamLeagues_DB")
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
                Email = $"footeo@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeo",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo User"
                }
            };

            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user2, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            teamsService.CreateTeam("Team2", "TTT", user2.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");
            var team2 = dbContext.Teams.FirstOrDefault(n => n.Name == "Team2");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);
            teamLeaguesService.JoinLeague(user2.UserName, league.Id);

            var teamLeague1 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team);
            var teamLeague2 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team2);

            teamLeague1.Points = 100;
            teamLeague2.Points = 50;
            dbContext.SaveChanges();

            league.Status = LeagueStatus.Completed;
            dbContext.SaveChanges();

            var winner = teamLeaguesService.GetTeamLeagueWinner(league.Id);

            Assert.AreEqual(winner.Team.Name, teamLeague1.Team.Name);
        }

        [Test]
        public void IsTeamInLeagueShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "IsTeamInLeagueTrue_TeamLeagues_DB")
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

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);

            var isTeamInLeague = teamLeaguesService.IsTeamInLeague(league.Id, user.UserName);

            Assert.True(isTeamInLeague);
        }

        [Test]
        public void IsTeamInLeagueShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "IsTeamInLeagueFalse_TeamLeagues_DB")
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

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            var isTeamInLeague = teamLeaguesService.IsTeamInLeague(league.Id, user.UserName);

            Assert.False(isTeamInLeague);
        }

        [Test]
        public void JoinLeagueShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "JoinLeague_TeamLeagues_DB")
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

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);

            var hasTeamJoinedLeague = league.Teams.Any(t => t.TeamId == team.Id);

            Assert.True(hasTeamJoinedLeague);
        }

        [Test]
        public void LeagueTableShouldReturnCorrectTeamsCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "LeagueTable_TeamLeagues_DB")
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
                Email = $"footeo@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeo",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo User"
                }
            };

            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user2, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            teamsService.CreateTeam("Team2", "TTT", user2.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");
            var team2 = dbContext.Teams.FirstOrDefault(n => n.Name == "Team2");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);
            teamLeaguesService.JoinLeague(user2.UserName, league.Id);

            var teams = teamLeaguesService.LeagueTable<TeamLeagueViewModel>(league.Id).ToList();

            var teamsCount = teams.Count;
            var expectedTeamsCount = 2;

            Assert.AreEqual(expectedTeamsCount, teamsCount);
        }

        [Test]
        public void TeamsCountShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "TeamsCount_TeamLeagues_DB")
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
                Email = $"footeo@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeo",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo User"
                }
            };

            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user2, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            teamsService.CreateTeam("Team2", "TTT", user2.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");
            var team2 = dbContext.Teams.FirstOrDefault(n => n.Name == "Team2");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);
            teamLeaguesService.JoinLeague(user2.UserName, league.Id);

            var teamsCount = teamLeaguesService.TeamsCount(league.Id);
            var expectedTeamsCount = 2;

            Assert.AreEqual(expectedTeamsCount, teamsCount);
        }


        [Test]
        public void AllPlayedMatchesShouldReturnCorrectSum()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "AllPlayedMatches_TeamLeagues_DB")
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
                Email = $"footeo@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeo",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo User"
                }
            };

            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user2, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            teamsService.CreateTeam("Team2", "TTT", user2.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");
            var team2 = dbContext.Teams.FirstOrDefault(n => n.Name == "Team2");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);
            teamLeaguesService.JoinLeague(user2.UserName, league.Id);

            var teamLeague1 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team);
            var teamLeague2 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team2);

            teamLeague1.PlayedMatches = 5;
            teamLeague2.PlayedMatches = 5;
            dbContext.SaveChanges();

            var allPlayedMatches = teamLeaguesService.AllPlayedMatchesCount(league.Id);
            var expectedPlayedMatches = 10;

            Assert.AreEqual(expectedPlayedMatches, allPlayedMatches);
        }

        [Test]
        public void HasTeamAlreadyCurrentTrophyShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "HasTeamAlreadyCurrentTrophyTrue_TeamLeagues_DB")
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
                Email = $"footeo@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeo",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo User"
                }
            };

            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user2, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            teamsService.CreateTeam("Team2", "TTT", user2.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");
            var team2 = dbContext.Teams.FirstOrDefault(n => n.Name == "Team2");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);
            teamLeaguesService.JoinLeague(user2.UserName, league.Id);

            var teamLeague1 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team);
            var teamLeague2 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team2);

            teamLeague1.Points = 100;
            teamLeague2.Points = 50;
            dbContext.SaveChanges();

            league.Status = LeagueStatus.Completed;
            dbContext.SaveChanges();

            var winner = teamLeaguesService.GetTeamLeagueWinner(league.Id);

            var trophy = new Trophy
            {
                Name = "League",
                Team = winner.Team
            };

            dbContext.Trophies.Add(trophy);
            dbContext.SaveChanges();

            var hasTeamAlreadyCurrentTrophy = teamLeaguesService.HasTeamAlreadyCurrentTrophy(league.Id);

            Assert.True(hasTeamAlreadyCurrentTrophy);
        }

        [Test]
        public void HasTeamAlreadyCurrentTrophyShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "HasTeamAlreadyCurrentTrophyFalse_TeamLeagues_DB")
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
                Email = $"footeo@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = $"footeo",
                Town = town,
                PasswordHash = "123123",
                Player = new Player
                {
                    FullName = "Footeo User"
                }
            };

            dbContext.Users.Add(user2);
            dbContext.SaveChanges();

            userManager.Setup(u => u.RemoveFromRoleAsync(user2, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user2, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            teamsService.CreateTeam("Team", "TTT", user.UserName);
            teamsService.CreateTeam("Team2", "TTT", user2.UserName);
            var team = dbContext.Teams.FirstOrDefault(n => n.Name == "Team");
            var team2 = dbContext.Teams.FirstOrDefault(n => n.Name == "Team2");

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(l => l.Name == "League");

            var teamLeaguesService = new TeamLeaguesService(dbContext, teamsService, leaguesService);

            teamLeaguesService.JoinLeague(user.UserName, league.Id);
            teamLeaguesService.JoinLeague(user2.UserName, league.Id);

            var teamLeague1 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team);
            var teamLeague2 = dbContext.TeamsLeagues.FirstOrDefault(t => t.Team == team2);

            teamLeague1.Points = 100;
            teamLeague2.Points = 50;
            dbContext.SaveChanges();

            league.Status = LeagueStatus.Completed;
            dbContext.SaveChanges();

            var winner = teamLeaguesService.GetTeamLeagueWinner(league.Id);

            var trophy = new Trophy
            {
                Name = "Trophy",
                Team = winner.Team
            };

            dbContext.Trophies.Add(trophy);
            dbContext.SaveChanges();

            var hasTeamAlreadyCurrentTrophy = teamLeaguesService.HasTeamAlreadyCurrentTrophy(league.Id);

            Assert.False(hasTeamAlreadyCurrentTrophy);
        }
    }
}