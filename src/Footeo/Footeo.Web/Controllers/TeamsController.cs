namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Input;
    using Footeo.Web.ViewModels.Teams.View;
    using Footeo.Web.ViewModels.Players;
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
            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.usersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.PlayerInTeamErrorMessage, currentUser)
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            var teamExists = this.teamsService.TeamExistsByName(model.Name);
            if (teamExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.TeamExistsErrorMessage, model.Name)
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            if (ModelState.IsValid)
            {
                this.teamsService.CreateTeam(model.Name, model.Initials, model.Town, currentUser);

                return this.RedirectToAction(nameof(All));
            }

            return this.View(model);
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

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            var playersCount = this.teamsService.PlayersCount(id);
            if (playersCount == GlobalConstants.MaxPlayersInTeamCount)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.TeamIsFullErrorMessage, id)
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.usersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.PlayerInTeamErrorMessage, currentUser)
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            this.usersService.JoinTeam(id, currentUser);

            return this.RedirectToAction(nameof(Details), new { Id = id });
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

                return this.View(GlobalConstants.ErrorViewName, errorModel);
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