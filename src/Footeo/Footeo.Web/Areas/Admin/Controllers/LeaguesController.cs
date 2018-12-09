namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.Input;
    using Footeo.Common;
    using Footeo.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    [Area(GlobalConstants.AdminRoleName)]
    public class LeaguesController : BaseController
    {
        private readonly ILeaguesService leaguesService;

        public LeaguesController(ILeaguesService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Create() => this.View();

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        [HttpPost]
        public IActionResult Create(LeagueCreateInputModel model)
        {
            var leagueExists = this.leaguesService.LeagueExistsByName(model.Name);
            if (leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.LeagueExistsErrorMessage, model.Name)
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            if (ModelState.IsValid)
            {
                this.leaguesService.CreateLeague(model.Name, model.Description, model.StartDate, model.EndDate, model.Town);

                return this.RedirectToAction(actionName: GlobalConstants.AllActionName,
                                             controllerName: GlobalConstants.LeaguesControllerName,
                                             routeValues: new { Area = GlobalConstants.EmptyArea });
            }

            return this.View(model);
        }
    }
}