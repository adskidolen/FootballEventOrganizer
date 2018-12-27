namespace Footeo.Web.ViewModels.Players.Output
{
    using Footeo.Common;

    public class PlayerDetailsViewModel
    {
        public string FullName { get; set; }
        public string Nickname { get; set; }

        public string Position { get; set; }
        public string ShowPosition => string.IsNullOrWhiteSpace(this.Position) ? GlobalConstants.None : this.Position;

        public int SquadNumber { get; set; }
        public string ShowSquadNumber => this.SquadNumber == 0 ? GlobalConstants.None : this.SquadNumber.ToString();
    }
}
