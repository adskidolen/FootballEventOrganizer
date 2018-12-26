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

        public void AddResultToMatch(int matchId, int homeTeamGoals, int awayTeamGoals)
        {
            var match = this.dbContext.Matches.FirstOrDefault(m => m.Id == matchId);
            match.HomeTeamGoals = homeTeamGoals;
            match.AwayTeamGoals = awayTeamGoals;

            var homeTeam = match.HomeTeam;
            var awayTeam = match.AwayTeam;

            var homeTeamLeague = this.teamLeaguesService.GetTeamLeague(homeTeam.Id);
            var awayTeamLeague = this.teamLeaguesService.GetTeamLeague(awayTeam.Id);

            this.AddTeamStats(homeTeamGoals, awayTeamGoals, homeTeamLeague, awayTeamLeague);

            var result = $"{match.HomeTeam.Name} {match.HomeTeamGoals} : {match.AwayTeamGoals} {match.AwayTeam.Name}";
            match.Result = result;

            this.dbContext.SaveChanges();
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

        public IQueryable<TModel> Referees<TModel>()
           => this.dbContext.Referees.AsQueryable().ProjectTo<TModel>();

        private void AddTeamStats(int homeTeamGoals, int awayTeamGoals, TeamLeague homeTeamLeague, TeamLeague awayTeamLeague)
        {
            if (homeTeamGoals > awayTeamGoals)
            {
                this.TeamWin(homeTeamGoals, awayTeamGoals, homeTeamLeague);
                this.TeamLose(homeTeamGoals, awayTeamGoals, awayTeamLeague);
            }
            else if (awayTeamGoals > homeTeamGoals)
            {
                this.TeamWin(homeTeamGoals, awayTeamGoals, awayTeamLeague);
                this.TeamLose(homeTeamGoals, awayTeamGoals, homeTeamLeague);
            }
            else
            {
                this.TeamDraw(homeTeamGoals, awayTeamGoals, homeTeamLeague);
                this.TeamDraw(homeTeamGoals, awayTeamGoals, awayTeamLeague);
            }
        }

        private void TeamWin(int homeTeamGoals, int awayTeamGoals, TeamLeague team)
        {
            team.PlayedMatches++;
            team.Won++;
            team.Points += 3;
            team.GoalsFor += homeTeamGoals;
            team.GoalsAgainst += awayTeamGoals;
            team.GoalDifference += team.GoalsFor - team.GoalsAgainst;
        }

        private void TeamDraw(int homeTeamGoals, int awayTeamGoals, TeamLeague team)
        {
            team.PlayedMatches++;
            team.Drawn++;
            team.Points += 1;
            team.GoalsFor += homeTeamGoals;
            team.GoalsAgainst += awayTeamGoals;
            team.GoalDifference += team.GoalsFor - team.GoalsAgainst;
        }

        private void TeamLose(int homeTeamGoals, int awayTeamGoals, TeamLeague team)
        {
            team.PlayedMatches++;
            team.Lost++;
            team.GoalsFor += awayTeamGoals;
            team.GoalsAgainst += homeTeamGoals;
            team.GoalDifference += team.GoalsFor - team.GoalsAgainst;
        }
    }
}