namespace Footeo.Web.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.ViewModels.Matches.Output;

    using Microsoft.AspNetCore.Mvc;

    using System.Linq;

    public class MatchesController : BaseController
    {
        private readonly IMatchesService matchesService;

        public MatchesController(IMatchesService matchesService)
        {
            this.matchesService = matchesService;
        }

        public IActionResult Details(int id)
        {
            var matchExists = this.matchesService.MatchExistsById(id);
            if (!matchExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.MatchDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var fixtureId = this.matchesService.GetFixturesIdByMatch(id);

            var matches = this.matchesService.MatchesByFixture<MatchDetailsViewModel>(fixtureId).ToList();

            var matchesViewModel = new AllMatchesViewModel
            {
                Matches = matches
            };

            return View(matchesViewModel);
        }
    }
}