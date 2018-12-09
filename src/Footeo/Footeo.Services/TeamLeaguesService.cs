namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System.Linq;

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

        public void JoinLeague(string userName, int leagueId)
        {
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == userName);

            var teamId = user.Player.Team.Id;
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

        public IQueryable<TModel> LeagueTable<TModel>(int leagueId)
            => this.dbContext.TeamsLeagues.Where(l => l.LeagueId == leagueId).AsQueryable().ProjectTo<TModel>();
    }
}