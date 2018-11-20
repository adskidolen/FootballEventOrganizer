namespace Footeo.Web.Controllers
{
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Players.View;

    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        public IActionResult Profile()
        {
            var playerViewModel = new PlayerViewModel();

            return View();
        }
    }
}