namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Fixtures.Output;

    using Microsoft.EntityFrameworkCore;

    using NUnit.Framework;

    using System;
    using System.Linq;

    [TestFixture]
    public class FixturesServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateFixtureShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "CreateFixture_Fixtures_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var fixturesService = new FixturesService(dbContext, leaguesService);

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow, league.Id);

            var fixture = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday 1");

            Assert.NotNull(fixture);
        }

        [Test]
        public void GetFixtureByNameShouldReturnCorrectFixture()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "GetFixtureByName_Fixtures_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var fixturesService = new FixturesService(dbContext, leaguesService);

            leaguesService.CreateLeague("League", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League");

            fixturesService.CreateFixture("Matchday 1", DateTime.UtcNow, league.Id);

            var fixtureId = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday 1").Id;
            var fixture = fixturesService.GetFixtureById<FixtureViewModel>(fixtureId);

            var expectedFixtureName = "Matchday 1";

            Assert.AreEqual(expectedFixtureName, fixture.Name);
        }

        [Test]
        public void FixtureExistsByIdShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "FixtureExistsByIdTrue_Fixtures_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var fixturesService = new FixturesService(dbContext, leaguesService);

            leaguesService.CreateLeague("League1", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League1");

            fixturesService.CreateFixture("Matchday 2", DateTime.UtcNow, league.Id);

            var fixtureId = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday 2").Id;

            var fixtureExists = fixturesService.FixtureExistsById(fixtureId);

            Assert.True(fixtureExists);
        }

        [Test]
        public void FixtureExistsByIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "FixtureExistsByIdFalse_Fixtures_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var fixturesService = new FixturesService(dbContext, leaguesService);

            leaguesService.CreateLeague("League1", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Sofia");

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League1");

            fixturesService.CreateFixture("Matchday 2", DateTime.UtcNow, league.Id);

            var fixtureId = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday 2").Id;
            var invalidFixtureId = 392;

            var fixtureExists = fixturesService.FixtureExistsById(invalidFixtureId);

            Assert.False(fixtureExists);
        }

        [Test]
        public void AllFixturesByLeagueIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "AllFixturesByLeague_Fixtures_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var fixturesService = new FixturesService(dbContext, leaguesService);

            leaguesService.CreateLeague("League2", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Varna");

            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League2");

            for (int i = 1; i <= 20; i++)
            {
                fixturesService.CreateFixture($"Matchday {i}", DateTime.UtcNow.AddDays(i + 7), league.Id);
            }

            var fixtures = fixturesService.AllFixtures<FixtureViewModel>(league.Id).ToList();

            var expectedFixtures = 20;

            Assert.AreEqual(expectedFixtures, fixtures.Count);
        }

        [Test]
        public void GetLeagueForFixtureShouldReturnCorrectLeague()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "GetLeagueForFixture_Fixtures_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);
            var townsService = new TownsService(dbContext);
            var leaguesService = new LeaguesService(dbContext, townsService);
            var fixturesService = new FixturesService(dbContext, leaguesService);

            leaguesService.CreateLeague("League3", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddMonths(2), "Plovdiv");
            var league = dbContext.Leagues.FirstOrDefault(n => n.Name == "League3");

            fixturesService.CreateFixture("Matchday 3", DateTime.UtcNow, league.Id);
            var fixture = dbContext.Fixtures.FirstOrDefault(n => n.Name == "Matchday 3");

            var leagueForFixture = fixturesService.GetLeagueForFixture(fixture.Id);

            Assert.AreEqual(league, leagueForFixture);
        }
    }
}