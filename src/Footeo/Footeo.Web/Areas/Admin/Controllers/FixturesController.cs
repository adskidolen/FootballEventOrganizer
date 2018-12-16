namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Admin.Controllers.Base;
    using Footeo.Web.Areas.Admin.ViewModels.Fixtures.Input;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using System.Linq;

    public class FixturesController : AdminBaseController
    {
        private readonly IFixturesService fixturesService;
        private readonly ILeaguesService leaguesService;

        public FixturesController(IFixturesService fixturesService, ILeaguesService leaguesService)
        {
            this.fixturesService = fixturesService;
            this.leaguesService = leaguesService;
        }

        public IActionResult Create()
        {
            this.ViewData[GlobalConstants.ViewDataForLeagues] = this.leaguesService.AllLeagues<SelectListItem>().ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(FixtureCreateInputModel model)
        {
            this.ViewData[GlobalConstants.ViewDataForLeagues] = this.leaguesService.AllLeagues<SelectListItem>().ToList();

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.fixturesService.CreateFixture(model.Name, model.Date, model.LeagueId);

            //var routeValues = new { Id = model.LeagueId, Area = GlobalConstants.EmptyArea };

            //return this.RedirectToAction(actionName: GlobalConstants.AllActionName,
            //                             controllerName: GlobalConstants.FixturesControllerName,
            //                             routeValues: routeValues);

            var routeValues = new { Id = model.LeagueId };

            return this.RedirectToAction(actionName: GlobalConstants.CreateActionName,
                                         controllerName: GlobalConstants.MatchesControllerName,
                                         routeValues: routeValues);
        }
    }
}