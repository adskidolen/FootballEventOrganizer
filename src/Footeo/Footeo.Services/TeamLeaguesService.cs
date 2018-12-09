namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    public class TeamLeaguesService : ITeamLeaguesService
    {
        private readonly FooteoDbContext dbContext;
        private readonly ITeamsService teamsService;
        private readonly ILeaguesService leaguesService;

        public TeamLeaguesService(FooteoDbContext dbContext, ITeamsService teamsService, ILeaguesService leaguesService)
        {
            this.dbContext = dbContext;
            this.teamsService = teamsService;
            this.leaguesService = leaguesService;
        }

        public void JoinLeague(int teamId, int leagueId)
        {
            var team = this.teamsService.GetTeamById<Team>(teamId);
            var league = this.leaguesService.GetLeagueById<League>(leagueId);

            var teamLeague = new TeamLeague
            {
                Team = team,
                League = league
            };

            this.dbContext.TeamsLeagues.Add(teamLeague);
            this.dbContext.SaveChanges();
        }
    }
}