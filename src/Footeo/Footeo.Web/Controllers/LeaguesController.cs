namespace Footeo.Web.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.View;
    using Footeo.Web.ViewModels.TeamLeagues.View;
    using Footeo.Web.ViewModels;
    using Footeo.Common;

    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    public class LeaguesController : BaseController
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

        public IActionResult All()
        {
            var leagues = this.leaguesService.AllLeagues<LeagueViewModel>().ToList();

            var leagueViewModels = new AllLeaguesViewModel
            {
                Leagues = leagues
            };

            return View(leagueViewModels);
        }

        [Authorize(Roles = GlobalConstants.CaptainRoleName)]
        public IActionResult Join(int id)
        {
            var leagueExists = this.leaguesService.LeagueExistsById(id);
            if (!leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.LeagueDoesNotExistsErrorMessage
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
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

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            this.teamLeaguesService.JoinLeague(user, id);

            return this.RedirectToAction(nameof(Table), routeValues: new { Id = id });
        }

        public IActionResult Table(int id)
        {
            var leagueExists = this.leaguesService.LeagueExistsById(id);
            if (!leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ErrorMessages.LeagueDoesNotExistsErrorMessage
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            var teams = this.teamLeaguesService.LeagueTable<TeamLeagueViewModel>(id).ToList();

            var leagueTableViewModel = new LeagueTableViewModel
            {
                Teams = teams
            };

            return this.View(leagueTableViewModel);
        }
    }
}