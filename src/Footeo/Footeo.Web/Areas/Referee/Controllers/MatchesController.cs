namespace Footeo.Web.Areas.Referee.Controllers
{
    using Footeo.Common;
    using Footeo.Models;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Referee.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.Areas.Referee.ViewModels.Matches.Input;
    using Footeo.Web.Areas.Referee.ViewModels.Players.Input;
    using Footeo.Web.Areas.Referee.ViewModels.Players.Output;

    using Microsoft.AspNetCore.Mvc;

    using System.Linq;
    using System.Collections.Generic;

    public class MatchesController : RefereeBaseController
    {
        private readonly IMatchesService matchesService;
        private readonly IRefereesService refereesService;
        private readonly IPlayersService playersService;
        private readonly IPlayersStatisticsService playersStatisticsService;

        public MatchesController(IMatchesService matchesService, IRefereesService refereesService,
            IPlayersService playersService, IPlayersStatisticsService playersStatisticsService)
        {
            this.matchesService = matchesService;
            this.refereesService = refereesService;
            this.playersService = playersService;
            this.playersStatisticsService = playersStatisticsService;
        }

        public IActionResult Add(int id)
        {
            var match = this.matchesService.GetMatchById<Match>(id);
            var homeTeam = match.HomeTeam;
            var awayTeam = match.AwayTeam;

            this.ViewData["HomeTeamPlayers"] = homeTeam.Players.Select(p => new PlayerViewModel
            {
                Id = p.Id,
                Name = p.FullName
            })
            .ToList();

            this.ViewData["AwayTeamPlayers"] = awayTeam.Players.Select(p => new PlayerViewModel
            {
                Id = p.Id,
                Name = p.FullName
            })
            .ToList();

            var model = new AddMatchInfoInputModel();

            return this.View(model);
        }

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

            // this.refereesService.AddResultToMatch(match.Id, model.HomeTeamGoals, model.AwayTeamGoals);

            var homeTeam = match.HomeTeam;
            var awayTeam = match.AwayTeam;

            this.ViewData["HomeTeamPlayers"] = homeTeam.Players.Select(p => new PlayerViewModel
            {
                Id = p.Id,
                Name = p.FullName
            })
          .ToList();

            this.ViewData["AwayTeamPlayers"] = awayTeam.Players.Select(p => new PlayerViewModel
            {
                Id = p.Id,
                Name = p.FullName
            })
            .ToList();

            //var players = new List<Player>();
            //players.AddRange(hometeam.Players);
            //players.AddRange(awayteam.Players);

            //foreach (var player in players)
            //{
            //    foreach (var stat in model.Stats)
            //    {
            //        // this.PlayersStatisticsService.CreatePlayerStatistics(match.Id, player.Id, stat.GoalsScored, stat.Assists);
            //    }
            //}

            var routeValues = new { match.Id };

            return this.RedirectToAction(controllerName: GlobalConstants.MatchesControllerName,
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

            var routeValues = new { Id = id };

            return this.RedirectToAction(controllerName: GlobalConstants.MatchesControllerName,
                                         actionName: GlobalConstants.DetailsActionName,
                                         routeValues: routeValues);
        }
    }
}