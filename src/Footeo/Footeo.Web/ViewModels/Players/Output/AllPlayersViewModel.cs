namespace Footeo.Web.ViewModels.Players.Output
{
    using System.Collections.Generic;

    public class AllPlayersViewModel
    {
        public IEnumerable<PlayerViewModel> Players { get; set; }
    }
}