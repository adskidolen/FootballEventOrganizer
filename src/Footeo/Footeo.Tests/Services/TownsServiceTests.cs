namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services;
    using Footeo.Tests.Base;

    using Microsoft.EntityFrameworkCore;

    using NUnit.Framework;

    public class TownsServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateTownShouldReturnCorrectTownsCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
               .UseInMemoryDatabase(databaseName: "CreateTown_Towns_DB")
               .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);

            for (int i = 1; i <= 50; i++)
            {
                townsService.CreateTown($"Town{i}");
            }

            var townsCount = dbContext.Towns.CountAsync().Result;
            var expectedTownsCount = 50;

            Assert.AreEqual(expectedTownsCount, townsCount);
        }

        [Test]
        public void GetTownByNameShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
              .UseInMemoryDatabase(databaseName: "GetTownByName_Towns_DB")
              .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);

            townsService.CreateTown("Sofia");

            var town = townsService.GetTownByName<Town>("Sofia");

            Assert.NotNull(town);
        }

        [Test]
        public void GetTownByNameShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
             .UseInMemoryDatabase(databaseName: "GetTownByName_Towns_DB")
             .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);

            townsService.CreateTown("Sofia");

            var town = townsService.GetTownByName<Town>("Burgas");

            Assert.Null(town);
        }
    }
}