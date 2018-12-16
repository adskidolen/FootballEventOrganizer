namespace Footeo.Web.Areas.Player.Controllers.Base
{
    using Footeo.Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.PlayerRoleName)]
    [Authorize(Roles = GlobalConstants.CaptainRoleName)]
    public abstract class CaptainBaseController : Controller
    {
        protected CaptainBaseController() { }
    }
}