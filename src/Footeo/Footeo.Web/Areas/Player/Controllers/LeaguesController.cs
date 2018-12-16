namespace Footeo.Web.Areas.Player.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Player.Controllers.Base;
    using Footeo.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class LeaguesController : CaptainBaseController
    {
        private readonly ILeaguesService leaguesService;
        private readonly ITeamLeaguesService teamLeaguesService;
        private readonly ITeamsService teamsService;

        public LeaguesController(ILeaguesService leaguesService, ITeamLeaguesService teamLeaguesService, ITeamsService teamsService)
        {
            this.leaguesService = leaguesService;
            this.teamLeaguesService = teamLeaguesService;
            this.teamsService = teamsService;
        }

        public IActionResult Join(int id)
        {
            var leagueExists = this.leaguesService.LeagueExistsById(id);
            if (!leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.LeagueDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var user = this.User.Identity.Name;

            var team = this.teamsService.GetUsersTeam(user);

            var isTeamInLeague = this.teamsService.IsTeamInLeague(team.Id);
            if (isTeamInLeague)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.TeamJoinedLeagueErrorMessage, team.Name)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.teamLeaguesService.JoinLeague(user, id);

            var routeValues = new { Id = id, Area = GlobalConstants.EmptyArea };

            return this.RedirectToAction(controllerName: GlobalConstants.LeaguesControllerName,
                                         actionName: GlobalConstants.TableActionName,
                                         routeValues: routeValues);
        }
    }
}