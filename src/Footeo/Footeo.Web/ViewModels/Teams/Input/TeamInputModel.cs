namespace Footeo.Web.ViewModels.Teams.Input
{
    public class TeamInputModel
    {
        // TODO: Validation
        public string Name { get; set; }
        public string Initials { get; set; }
        public byte[] Logo { get; set; }
        public int TownId { get; set; }
    }
}