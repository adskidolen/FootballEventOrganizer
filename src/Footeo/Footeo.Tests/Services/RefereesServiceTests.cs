namespace Footeo.Tests.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services;
    using Footeo.Tests.Base;
    using Footeo.Web.ViewModels.Referees.Output;

    using Microsoft.EntityFrameworkCore;

    using NUnit.Framework;

    using System;
    using System.Linq;

    public class RefereesServiceTests : BaseServiceTests
    {
        [Test]
        public void CreateRefereeShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "CreateReferee_Referees_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoReferee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"footeoReferee",
                TownId = new Random().Next(1, 20),
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
        public void RefereeAttendToMatchShouldNotReturnNull()
        {
            var options = new DbContextOptionsBuilder<FooteoDbContext>()
                  .UseInMemoryDatabase(databaseName: "AttendToMatch_Referees_DB")
                  .Options;

            var dbContext = new FooteoDbContext(options);

            var user = new FooteoUser
            {
                Age = new Random().Next(20, 30),
                Email = $"footeoReferee@mail.bg",
                FirstName = "Footeo",
                LastName = "Referee",
                UserName = $"footeoReferee",
                TownId = new Random().Next(1, 20),
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

            var match = new Models.Match();
            dbContext.Matches.Add(match);
            dbContext.SaveChanges();

            refereesService.AttendAMatch(user.UserName, match.Id);

            Assert.NotNull(match.Referee);
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
    }
}