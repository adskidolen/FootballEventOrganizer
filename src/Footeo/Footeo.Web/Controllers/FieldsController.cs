namespace Footeo.Web.Controllers
{
    using Footeo.Services.Contracts;
    using Footeo.Web.Controllers.Base;
    using Footeo.Web.Utilities;
    using Footeo.Web.ViewModels.Fields.Input;
    using Footeo.Web.ViewModels.Fields.View;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Linq;

    public class FieldsController : BaseController
    {
        private readonly IFieldsService fieldsService;

        public FieldsController(IFieldsService fieldsService)
        {
            this.fieldsService = fieldsService;
        }

        [Authorize(Roles = Constants.AdminRoleName)]
        public IActionResult Add() => this.View();

        [Authorize(Roles = Constants.AdminRoleName)]
        [HttpPost]
        public IActionResult Add(FieldCreateInputModel model)
        {
            if (this.fieldsService.ExistsByName(model.Name))
            {
                // TODO: Error for existing field
            }

            if (ModelState.IsValid)
            {
                this.fieldsService.CreateField(model.Name, model.Address, model.IsIndoors, model.Town);

                return this.RedirectToAction(nameof(All));
            }

            return this.View(model);
        }

        public IActionResult All()
        {
            var fields = this.fieldsService
                              .All()
                              .Select(vm => new FieldViewModel
                              {
                                  Name = vm.Name,
                                  Address = vm.Address,
                                  IsIndoors = vm.IsIndoors,
                                  Town = vm.Town.Name
                              })
                              .ToList();

            var fieldViewModels = new AllFieldsViewModel
            {
                Fields = fields
            };

            return View(fieldViewModels);
        }
    }
}