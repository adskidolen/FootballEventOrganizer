namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Admin.Controllers.Base;
    using Footeo.Web.Areas.Admin.ViewModels.Matches.Input;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using System.Linq;

    public class MatchesController : AdminBaseController
    {
        private readonly ITeamLeaguesService teamLeaguesService;
        private readonly IFixturesService fixturesService;
        private readonly IFieldsService fieldsService;
        private readonly IPlayersService usersService;
        private readonly IMatchesService matchesService;

        public MatchesController(ITeamLeaguesService teamLeaguesService, IFixturesService fixturesService,
            IFieldsService fieldsService, IPlayersService usersService, IMatchesService matchesService)
        {
            this.teamLeaguesService = teamLeaguesService;
            this.fixturesService = fixturesService;
            this.fieldsService = fieldsService;
            this.usersService = usersService;
            this.matchesService = matchesService;
        }

        public IActionResult Create(int id)
        {
            // TODO: find other way without ViewData

            this.ViewData[GlobalConstants.HomeTeamsViewDataName] = this.teamLeaguesService.LeagueTable<SelectListItem>(id).ToList();
            this.ViewData[GlobalConstants.AwayTeamsViewDataName] = this.teamLeaguesService.LeagueTable<SelectListItem>(id).ToList();
            this.ViewData[GlobalConstants.FieldsViewDataName] = this.fieldsService.AllFields<SelectListItem>().ToList();
            this.ViewData[GlobalConstants.FixturesViewDataName] = this.fixturesService.AllFixtures<SelectListItem>(id).ToList();

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateMatchInputModel model)
        {
            var fixtureLeague = this.fixturesService.GetLeagueForFixture(model.FixtureId);

            // TODO: find other way without ViewData

            this.ViewData[GlobalConstants.HomeTeamsViewDataName] = this.teamLeaguesService.LeagueTable<SelectListItem>(fixtureLeague.Id).ToList();
            this.ViewData[GlobalConstants.AwayTeamsViewDataName] = this.teamLeaguesService.LeagueTable<SelectListItem>(fixtureLeague.Id).ToList();
            this.ViewData[GlobalConstants.FieldsViewDataName] = this.fieldsService.AllFields<SelectListItem>().ToList();
            this.ViewData[GlobalConstants.FixturesViewDataName] = this.fixturesService.AllFixtures<SelectListItem>(fixtureLeague.Id).ToList();

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.matchesService.CreateMatch(model.HomeTeamId, model.AwayTeamId, model.FieldId, model.FixtureId);

            var routeValues = new { Id = model.FixtureId, Area = GlobalConstants.EmptyArea };

            return this.RedirectToAction(controllerName: GlobalConstants.FixturesControllerName,
                                         actionName: GlobalConstants.DetailsActionName,
                                         routeValues: routeValues);
        }
    }
}