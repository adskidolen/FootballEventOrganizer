namespace Footeo.Web.Areas.Referee.Controllers
{
    using Footeo.Common;
    using Footeo.Models;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Referee.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.Areas.Referee.ViewModels.Matches.Input;

    using Microsoft.AspNetCore.Mvc;

    public class MatchesController : RefereeBaseController
    {
        private readonly IMatchesService matchesService;
        private readonly IRefereesService refereesService;

        public MatchesController(IMatchesService matchesService, IRefereesService refereesService)
        {
            this.matchesService = matchesService;
            this.refereesService = refereesService;
        }

        public IActionResult Add() => this.View();

        [HttpPost]
        public IActionResult Add(int id, AddMatchInfoInputModel model)
        {
            var matchExists = this.matchesService.MatchExistsById(id);
            if (!matchExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.MatchDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var matchHasResult = this.matchesService.MatchHasResult(id);
            if (matchHasResult)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.MatchHasResultErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var match = this.matchesService.GetMatchById<Match>(id);
            this.refereesService.AddResultToMatch(match.Id, model.HomeTeamGoals, model.AwayTeamGoals);

            var routeValues = new { match.Id };

            return this.RedirectToAction(controllerName: GlobalConstants.MatchesControllerName,
                                         actionName: GlobalConstants.DetailsActionName,
                                         routeValues: routeValues);
        }

        public IActionResult Join(int id)
        {
            var matchExists = this.matchesService.MatchExistsById(id);
            if (!matchExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.MatchDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var matchHasAreferee = this.matchesService.MatchHasReferee(id);
            if (matchHasAreferee)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.MatchHasRefereeErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var user = this.User.Identity.Name;

            this.refereesService.JoinMatch(user, id);

            var routeValues = new { Id = id };

            return this.RedirectToAction(controllerName: GlobalConstants.MatchesControllerName,
                                         actionName: GlobalConstants.DetailsActionName,
                                         routeValues: routeValues);
        }
    }
}