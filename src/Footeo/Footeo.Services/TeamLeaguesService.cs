namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Common;
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Models.Enums;
    using Footeo.Services.Contracts;

    using System;
    using System.Collections.Generic;
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

        public TeamLeague GetTeamLeague(int teamId)
            => this.dbContext.TeamsLeagues.FirstOrDefault(t => t.TeamId == teamId);

        public TeamLeague GetTeamLeagueWinner(int leagueId)
            => this.LeagueTable<TeamLeague>(leagueId)
                   .Where(l => l.League.Status == LeagueStatus.Completed)
                   .OrderByDescending(p => p.Points)
                   .ThenByDescending(gd => gd.GoalDifference)
                   .ThenByDescending(gf => gf.GoalsFor)
                   .ToList()
                   .First();

        public bool IsTeamInLeague(int leagueId, string userName)
        {
            var league = this.leaguesService.GetLeagueById<League>(leagueId);
            var team = this.teamsService.GetUsersTeam(userName);

            var teamInCurrentLeague = league.Teams.Any(t => t.TeamId == team.Id);
            if (!teamInCurrentLeague)
            {
                return false;
            }

            return true;
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

            if (this.TeamsCount(league.Id) == GlobalConstants.MaxTeamsInLeagueCount)
            {
                league.Status = LeagueStatus.InProgress;
            }

            this.dbContext.TeamsLeagues.Add(teamLeague);
            this.dbContext.SaveChanges();
        }

        public IQueryable<TModel> LeagueTable<TModel>(int leagueId)
            => this.By<TModel>(l => l.LeagueId == leagueId).AsQueryable().ProjectTo<TModel>();

        public int TeamsCount(int leagueId)
            => this.dbContext.TeamsLeagues.Where(l => l.LeagueId == leagueId).Count();

        private IEnumerable<TModel> By<TModel>(Func<TeamLeague, bool> predicate)
           => this.dbContext.TeamsLeagues.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}