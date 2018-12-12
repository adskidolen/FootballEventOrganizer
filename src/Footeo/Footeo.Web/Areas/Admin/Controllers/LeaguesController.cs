namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Leagues.Input;
    using Footeo.Common;
    using Footeo.Web.ViewModels;
    using Footeo.Web.Areas.Admin.Controllers.Base;

    using Microsoft.AspNetCore.Mvc;

    public class LeaguesController : AdminBaseController
    {
        private readonly ILeaguesService leaguesService;

        public LeaguesController(ILeaguesService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(LeagueCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var leagueExists = this.leaguesService.LeagueExistsByName(model.Name);
            if (leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.LeagueExistsErrorMessage, model.Name)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.leaguesService.CreateLeague(model.Name, model.Description, model.StartDate, model.EndDate, model.Town);

            var routeValues = new { Area = GlobalConstants.EmptyArea };

            return this.RedirectToAction(actionName: GlobalConstants.AllActionName,
                                         controllerName: GlobalConstants.LeaguesControllerName,
                                         routeValues: routeValues);
        }
    }
}