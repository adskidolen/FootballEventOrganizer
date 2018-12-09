namespace Footeo.Web.ViewModels.Players
{
    public class PlayerViewModel
    {
        public string Nickname { get; set; }
        public string Captain => this.IsCaptain ? "Captain" : "Player";
        public bool IsCaptain { get; set; }
    }
}