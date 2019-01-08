namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System.Linq;

    public class RefereesService : IRefereesService
    {
        private readonly FooteoDbContext dbContext;
        private readonly IMatchesService matchesService;
        private readonly ITeamLeaguesService teamLeaguesService;

        public RefereesService(FooteoDbContext dbContext, IMatchesService matchesService, ITeamLeaguesService teamLeaguesService)
        {
            this.dbContext = dbContext;
            this.matchesService = matchesService;
            this.teamLeaguesService = teamLeaguesService;
        }

        public void CreateReferee(FooteoUser user, Referee referee)
        {
            user.Referee = referee;
            this.dbContext.SaveChanges();
        }

        public void AttendAMatch(string userName, int matchId)
        {
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            var match = this.dbContext.Matches.FirstOrDefault(m => m.Id == matchId);

            match.Referee = user.Referee;

            this.dbContext.SaveChanges();
        }

        public void AddResultToMatch(int matchId, int homeTeamGoals, int awayTeamGoals)
        {
            var match = this.dbContext.Matches.FirstOrDefault(m => m.Id == matchId);
            match.HomeTeamGoals = homeTeamGoals;
            match.AwayTeamGoals = awayTeamGoals;

            var homeTeam = match.HomeTeam;
            var awayTeam = match.AwayTeam;

            var leagueId = match.Fixture.LeagueId;

            var homeTeamLeague = this.teamLeaguesService.GetTeamLeague(homeTeam.Id, leagueId);
            var awayTeamLeague = this.teamLeaguesService.GetTeamLeague(awayTeam.Id, leagueId);

            this.AddTeamStats(homeTeamGoals, awayTeamGoals, homeTeamLeague, awayTeamLeague);

            var result = $"{match.HomeTeam.Name} {match.HomeTeamGoals} : {match.AwayTeamGoals} {match.AwayTeam.Name}";
            match.Result = result;

            this.dbContext.SaveChanges();
        }

        private void AddTeamStats(int homeTeamGoals, int awayTeamGoals, TeamLeague homeTeamLeague, TeamLeague awayTeamLeague)
        {
            if (homeTeamGoals > awayTeamGoals)
            {
                this.HomeWin(homeTeamGoals, awayTeamGoals, homeTeamLeague, awayTeamLeague);
            }
            if (awayTeamGoals > homeTeamGoals)
            {
                this.AwayWin(homeTeamGoals, awayTeamGoals, homeTeamLeague, awayTeamLeague);
            }
            if (homeTeamGoals == awayTeamGoals)
            {
                Draw(homeTeamGoals, awayTeamGoals, homeTeamLeague, awayTeamLeague);
            }
        }

        private void Draw(int homeTeamGoals, int awayTeamGoals, TeamLeague homeTeamLeague, TeamLeague awayTeamLeague)
        {
            homeTeamLeague.PlayedMatches++;
            homeTeamLeague.Drawn++;
            homeTeamLeague.Points += 1;
            homeTeamLeague.GoalsFor += homeTeamGoals;
            homeTeamLeague.GoalsAgainst += awayTeamGoals;
            homeTeamLeague.GoalDifference += homeTeamLeague.GoalsFor - homeTeamLeague.GoalsAgainst;
            this.dbContext.SaveChanges();

            awayTeamLeague.PlayedMatches++;
            awayTeamLeague.Drawn++;
            awayTeamLeague.Points += 1;
            awayTeamLeague.GoalsFor += homeTeamGoals;
            awayTeamLeague.GoalsAgainst += awayTeamGoals;
            awayTeamLeague.GoalDifference += awayTeamLeague.GoalsFor - awayTeamLeague.GoalsAgainst;
            this.dbContext.SaveChanges();
        }

        private void AwayWin(int homeTeamGoals, int awayTeamGoals, TeamLeague homeTeamLeague, TeamLeague awayTeamLeague)
        {
            awayTeamLeague.PlayedMatches++;
            awayTeamLeague.Won++;
            awayTeamLeague.Points += 3;
            awayTeamLeague.GoalsFor += awayTeamGoals;
            awayTeamLeague.GoalsAgainst += homeTeamGoals;
            awayTeamLeague.GoalDifference += awayTeamLeague.GoalsFor - awayTeamLeague.GoalsAgainst;
            this.dbContext.SaveChanges();

            homeTeamLeague.PlayedMatches++;
            homeTeamLeague.Lost++;
            homeTeamLeague.GoalsFor += homeTeamGoals;
            homeTeamLeague.GoalsAgainst += awayTeamGoals;
            homeTeamLeague.GoalDifference += homeTeamLeague.GoalsFor - homeTeamLeague.GoalsAgainst;
            this.dbContext.SaveChanges();
        }

        private void HomeWin(int homeTeamGoals, int awayTeamGoals, TeamLeague homeTeamLeague, TeamLeague awayTeamLeague)
        {
            homeTeamLeague.PlayedMatches++;
            homeTeamLeague.Won++;
            homeTeamLeague.Points += 3;
            homeTeamLeague.GoalsFor += homeTeamGoals;
            homeTeamLeague.GoalsAgainst += awayTeamGoals;
            homeTeamLeague.GoalDifference += homeTeamLeague.GoalsFor - homeTeamLeague.GoalsAgainst;
            this.dbContext.SaveChanges();

            awayTeamLeague.PlayedMatches++;
            awayTeamLeague.Lost++;
            awayTeamLeague.GoalsFor += awayTeamGoals;
            awayTeamLeague.GoalsAgainst += homeTeamGoals;
            awayTeamLeague.GoalDifference += awayTeamLeague.GoalsFor - awayTeamLeague.GoalsAgainst;
            this.dbContext.SaveChanges();
        }

        public IQueryable<TModel> Referees<TModel>()
           => this.dbContext.Referees.AsQueryable().ProjectTo<TModel>();
    }
}