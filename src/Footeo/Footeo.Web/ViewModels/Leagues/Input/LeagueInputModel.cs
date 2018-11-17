namespace Footeo.Web.ViewModels.Leagues.Input
{
    public class LeagueInputModel
    {
        // TODO: Validation
        public string Name { get; set; }
        public string Description { get; set; }
        public string News { get; set; }
        public int TownId { get; set; }
    }
}