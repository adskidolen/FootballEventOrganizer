namespace Footeo.Web.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.ViewModels.Players.Output;

    using Microsoft.AspNetCore.Mvc;

    public class PlayersController : BaseController
    {
        private readonly IPlayersService playersService;

        public PlayersController(IPlayersService playersService)
        {
            this.playersService = playersService;
        }

        public IActionResult Details(int id)
        {
            var playerExists = this.playersService.PlayerExistsById(id);
            if (!playerExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ErrorMessages.PlayerDoesNotExistsErrorMessage
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            var player = this.playersService.GetPlayerById<PlayerDetailsViewModel>(id);

            return View(player);
        }
    }
}