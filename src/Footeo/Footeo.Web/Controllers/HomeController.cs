namespace Footeo.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Footeo.Web.Controllers.Base;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}