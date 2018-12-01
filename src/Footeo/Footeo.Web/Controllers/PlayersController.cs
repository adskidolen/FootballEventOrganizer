namespace Footeo.Web.Controllers
{
    using System.Linq;

    using Footeo.Services.Contracts;
    using Footeo.Web.ViewModels.Players;

    using Microsoft.AspNetCore.Mvc;

    public class PlayersController : Controller
    {
        private readonly ITeamsService teamsService;

        public PlayersController(ITeamsService teamsService)
        {
            this.teamsService = teamsService;
        }

        public IActionResult All(int id)
        {
            var players = this.teamsService.Players(id)
                                           .Select(p => new PlayerViewModel
                                           {
                                               Name = p.Nickname,
                                               TeamName = p.Team.Name
                                           })
                                           .ToList();

            var playersViewModel = new AllPlayersViewModel
            {
                Players = players
            };

            return View(playersViewModel);
        }
    }
}