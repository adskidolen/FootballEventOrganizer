﻿namespace Footeo.Web.Areas.Admin.Controllers
{
    using System.Linq;

    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Admin.Controllers.Base;
    using Footeo.Web.Areas.Admin.ViewModels.Leagues.Output;
    using Footeo.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class TrophiesController : AdminBaseController
    {
        private readonly ILeaguesService leaguesService;
        private readonly ITrophiesService trophiesService;

        public TrophiesController(ILeaguesService leaguesService, ITrophiesService trophiesService)
        {
            this.leaguesService = leaguesService;
            this.trophiesService = trophiesService;
        }

        public IActionResult TrophiesForLeagues()
        {
            var leagues = this.leaguesService.AllCompletedLeagues<LeagueViewModel>().ToList();
            var leaguesViewModel = new AllLeaguesViewModel
            {
                Leagues = leagues
            };

            return this.View(leaguesViewModel);
        }

        public IActionResult GenerateWinner(int id)
        {
            var leagueExists = this.leaguesService.LeagueExistsById(id);
            if (!leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.LeagueDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.trophiesService.CreateTrophy(id);

            var routeValues = new { Area = GlobalConstants.EmptyArea };

            return this.RedirectToAction(controllerName: GlobalConstants.LeaguesControllerName,
                                         actionName: GlobalConstants.CompletedActionName,
                                         routeValues: routeValues);
        }
    }
}