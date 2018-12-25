namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Output;
    using Footeo.Web.ViewModels.Players.Output;
    using Footeo.Common;
    using Footeo.Web.ViewModels;
    using Footeo.Web.ViewModels.Trophies.Output;
    using Footeo.Web.ViewModels.Matches.Output;

    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using AutoMapper;

    public class TeamsController : BaseController
    {
        private readonly ITeamsService teamsService;
        private readonly IPlayersService playersService;
        private readonly IMapper mapper;

        public TeamsController(ITeamsService teamsService, IPlayersService playersService, IMapper mapper)
        {
            this.teamsService = teamsService;
            this.playersService = playersService;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var teams = this.teamsService.AllTeams<TeamViewModel>()
                                         .OrderByDescending(t => t.TrophiesCount)
                                         .ThenBy(n => n.Name)
                                         .ToList();

            var teamViewModels = new AllTeamsViewModel
            {
                Teams = teams
            };

            return View(teamViewModels);
        }

        public IActionResult Players(int id)
        {
            var teamExists = this.teamsService.TeamExistsById(id);
            if (!teamExists)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.TeamDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorModel);
            }

            var players = this.playersService.PlayersByTeam<PlayerViewModel>(id).ToList();

            var teamDetailsViewModel = new TeamDetailsViewModel
            {
                Players = players
            };

            return this.View(teamDetailsViewModel);
        }

        public IActionResult Trophies(int id)
        {
            var teamExists = this.teamsService.TeamExistsById(id);
            if (!teamExists)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.TeamDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorModel);
            }

            var trophies = this.teamsService.AllTrophiesByTeamId<TrophyViewModel>(id).ToList();

            var allTrophiesViewModel = new AllTrophiesViewModel
            {
                Trophies = trophies
            };

            return this.View(allTrophiesViewModel);
        }

        public IActionResult Matches(int id)
        {
            var teamExists = this.teamsService.TeamExistsById(id);
            if (!teamExists)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.TeamDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorModel);
            }

            var homeMatches = this.teamsService.AllHomeMatchesByTeamId(id).ToList();
            var awayMatches = this.teamsService.AllAwayMatchesByTeamId(id).ToList();

            var mappedHomeMatches = this.mapper.ProjectTo<TeamMatchViewModel>(homeMatches.AsQueryable());
            var mappedAwayMatches = this.mapper.ProjectTo<TeamMatchViewModel>(awayMatches.AsQueryable());

            var allMatchesViewModel = new AllTeamMatchesViewModel
            {
                HomeMatches = mappedHomeMatches,
                AwayMatches = mappedAwayMatches
            };

            return this.View(allMatchesViewModel);
        }
    }
}