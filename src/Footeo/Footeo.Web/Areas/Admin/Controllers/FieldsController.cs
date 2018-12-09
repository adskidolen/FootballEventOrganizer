namespace Footeo.Web.Areas.Admin.Controllers
{
    using Footeo.Common;
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.ViewModels;
    using Footeo.Web.ViewModels.Fields.Input;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.AdminRoleName)]
    public class FieldsController : BaseController
    {
        private readonly IFieldsService fieldsService;

        public FieldsController(IFieldsService fieldsService)
        {
            this.fieldsService = fieldsService;
        }

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Add() => this.View();

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        [HttpPost]
        public IActionResult Add(FieldCreateInputModel model)
        {
            var fieldExists = this.fieldsService.FieldExistsByName(model.Name);
            if (!fieldExists)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = string.Format(ErrorMessages.FieldExistsErrorMessage, model.Name)
                };

                return this.View(GlobalConstants.ErrorViewName, errorViewModel);
            }

            if (ModelState.IsValid)
            {
                this.fieldsService.CreateField(model.Name, model.Address, model.IsIndoors, model.Town);

                return this.RedirectToAction(actionName: GlobalConstants.AllActionName,
                                             controllerName: GlobalConstants.FieldsControllerName,
                                             routeValues: new { Area = GlobalConstants.EmptyArea });
            }

            return this.View(model);
        }
    }
}