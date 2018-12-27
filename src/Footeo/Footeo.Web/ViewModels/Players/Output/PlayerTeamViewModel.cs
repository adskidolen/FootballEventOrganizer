namespace Footeo.Web.ViewModels.Players.Output
{
    public class PlayerTeamViewModel
    {
        private const string CaptainSign = "(C)";

        public int Id { get; set; }

        public string FullName { get; set; }

        public bool IsCaptain { get; set; }

        public string ShowPlayerName => this.IsCaptain ? $"{this.FullName} {CaptainSign}" : this.FullName;

    }
}