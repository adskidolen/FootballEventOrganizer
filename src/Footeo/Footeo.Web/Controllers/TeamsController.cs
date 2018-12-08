namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Input;
    using Footeo.Web.ViewModels.Teams.View;
    using Footeo.Web.ViewModels.Players;
    using Footeo.Common;

    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Footeo.Web.ViewModels;

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
            var currentUser = this.User.Identity.Name;

            this.usersService.JoinTeam(id, currentUser);

            return this.RedirectToAction(nameof(Details), new { Id = id });
        }

        public IActionResult Details(int id)
        {
            var players = this.usersService.PlayersByTeam<PlayerViewModel>(id).ToList();

            var teamDetailsViewModel = new TeamDetailsViewModel
            {
                Players = players
            };

            return this.View(teamDetailsViewModel);
        }
    }
}