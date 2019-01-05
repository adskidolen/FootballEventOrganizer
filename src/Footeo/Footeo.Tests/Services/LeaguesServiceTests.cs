namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Leagues.Output;

    using Microsoft.EntityFrameworkCore;

    using NUnit.Framework;

    using System;
    using System.Linq;

    public class LeaguesServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateLeagueShouldReturnCorrectCountOfLeagues()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateLeague_Leagues_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            for (int i = 1; i <= 5; i++)
            {
                leaguesService.CreateLeague($"League{i}", $"Description{i}", DateTime.UtcNow.AddDays(i), DateTime.UtcNow.AddDays(i * i), "Sofia");
            }

            var leaguesCount = dbContext.Leagues.CountAsync().Result;
            var expectedLeaguesCount = 5;

            Assert.AreEqual(expectedLeaguesCount, leaguesCount);
        }

        [Test]
        public void LeagueExistsByIdShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "LeagueExistsById_Leagues_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague("League", "Description", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            var leagueExists = leaguesService.LeagueExistsById(league.Id);

            Assert.True(leagueExists);
        }

        [Test]
        public void LeagueExistsByIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "LeagueExistsById_Leagues_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague("League", "Description", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            var notValidLeagueId = 2;

            var leagueExists = leaguesService.LeagueExistsById(notValidLeagueId);

            Assert.False(leagueExists);
        }

        [Test]
        public void LeagueExistsByNameShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "LeagueExistsByName_Leagues_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague("League", "Description", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            var leagueExists = leaguesService.LeagueExistsByName(league.Name);

            Assert.True(leagueExists);
        }

        [Test]
        public void LeagueExistsByNameShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "LeagueExistsByName_Leagues_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague("League", "Description", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");
            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            var notValidLeagueName = "League1";

            var leagueExists = leaguesService.LeagueExistsByName(notValidLeagueName);

            Assert.False(leagueExists);
        }

        [Test]
        public void GetLeagueByIdShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "GetLeagueByName_Leagues_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague("League", "Description", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");

            var leagueId = dbContext.Leagues.FirstOrDefault(n => n.Name == "League").Id;
            var league = leaguesService.GetLeagueById<League>(leagueId);

            Assert.NotNull(league);
        }

        [Test]
        public void GetLeagueByIdShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "GetLeagueByName_Leagues_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague("League", "Description", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");

            var invalidLeagueId = 2;
            var league = leaguesService.GetLeagueById<League>(invalidLeagueId);

            Assert.Null(league);
        }

        [Test]
        public void AllPendingLeaguesShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "PendingLeagues_Leagues_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            for (int i = 1; i <= 3; i++)
            {
                leaguesService.CreateLeague($"League{i}", $"Description{i}", DateTime.UtcNow.AddDays(i), DateTime.UtcNow.AddDays(i * i), "Sofia");
            }

            var pendingLeagues = leaguesService.AllPendingLeagues<PendingLeagueViewModel>().ToList();
            var pendingLeaguesCount = pendingLeagues.Count;

            var expectedPendingLeaguesCount = 3;

            Assert.AreEqual(expectedPendingLeaguesCount, pendingLeaguesCount);
        }

        [Test]
        public void SetLeagueStatusToInProgressAndReturnAllInProgressLeagues()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                          .UseInMemoryDatabase(databaseName: "InProgressLeagues_Leagues_DB")
                          .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            for (int i = 1; i <= 5; i++)
            {
                leaguesService.CreateLeague($"League{i}", $"Description{i}", DateTime.UtcNow.AddDays(i), DateTime.UtcNow.AddDays(i * i), "Sofia");
            }

            var leagues = dbContext.Leagues.ToList();

            foreach (var league in leagues)
            {
                leaguesService.SetLeagueStatusToInProgress(league.Id);
            }

            var inProgressLeagues = leaguesService.AllInProgressLeagues<InProgressLeagueViewModel>().ToList();

            var inProgressLeaguesCount = inProgressLeagues.Count;

            var expectedInProgressLeaguesCount = 5;

            Assert.AreEqual(expectedInProgressLeaguesCount, inProgressLeaguesCount);
        }

        [Test]
        public void SetLeagueStatusToCompletedAndReturnAllCompletedLeagues()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "CompletedLeagues_Leagues_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            for (int i = 1; i <= 5; i++)
            {
                leaguesService.CreateLeague($"League{i}", $"Description{i}", DateTime.UtcNow.AddDays(i), DateTime.UtcNow.AddDays(i * i), "Sofia");
            }

            var leagues = dbContext.Leagues.ToList();

            foreach (var league in leagues)
            {
                leaguesService.SetLeagueStatusToCompleted(league.Id);
            }

            var completedLeagues = leaguesService.AllCompletedLeagues<CompletedLeagueViewModel>().ToList();

            var completedLeaguesCount = completedLeagues.Count;

            var expectedCompletedLeaguesCount = 5;

            Assert.AreEqual(expectedCompletedLeaguesCount, completedLeaguesCount);
        }

        [Test]
        public void SetLeagueStatusShouldReturnInProgressStatus()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                          .UseInMemoryDatabase(databaseName: "SetInProgressStatus_Leagues_DB")
                          .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague($"LeagueIP", $"Description", DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddMonths(2), "Sofia");

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "LeagueIP");

            leaguesService.SetLeagueStatusToInProgress(league.Id);

            var expectedLeagueStatus = "InProgress";

            Assert.AreEqual(expectedLeagueStatus, league.Status.ToString());
        }

        [Test]
        public void SetLeagueStatusShouldReturnCompletedStatus()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                         .UseInMemoryDatabase(databaseName: "SetCompletedStatus_Leagues_DB")
                         .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);

            leaguesService.CreateLeague($"LeagueC", $"Description", DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddMonths(3), "Varna");

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "LeagueC");

            leaguesService.SetLeagueStatusToCompleted(league.Id);

            var expectedLeagueStatus = "Completed";

            Assert.AreEqual(expectedLeagueStatus, league.Status.ToString());
        }
    }
}