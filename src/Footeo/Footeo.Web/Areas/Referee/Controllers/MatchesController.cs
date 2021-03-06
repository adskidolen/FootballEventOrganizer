﻿namespace Footeo.Web.Areas.Referee.Controllers
{
    using Footeo.Common;
    using Footeo.Models;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels;
    using Footeo.Web.Areas.Referee.Controllers.Base;
    using Footeo.Web.Areas.Referee.ViewModels.Matches.Input;

    using Microsoft.AspNetCore.Mvc;

    public class MatchesController : RefereeBaseController
    {
        private readonly IMatchesService matchesService;
        private readonly IRefereesService refereesService;
        private readonly IPlayersService playersService;

        public MatchesController(IMatchesService matchesService, IRefereesService refereesService,
            IPlayersService playersService)
        {
            this.matchesService = matchesService;
            this.refereesService = refereesService;
            this.playersService = playersService;
        }

        public IActionResult Add() => this.View();

        [HttpPost]
        public IActionResult Add(int id, AddMatchInfoInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var matchExists = this.matchesService.MatchExistsById(id);
            if (!matchExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.MatchDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var matchHasResult = this.matchesService.MatchHasResult(id);
            if (matchHasResult)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.MatchHasResultErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var match = this.matchesService.GetMatchById<Match>(id);

            this.refereesService.AddResultToMatch(match.Id, model.HomeTeamGoals, model.AwayTeamGoals);

            var fixtureId = match.Fixture.Id;

            var routeValues = new { Id = fixtureId };

            return this.RedirectToAction(controllerName: GlobalConstants.FixturesControllerName,
                                         actionName: GlobalConstants.DetailsActionName,
                                         routeValues: routeValues);

        }

        public IActionResult Attend(int id)
        {
            var matchExists = this.matchesService.MatchExistsById(id);
            if (!matchExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.MatchDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var matchHasAreferee = this.matchesService.MatchHasReferee(id);
            if (matchHasAreferee)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.MatchHasRefereeErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var user = this.User.Identity.Name;

            this.refereesService.AttendAMatch(user, id);

            var match = this.matchesService.GetMatchById<Match>(id);
            var fixtureId = match.Fixture.Id;

            var routeValues = new { Id = fixtureId };

            return this.RedirectToAction(controllerName: GlobalConstants.FixturesControllerName,
                                         actionName: GlobalConstants.DetailsActionName,
                                         routeValues: routeValues);
        }
    }
}