namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Fields.Output;

    using Microsoft.EntityFrameworkCore;

    using NUnit.Framework;

    using System.Linq;

    public class FieldsServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateFieldShouldReturnCorrectCountOfFields()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateField_Fields_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var fieldsService = new FieldsService(dbContext, townsService);

            for (int i = 1; i <= 20; i++)
            {
                fieldsService.CreateField($"Field{i}", $"Address{i}", false, "Sofia");
            }

            var fieldsCount = dbContext.Fields.CountAsync().Result;
            var expectedFieldsCount = 20;

            Assert.AreEqual(expectedFieldsCount, fieldsCount);
        }

        [Test]
        public void FieldExistsByNameShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "FieldExists_Fields_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var fieldsService = new FieldsService(dbContext, townsService);

            fieldsService.CreateField("Field", "Address", true, "Burgas");

            var fieldName = "Field";
            var fieldExists = fieldsService.FieldExistsByName(fieldName);

            Assert.True(fieldExists);
        }

        [Test]
        public void FieldExistsByNameShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "FieldExists_Fields_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var fieldsService = new FieldsService(dbContext, townsService);

            fieldsService.CreateField("Field", "Address", true, "Burgas");

            var fieldNameForTest = "Field1";
            var fieldExists = fieldsService.FieldExistsByName(fieldNameForTest);

            Assert.False(fieldExists);
        }

        [Test]
        public void AllFieldsShouldReturnCorrectFieldsCount()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                .UseInMemoryDatabase(databaseName: "AllFields_Fields_DB")
                .Options;

            var dbContext = new FooteoDbContext(options);

            var townsService = new TownsService(dbContext);
            var fieldsService = new FieldsService(dbContext, townsService);

            for (int i = 1; i <= 10; i++)
            {
                fieldsService.CreateField($"Field{i}", $"Address{i}", false, "Sofia");
            }

            var fields = fieldsService.AllFields<FieldViewModel>().ToList();

            var expectedFieldsCount = 10;

            Assert.AreEqual(expectedFieldsCount, fields.Count);
        }
    }
}