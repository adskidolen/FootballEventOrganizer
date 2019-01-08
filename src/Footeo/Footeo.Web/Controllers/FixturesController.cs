namespace Footeo.Web.Controllers
{
    using System.Linq;

    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.ViewModels.Fixtures.Output;
    using Footeo.Web.ViewModels.Matches.Output;

    using Microsoft.AspNetCore.Mvc;

    public class FixturesController : BaseController
    {
        private readonly IFixturesService fixturesService;
        private readonly ILeaguesService leaguesService;
        private readonly IMatchesService matchesService;

        public FixturesController(IFixturesService fixturesService, ILeaguesService leaguesService, IMatchesService matchesService)
        {
            this.fixturesService = fixturesService;
            this.leaguesService = leaguesService;
            this.matchesService = matchesService;
        }

        public IActionResult All(int id)
        {
            var leagueExists = this.leaguesService.LeagueExistsById(id);
            if (!leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.LeagueDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var fixtures = this.fixturesService.AllFixtures<FixtureViewModel>(id)
                                               .OrderBy(f => f.Name)
                                               .ToList();

            var fixturesViewModel = new AllFixturesViewModel
            {
                Fixtures = fixtures
            };

            return View(fixturesViewModel);
        }

        public IActionResult Details(int id)
        {
            var fixtureExists = this.fixturesService.FixtureExistsById(id);
            if (!fixtureExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.FixtureDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var matches = this.matchesService.MatchesByFixture<MatchDetailsViewModel>(id).ToList();
            var matchesViewModel = new AllMatchesViewModel
            {
                Matches = matches
            };

            return this.View(matchesViewModel);
        }
    }
}