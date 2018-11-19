namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Input;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Footeo.Web.ViewModels.Teams.View;

    public class TeamsController : BaseController
    {
        private readonly ITeamsService teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            this.teamsService = teamsService;
        }

        [Authorize]
        public IActionResult Create() => this.View();

        [Authorize]
        [HttpPost]
        public IActionResult Create(TeamCreateInputModel model)
        {
            if (this.teamsService.Exists(model.Name))
            {
                // TODO: Error for existing team
            }

            if (ModelState.IsValid)
            {
                this.teamsService.CreateTeam(model.Name, model.Initials, model.Town);

                return this.RedirectToAction(nameof(All));
            }

            return this.View();
        }

        public IActionResult All()
        {
            var teams = this.teamsService
                             .All()
                             .Select(vm => new TeamViewModel
                             {
                                 Name = vm.Name,
                                 Initials = vm.Initials,
                                 CreatedOn = vm.CreatedOn,
                                 Town = vm.Town.Name
                             })
                             .ToList();

            var teamViewModels = new AllTeamsViewModel
            {
                Teams = teams
            };

            return View(teamViewModels);
        }
    }
}