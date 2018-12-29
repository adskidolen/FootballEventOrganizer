namespace Footeo.Web.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels.Fields.Output;

    using Microsoft.AspNetCore.Mvc;

    using System.Linq;

    using X.PagedList;

    public class FieldsController : BaseController
    {
        private readonly IFieldsService fieldsService;

        public FieldsController(IFieldsService fieldsService)
        {
            this.fieldsService = fieldsService;
        }

        public IActionResult All(int? pageNumber)
        {
            var nextPage = pageNumber ?? GlobalConstants.NextPageValue;

            var fields = this.fieldsService.AllFields<FieldViewModel>().ToList();

            var pagedFields = fields.ToPagedList(nextPage, GlobalConstants.MaxElementsOnPage);

            var fieldViewModels = new AllFieldsViewModel
            {
                Fields = pagedFields
            };

            return View(fieldViewModels);
        }
    }
}