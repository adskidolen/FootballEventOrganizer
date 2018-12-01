namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.Input;
    using Footeo.Web.Utilities;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    [Area("Admin")]
    public class LeaguesController : BaseController
    {
        private readonly ILeaguesService leaguesService;

        public LeaguesController(ILeaguesService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        [Authorize(Roles = Constants.AdminRoleName)]
        public IActionResult Create() => this.View();

        [Authorize(Roles = Constants.AdminRoleName)]
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