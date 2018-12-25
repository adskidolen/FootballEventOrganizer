﻿namespace Footeo.Web.Areas.Admin.ViewModels.Fields.Input
{
    using System.ComponentModel.DataAnnotations;

    public class FieldCreateInputModel
    {
        private const int TownMaxLength = 30;
        private const int TownMinLength = 3;

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public bool IsIndoors { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength)]
        public string Town { get; set; }
    }
}