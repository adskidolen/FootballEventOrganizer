namespace Footeo.Web.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.Input;
    using Footeo.Web.ViewModels.Leagues.View;
    using Footeo.Web.Utilities;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

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

                return this.RedirectToAction(nameof(All));
            }

            return this.View(model);
        }

        public IActionResult All()
        {
            var leagues = this.leaguesService
                              .All()
                              .Select(vm => new LeagueViewModel
                              {
                                  Name = vm.Name,
                                  Description = vm.Description,
                                  StartDate = vm.StartDate,
                                  EndDate = vm.EndDate,
                                  Status = vm.Status,
                                  Town = vm.Town.Name
                              })
                              .ToList();

            var leagueViewModels = new AllLeaguesViewModel
            {
                Leagues = leagues
            };

            return View(leagueViewModels);
        }
    }
}