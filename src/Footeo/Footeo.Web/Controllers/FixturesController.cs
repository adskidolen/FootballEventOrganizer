namespace Footeo.Web.Controllers
{
    using System.Linq;

    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.ViewModels.Fixtures.Output;

    using Microsoft.AspNetCore.Mvc;

    public class FixturesController : BaseController
    {
        private readonly IFixturesService fixturesService;
        private readonly ILeaguesService leaguesService;

        public FixturesController(IFixturesService fixturesService, ILeaguesService leaguesService)
        {
            this.fixturesService = fixturesService;
            this.leaguesService = leaguesService;
        }

        public IActionResult All(int id)
        {
            var leagueExists = this.leaguesService.LeagueExistsById(id);
            if (!leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.LeagueDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var fixtures = this.fixturesService.AllFixtures<FixturesViewModel>(id)
                                               .OrderBy(d => d.Date)
                                               .ToList();

            var fixturesViewModel = new AllFixturesViewModel
            {
                Fixtures = fixtures
            };

            return View(fixturesViewModel);
        }
    }
}