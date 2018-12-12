namespace Footeo.Web.Areas.Admin.Controllers.Base
{
    using Footeo.Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.AdminRoleName)]
    [Authorize(Roles = GlobalConstants.AdminRoleName)]
    public class AdminBaseController : Controller
    {
        protected AdminBaseController() { }
    }
}