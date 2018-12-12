namespace Footeo.Web.ViewModels.Teams.Output
{
    using Footeo.Web.ViewModels.Players.Output;

    using System.Collections.Generic;

    public class TeamDetailsViewModel
    {
        public IEnumerable<PlayerViewModel> Players { get; set; }
    }
}