namespace Footeo.Web.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Leagues.View;

    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    public class LeaguesController : BaseController
    {
        private readonly ILeaguesService leaguesService;

        public LeaguesController(ILeaguesService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        public IActionResult All()
        {
            var leagues = this.leaguesService
                              .All()
                              .Select(vm => new LeagueViewModel
                              {
                                  Name = vm.Name,
                                  Description = vm.Description,
                                  StartDate = vm.StartDate,
                                  EndDate = vm.EndDate,
                                  Status = vm.Status,
                                  Town = vm.Town.Name
                              })
                              .ToList();

            var leagueViewModels = new AllLeaguesViewModel
            {
                Leagues = leagues
            };

            return View(leagueViewModels);
        }
    }
}