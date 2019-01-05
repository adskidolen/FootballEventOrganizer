namespace Footeo.Tests.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers;
    using Footeo.Web.ViewModels.Home;
    using Footeo.Web.ViewModels.Leagues.Output;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using NUnit.Framework;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void IndexActionShouldReturnCorrectLeagueViewModels()
        {
            var mockLeaguesService = new Mock<ILeaguesService>();

            var leagues = new[]
            {
                new LeagueIndexViewModel
                {
                     Name = "League1",
                     Description = "Description1",
                     StartDate = DateTime.UtcNow.AddDays(3)
                },
                new LeagueIndexViewModel
                {
                     Name = "League2",
                     Description = "Description2",
                     StartDate = DateTime.UtcNow.AddDays(5)
                },
                new LeagueIndexViewModel
                {
                     Name = "League3",
                     Description = "Description3",
                     StartDate = DateTime.UtcNow.AddDays(7)
                },
            };

            mockLeaguesService.Setup(s => s.AllPendingLeagues<LeagueIndexViewModel>())
                              .Returns(PopulateLeagues(leagues));

            var homeController = new HomeController(mockLeaguesService.Object);

            var result = homeController.Index();

            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult)result;

            Assert.That(viewResult.Model, Is.AssignableTo<IndexViewModel>());
        }

        private static IQueryable<LeagueIndexViewModel> PopulateLeagues(IEnumerable<LeagueIndexViewModel> leagues)
        {
            return leagues.AsQueryable();
        }
    }
}