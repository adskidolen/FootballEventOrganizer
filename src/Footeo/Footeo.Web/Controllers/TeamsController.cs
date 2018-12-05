namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Teams.Input;
    using Footeo.Web.ViewModels.Teams.View;
    using Footeo.Web.ViewModels;
    using Footeo.Common;
    
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

            var hasAteam = this.usersService.HasATeam(currentUser);
            if (hasAteam)
            {
                // TODO: check if player has a team

                return this.View("Error", new ErrorViewModel { RequestId = model.Name + " User has a team" });
            }

            if (this.teamsService.ExistsByName(model.Name))
            {
                // TODO: check if team exists before creating it

                return this.View("Error", new ErrorViewModel { RequestId = model.Name + " Team already exists" });
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
            var teams = this.teamsService
                             .All()
                             .Select(vm => new TeamViewModel
                             {
                                 Id = vm.Id,
                                 Name = vm.Name,
                                 Initials = vm.Initials,
                                 CreatedOn = vm.CreatedOn,
                                 Town = vm.Town.Name
                             })
                             .ToList();

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

            var hasAteam = this.usersService.HasATeam(currentUser);
            if (hasAteam)
            {
                // TODO: check if player has a team

                return this.View("Error", new ErrorViewModel { RequestId = id + " User has a team" });
            }

            this.teamsService.JoinTeam(id, currentUser);

            return this.RedirectToAction(controllerName: "Players", actionName: "All", routeValues: id);
        }
    }
}