namespace Footeo.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.Output;
    using Footeo.Web.ViewModels.Home;

    using System.Linq;

    public class HomeController : BaseController
    {
        private readonly ILeaguesService leaguesService;

        public HomeController(ILeaguesService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        public IActionResult Index()
        {
            var leagues = this.leaguesService.AllPendingLeagues<LeagueIndexViewModel>()
                                             .OrderBy(d => d.StartDate)
                                             .ToList()
                                             .Take(GlobalConstants.LeaguesIndexCount)
                                             .ToList();

            var indexViewModel = new IndexViewModel
            {
                Leagues = leagues
            };

            return View(indexViewModel);
        }
    }
}