namespace Footeo.Web.Areas.Referee.Controllers.Base
{
    using Footeo.Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.RefereeRoleName)]
    [Authorize(Roles = GlobalConstants.RefereeRoleName)]
    public abstract class RefereeBaseController : Controller
    {
        protected RefereeBaseController() { }
    }
}