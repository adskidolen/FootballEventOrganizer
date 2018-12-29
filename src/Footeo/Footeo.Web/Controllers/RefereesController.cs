namespace Footeo.Web.Controllers
{
    using System.Linq;

    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Referees.Output;

    using Microsoft.AspNetCore.Mvc;

    using X.PagedList;

    public class RefereesController : BaseController
    {
        private readonly IRefereesService refereesService;

        public RefereesController(IRefereesService refereesService)
        {
            this.refereesService = refereesService;
        }

        public IActionResult All(int? pageNumber)
        {
            var nextPage = pageNumber ?? GlobalConstants.NextPageValue;

            var referees = this.refereesService.Referees<RefereeViewModel>().ToList();

            var pagedReferees = referees.ToPagedList(nextPage, GlobalConstants.MaxElementsOnPage);

            var refereesViewModel = new AllRefereesViewModel
            {
                Referees = pagedReferees
            };

            return View(refereesViewModel);
        }
    }
}