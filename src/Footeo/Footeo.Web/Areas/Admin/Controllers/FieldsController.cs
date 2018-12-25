namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Areas.Admin.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.Areas.Admin.ViewModels.Fields.Input;

    using Microsoft.AspNetCore.Mvc;

    public class FieldsController : AdminBaseController
    {
        private readonly IFieldsService fieldsService;

        public FieldsController(IFieldsService fieldsService)
        {
            this.fieldsService = fieldsService;
        }

        public IActionResult Add() => this.View();

        [HttpPost]
        public IActionResult Add(FieldCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var fieldExists = this.fieldsService.FieldExistsByName(model.Name);
            if (fieldExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.FieldExistsErrorMessage, model.Name)
                };

                return this.View(viewName: GlobalConstants.ErrorViewName, model: errorViewModel);
            }

            this.fieldsService.CreateField(model.Name.Trim(), model.Address.Trim(), model.IsIndoors, model.Town.Trim());

            var routeValues = new { Area = GlobalConstants.EmptyArea };

            return this.RedirectToAction(actionName: GlobalConstants.AllActionName,
                                         controllerName: GlobalConstants.FieldsControllerName,
                                         routeValues: routeValues);
        }
    }
}