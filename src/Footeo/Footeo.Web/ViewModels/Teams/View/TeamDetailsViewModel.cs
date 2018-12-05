namespace Footeo.Web.ViewModels.Teams.View
{
    using Footeo.Web.ViewModels.Players;

    using System.Collections.Generic;

    public class TeamDetailsViewModel
    {
        public IEnumerable<PlayerViewModel> Players { get; set; }
    }
}