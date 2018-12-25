namespace Footeo.Web.ViewModels.Players.Output
{
    using Footeo.Common;

    public class PlayerViewModel
    {
        public string FullName { get; set; }
        public string Nickname { get; set; }

        public string Position { get; set; }
        public string ShowPosition => string.IsNullOrWhiteSpace(this.Position) ? GlobalConstants.None : this.Position;

        public bool IsCaptain { get; set; }
        public string Captain => this.IsCaptain ? GlobalConstants.CaptainRoleName : GlobalConstants.PlayerRoleName;

        public int SquadNumber { get; set; }
        public string ShowSquadNumber => this.SquadNumber == 0 ? GlobalConstants.None : this.SquadNumber.ToString();

    }
}