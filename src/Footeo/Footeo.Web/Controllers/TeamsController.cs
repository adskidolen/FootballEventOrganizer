namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Output;
    using Footeo.Web.ViewModels.Players.Output;
    using Footeo.Common;
    using Footeo.Web.ViewModels;

    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : BaseController
    {
        private readonly ITeamsService teamsService;
        private readonly IPlayersService playersService;

        public TeamsController(ITeamsService teamsService, IPlayersService playersService)
        {
            this.teamsService = teamsService;
            this.playersService = playersService;
        }

        public IActionResult All()
        {
            var teams = this.teamsService.AllTeams<TeamViewModel>().ToList();

            var teamViewModels = new AllTeamsViewModel
            {
                Teams = teams
            };

            return View(teamViewModels);
        }

        public IActionResult Details(int id)
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
    }
}