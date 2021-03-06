﻿namespace Footeo.Web.Areas.Admin.ViewModels.Fields.Input
{
    using System.ComponentModel.DataAnnotations;

    public class FieldCreateInputModel
    {
        private const string RegexForTownName = @"[A-Z]{1}[a-z]+";

        private const int TownMaxLength = 30;
        private const int TownMinLength = 3;

        private const string IsIndoorsDisplayName = "Indoor";

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = IsIndoorsDisplayName)]
        public bool IsIndoors { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength)]
        [RegularExpression(RegexForTownName)]
        public string Town { get; set; }
    }
}