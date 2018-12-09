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

        [Authorize(Roles = GlobalConstants.AdminAndPlayerRoleName)]
        public IActionResult Create() => this.View();

        [Authorize(Roles = GlobalConstants.AdminAndPlayerRoleName)]
        [HttpPost]
        public IActionResult Create(TeamCreateInputModel model)
        {
            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.usersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                return this.View("Error", new ErrorViewModel { RequestId = currentUser + " player already has a team!" });
            }

            var teamExists = this.teamsService.TeamExistsByName(model.Name);
            if (!teamExists)
            {
                return this.View("Error", new ErrorViewModel { RequestId = model.Name + " already exists!" });
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

        [Authorize(Roles = GlobalConstants.AdminAndPlayerRoleName)]
        public IActionResult Join(int id)
        {
            var teamExists = this.teamsService.TeamExistsById(id);
            if (!teamExists)
            {
                return this.View("Error", new ErrorViewModel { RequestId = id + " team does not exists!" });
            }

            var playersCount = this.teamsService.PlayersCount(id);
            if (playersCount == GlobalConstants.MaxPlayersInTeamCount)
            {
                return this.View("Error", new ErrorViewModel { RequestId = id + " team already full!" });
            }

            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.usersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                return this.View("Error", new ErrorViewModel { RequestId = id + " player already has a team!" });
            }

            this.usersService.JoinTeam(id, currentUser);

            return this.RedirectToAction(nameof(Details), new { Id = id });
        }

        public IActionResult Details(int id)
        {
            var teamExists = this.teamsService.TeamExistsById(id);
            if (!teamExists)
            {
                return this.View("Error", new ErrorViewModel { RequestId = id + " team does not exists" });
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