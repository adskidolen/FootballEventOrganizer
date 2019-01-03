namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    public class TrophiesService : ITrophiesService
    {
        private readonly FooteoDbContext dbContext;
        private readonly ITeamsService teamsService;
        private readonly ITeamLeaguesService teamLeaguesService;

        public TrophiesService(FooteoDbContext dbContext, ITeamsService teamsService, ITeamLeaguesService teamLeaguesService)
        {
            this.dbContext = dbContext;
            this.teamsService = teamsService;
            this.teamLeaguesService = teamLeaguesService;
        }

        public void CreateTrophy(int leagueId)
        {
            var teamLeague = this.teamLeaguesService.GetTeamLeagueWinner(leagueId);
            var team = teamLeague.Team;
            var leagueName = teamLeague.League.Name;
            
            var trophy = new Trophy
            {
                Name = leagueName,
                Team = team
            };

            team.Trophies.Add(trophy);

            this.dbContext.Trophies.Add(trophy);
            this.dbContext.SaveChanges();
        }
    }
}