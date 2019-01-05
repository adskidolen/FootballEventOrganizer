namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Models.Enums;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Players.Output;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using NUnit.Framework;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class PlayersServiceTests : BaseServiceTests
    {
        [Test]
        public void CreatePlayerShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "CreatePlayer_Players_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            Assert.NotNull(user.Player);
        }

        [Test]
        public void PlayerHasATeamShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "PlayerHasATeamFalse_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            var playerUser = dbContext.Players.FirstOrDefault(p => p.FullName == "Footeo Player");

            var playerHasATeam = playersService.PlayerHasATeam(user.UserName);

            Assert.False(playerHasATeam);
        }

        [Test]
        public void PlayerHasATeamShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "PlayerHasATeamTrue_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            var playerUser = dbContext.Players.FirstOrDefault(p => p.FullName == "Footeo Player");
            playerUser.Team = new Team
            {
                Name = "Team",
                Initials = "TTT"
            };
            dbContext.SaveChanges();

            var playerHasATeam = playersService.PlayerHasATeam(user.UserName);

            Assert.True(playerHasATeam);
        }

        [Test]
        public void SetPlayersNicknameShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "SetPlayersNickname_Players_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            playersService.SetPlayersNickname(user.UserName, "usercheto");

            var nickname = player.Nickname;
            var expectedNickname = "usercheto";

            Assert.AreEqual(expectedNickname, nickname);
        }

        [Test]
        public void SetSquadNumberShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "SetSquadNumber_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            playersService.SetSquadNumber(user.UserName, 7);

            var squadNumber = player.SquadNumber;
            var expectedSquadNumber = 7;

            Assert.AreEqual(expectedSquadNumber, squadNumber);
        }

        [Test]
        public void SetPositionShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "SetPosition_Players_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            playersService.SetPosition(user.UserName, PlayerPosition.Forward);

            var position = player.Position.ToString();
            var expectedPosition = "Forward";

            Assert.AreEqual(expectedPosition, position);
        }

        [Test]
        public void PlayerExistsByIdShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "PlayerExistsByIdTrue_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            var playerExists = playersService.PlayerExistsById(player.Id);

            Assert.True(playerExists);
        }

        [Test]
        public void PlayerExistsByIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "PlayerExistsByIdFalse_Players_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            var invalidPlayerId = 83;
            var playerExists = playersService.PlayerExistsById(invalidPlayerId);

            Assert.False(playerExists);
        }

        [Test]
        public void GetPlayerByIdShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetPlayerById_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            var playerModel = playersService.GetPlayerById<PlayerDetailsViewModel>(player.Id);

            Assert.NotNull(playerModel);
        }

        [Test]
        public void GetPlayerByIdShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetPlayerByIdNull_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var playersService = new PlayersService(dbContext, null, null, null);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            playersService.CreatePlayer(user, player);

            var invalidPlayerId = 38;
            var playerModel = playersService.GetPlayerById<PlayerDetailsViewModel>(invalidPlayerId);

            Assert.Null(playerModel);
        }

        [Test]
        public void PlayerJoinTeamShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "JoinTeam_Players_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(user, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));

            var leaguesService = new LeaguesService(dbContext, townsService);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            var playerUser = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "player@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "player",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(playerUser);
            dbContext.SaveChanges();

            var playerTwo = new Player
            {
                FullName = "Footeo Player"
            };

            userManager.Setup(u => u.RemoveFromRoleAsync(playerUser, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            var playersService = new PlayersService(dbContext, userManager.Object, null, teamsService);
            playersService.CreatePlayer(user, player);
            playersService.CreatePlayer(playerUser, playerTwo);

            teamsService.CreateTeam("Team", "TTT", playerUser.UserName);
            var team = dbContext.Teams.FirstOrDefault(t => t.Name == "Team");

            playersService.JoinTeam(team.Id, user.UserName);

            var playerTeam = player.Team;

            Assert.NotNull(playerTeam);
        }

        [Test]
        public void PlayersByTeamShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "PlayersByTeam_Players_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(user, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));

            var leaguesService = new LeaguesService(dbContext, townsService);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            var playerUser = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "player@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "player",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(playerUser);
            dbContext.SaveChanges();

            var playerTwo = new Player
            {
                FullName = "Footeo Player"
            };

            userManager.Setup(u => u.RemoveFromRoleAsync(playerUser, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            var playersService = new PlayersService(dbContext, userManager.Object, null, teamsService);
            playersService.CreatePlayer(user, player);
            playersService.CreatePlayer(playerUser, playerTwo);

            teamsService.CreateTeam("Team", "TTT", playerUser.UserName);
            var team = dbContext.Teams.FirstOrDefault(t => t.Name == "Team");

            playersService.JoinTeam(team.Id, user.UserName);

            var players = playersService.PlayersByTeam<PlayerTeamViewModel>(team.Id).ToList();

            var playersCount = players.Count;
            var expectedPlayersCount = 2;

            Assert.AreEqual(expectedPlayersCount, playersCount);
        }

        [Test]
        public void IsSquadNumberTakenShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "IsSquadNumberTakenFalse_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(user, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));

            var leaguesService = new LeaguesService(dbContext, townsService);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            var playerUser = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "player@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "player",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(playerUser);
            dbContext.SaveChanges();

            var playerTwo = new Player
            {
                FullName = "Footeo Player"
            };

            userManager.Setup(u => u.RemoveFromRoleAsync(playerUser, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            var playersService = new PlayersService(dbContext, userManager.Object, null, teamsService);
            playersService.CreatePlayer(user, player);
            playersService.CreatePlayer(playerUser, playerTwo);

            teamsService.CreateTeam("Team", "TTT", playerUser.UserName);
            var team = dbContext.Teams.FirstOrDefault(t => t.Name == "Team");

            playersService.JoinTeam(team.Id, user.UserName);

            var isSquadNumberTaken = playersService.IsSquadNumberTaken(7, team.Id);

            Assert.False(isSquadNumberTaken);
        }

        [Test]
        public void IsSquadNumberTakenShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                 .UseInMemoryDatabase(databaseName: "IsSquadNumberTakenTrue_Players_DB")
                 .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var town = townsService.CreateTown("Vraca");

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "footeoPlayer@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "footeoPlayer",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<FooteoUser>>();
            var userManager = new Mock<UserManager<FooteoUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(u => u.RemoveFromRoleAsync(user, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(user, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));

            var leaguesService = new LeaguesService(dbContext, townsService);

            var player = new Player
            {
                FullName = "Footeo Player"
            };

            var playerUser = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = "player@mail.bg",
                FirstName = "Footeo",
                LastName = "Player",
                UserName = "player",
                Town = town,
                PasswordHash = "123123"
            };

            dbContext.Users.Add(playerUser);
            dbContext.SaveChanges();

            var playerTwo = new Player
            {
                FullName = "Footeo Player"
            };

            userManager.Setup(u => u.RemoveFromRoleAsync(playerUser, "Player")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "PlayerInTeam")).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(playerUser, "Captain")).Returns(Task.FromResult(IdentityResult.Success));

            var teamsService = new TeamsService(dbContext, townsService, leaguesService, userManager.Object, null);

            var playersService = new PlayersService(dbContext, userManager.Object, null, teamsService);
            playersService.CreatePlayer(user, player);
            playersService.CreatePlayer(playerUser, playerTwo);

            teamsService.CreateTeam("Team", "TTT", playerUser.UserName);
            var team = dbContext.Teams.FirstOrDefault(t => t.Name == "Team");

            playersService.JoinTeam(team.Id, user.UserName);

            playersService.SetSquadNumber(user.UserName, 7);

            var isSquadNumberTaken = playersService.IsSquadNumberTaken(7, team.Id);

            Assert.True(isSquadNumberTaken);
        }
    }
}