namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.Input;
    using Footeo.Common;

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
            if (this.leaguesService.ExistsByName(model.Name))
            {
                // TODO: Error for existing league
            }

            if (ModelState.IsValid)
            {
                this.leaguesService.CreateLeague(model.Name, model.Description, model.Town);

                return this.RedirectToAction(actionName: "All", controllerName: "Leagues");
            }

            return this.View(model);
        }
    }
}