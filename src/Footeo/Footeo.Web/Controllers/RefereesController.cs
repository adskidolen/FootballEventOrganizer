namespace Footeo.Web.Controllers
{
    using System.Linq;

    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Referees.Output;

    using Microsoft.AspNetCore.Mvc;

    public class RefereesController : BaseController
    {
        private readonly IRefereesService refereesService;

        public RefereesController(IRefereesService refereesService)
        {
            this.refereesService = refereesService;
        }

        public IActionResult All()
        {
            var referees = this.refereesService.Referees<RefereeViewModel>().ToList();

            var refereesViewModel = new AllRefereesViewModel
            {
                Referees = referees
            };

            return View(refereesViewModel);
        }
    }
}