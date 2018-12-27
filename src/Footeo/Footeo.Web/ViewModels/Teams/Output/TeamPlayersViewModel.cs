namespace Footeo.Web.ViewModels.Teams.Output
{
    using Footeo.Web.ViewModels.Players.Output;

    using System.Collections.Generic;

    public class TeamPlayersViewModel
    {
        public IEnumerable<PlayerTeamViewModel> Players { get; set; }
    }
}