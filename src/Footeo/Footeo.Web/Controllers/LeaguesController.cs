namespace Footeo.Web.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.Output;
    using Footeo.Web.ViewModels.TeamLeagues.Output;
    using Footeo.Web.ViewModels;
    using Footeo.Common;

    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    public class LeaguesController : BaseController
    {
        private readonly ILeaguesService leaguesService;
        private readonly ITeamLeaguesService teamLeaguesService;

        public LeaguesController(ILeaguesService leaguesService, ITeamLeaguesService teamLeaguesService)
        {
            this.leaguesService = leaguesService;
            this.teamLeaguesService = teamLeaguesService;
        }

        public IActionResult Pending()
        {
            var leagues = this.leaguesService.AllPendingLeagues<PendingLeagueViewModel>().ToList();

            var leagueViewModels = new AllPendingLeaguesViewModel
            {
                Leagues = leagues
            };

            return View(leagueViewModels);
        }

        public IActionResult InProgress()
        {
            var leagues = this.leaguesService.AllInProgressLeagues<InProgressLeagueViewModel>().ToList();

            var leagueViewModels = new AllInProgressLeaguesViewModel
            {
                Leagues = leagues
            };

            return View(leagueViewModels);
        }

        public IActionResult Completed()
        {
            var leagues = this.leaguesService.AllCompletedLeagues<CompletedLeagueViewModel>().ToList();

            var leagueViewModels = new AllCompletedLeaguesViewModel
            {
                Leagues = leagues
            };

            return View(leagueViewModels);
        }

        public IActionResult Table(int id)
        {
            var leagueExists = this.leaguesService.LeagueExistsById(id);
            if (!leagueExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.LeagueDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var teams = this.teamLeaguesService.LeagueTable<TeamLeagueViewModel>(id)
                                               .OrderByDescending(p => p.Points)
                                               .ThenByDescending(gd => gd.GoalDifference)
                                               .ThenByDescending(gf => gf.GoalsFor)
                                               .ToList();

            var leagueTableViewModel = new LeagueTableViewModel
            {
                Teams = teams
            };

            return this.View(leagueTableViewModel);
        }
    }
}