namespace Footeo.Web.ViewModels.Trophies.Output
{
    using System.Collections.Generic;

    public class AllTrophiesViewModel
    {
        public IEnumerable<TrophyViewModel> Trophies { get; set; }
    }
}