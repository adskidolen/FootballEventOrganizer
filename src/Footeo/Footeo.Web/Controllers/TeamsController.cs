namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Input;

    public class TeamsController : BaseController
    {
        private readonly ITeamsService teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            this.teamsService = teamsService;
        }

        public IActionResult All()
        {
            var teams = this.teamsService.All();

            return View(teams);
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(TeamInputModel model)
        {
            this.teamsService.CreateTeam(model.Name, model.Initials, model.Logo, model.TownId);

            return this.Redirect("/Teams/All");
        }
    }
}