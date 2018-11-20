namespace Footeo.Models
{
    public class LeagueTrophy
    {
        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        public int TrophyId { get; set; }
        public virtual Trophy Trophy { get; set; }
    }
}