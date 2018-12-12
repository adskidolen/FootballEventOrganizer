namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Input;
    using Footeo.Web.ViewModels.Teams.Output;
    using Footeo.Web.ViewModels.Players.Output;
    using Footeo.Common;
    using Footeo.Web.ViewModels;

    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    public class TeamsController : BaseController
    {
        private readonly ITeamsService teamsService;
        private readonly IUsersService usersService;

        public TeamsController(ITeamsService teamsService, IUsersService usersService)
        {
            this.teamsService = teamsService;
            this.usersService = usersService;
        }

        [Authorize(Roles = GlobalConstants.PlayerRoleName)]
        public IActionResult Create() => this.View();

        [Authorize(Roles = GlobalConstants.PlayerRoleName)]
        [HttpPost]
        public IActionResult Create(TeamCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.usersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.PlayerInTeamErrorMessage, currentUser)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var teamExists = this.teamsService.TeamExistsByName(model.Name);
            if (teamExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.TeamExistsErrorMessage, model.Name)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.teamsService.CreateTeam(model.Name, model.Initials, currentUser);

            return this.RedirectToAction(nameof(All));
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

        [Authorize(Roles = GlobalConstants.PlayerRoleName)]
        public IActionResult Join(int id)
        {
            var teamExists = this.teamsService.TeamExistsById(id);
            if (!teamExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.TeamExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var playersCount = this.teamsService.PlayersCount(id);
            if (playersCount == GlobalConstants.MaxPlayersInTeamCount)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.TeamIsFullErrorMessage, id)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.usersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.PlayerInTeamErrorMessage, currentUser)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.usersService.JoinTeam(id, currentUser);

            var routeValues = new { Id = id };

            return this.RedirectToAction(actionName: nameof(Details), routeValues: routeValues);
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

            var players = this.usersService.PlayersByTeam<PlayerViewModel>(id).ToList();

            var teamDetailsViewModel = new TeamDetailsViewModel
            {
                Players = players
            };

            return this.View(teamDetailsViewModel);
        }
    }
}