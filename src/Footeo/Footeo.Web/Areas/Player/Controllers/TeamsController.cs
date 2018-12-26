namespace Footeo.Web.Areas.Player.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Player.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.Areas.Player.ViewModels.Teams.Input;

    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : PlayerBaseController
    {
        private readonly ITeamsService teamsService;
        private readonly IPlayersService playersService;

        public TeamsController(ITeamsService teamsService, IPlayersService playersService)
        {
            this.teamsService = teamsService;
            this.playersService = playersService;
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(TeamCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.playersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = string.Format(ErrorMessages.PlayerInTeamErrorMessage, currentUser)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var teamExists = this.teamsService.TeamExistsByName(model.Name);
            if (teamExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = string.Format(ErrorMessages.TeamExistsErrorMessage, model.Name)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.teamsService.CreateTeam(model.Name.Trim(), model.Initials.Trim(), currentUser);

            var routeValues = new { Area = GlobalConstants.EmptyArea };

            return this.RedirectToAction(controllerName: GlobalConstants.TeamsControllerName,
                                         actionName: GlobalConstants.AllActionName,
                                         routeValues: routeValues);
        }

        public IActionResult Join(int id)
        {
            var teamExists = this.teamsService.TeamExistsById(id);
            if (!teamExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.TeamExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var playersCount = this.teamsService.PlayersCount(id);
            if (playersCount == GlobalConstants.MaxPlayersInTeamCount)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = string.Format(ErrorMessages.TeamIsFullErrorMessage, id)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var currentUser = this.User.Identity.Name;

            var playerHasATeam = this.playersService.PlayerHasATeam(currentUser);
            if (playerHasATeam)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = string.Format(ErrorMessages.PlayerInTeamErrorMessage, currentUser)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.playersService.JoinTeam(id, currentUser);

            var routeValues = new { Id = id, Area = GlobalConstants.EmptyArea };

            return this.RedirectToAction(controllerName: GlobalConstants.TeamsControllerName,
                                         actionName: GlobalConstants.PlayersActionName,
                                         routeValues: routeValues);
        }
    }
}