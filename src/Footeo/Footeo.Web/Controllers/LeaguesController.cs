namespace Footeo.Web.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.Input;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    public class LeaguesController : BaseController
    {
        private readonly ILeaguesService leaguesService;

        public LeaguesController(ILeaguesService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        public IActionResult All()
        {
            var leagues = this.leaguesService.All();

            return View(leagues);
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(LeagueInputModel model)
        {
            this.leaguesService.CreateLeague(model.Name, model.Description, model.News, model.TownId);

            return this.Redirect("/Leagues/All");
        }
    }
}