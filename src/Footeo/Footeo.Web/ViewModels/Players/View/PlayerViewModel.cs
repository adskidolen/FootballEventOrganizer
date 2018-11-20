namespace Footeo.Web.ViewModels.Players.View
{
    using Footeo.Models.Enums;

    public class PlayerViewModel
    {
        public int Id { get; set; }

        public string Nickname { get; set; }

        public string Team { get; set; }

        public Position Position { get; set; }

        public int SquadNumber { get; set; }

        public int Rating { get; set; }
    }
}
