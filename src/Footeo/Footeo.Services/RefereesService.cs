namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using Microsoft.AspNetCore.Identity;

    using System.Linq;

    public class RefereesService : IRefereesService
    {
        private readonly FooteoDbContext dbContext;
        private readonly UserManager<FooteoUser> userManager;
        private readonly IMatchesService matchesService;

        public RefereesService(FooteoDbContext dbContext, UserManager<FooteoUser> userManager, IMatchesService matchesService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.matchesService = matchesService;
        }

        public void AddResultToMatch(int matchId, int homeTeamGoals, int awayTeamGoals)
        {
            var match = this.dbContext.Matches.FirstOrDefault(m => m.Id == matchId);
            match.HomeTeamGoals = homeTeamGoals;
            match.AwayTeamGoals = awayTeamGoals;

            var result = $"{match.HomeTeam.Name} {match.HomeTeamGoals} : {match.AwayTeamGoals} {match.AwayTeam.Name}";
            match.Result = result;

            this.dbContext.SaveChanges();
        }

        public void CreateReferee(FooteoUser user, Referee referee)
        {
            user.Referee = referee;
            this.dbContext.SaveChanges();
        }

        public void JoinMatch(string userName, int matchId)
        {
            var user = this.userManager.Users.FirstOrDefault(u => u.UserName == userName);
            var match = this.dbContext.Matches.FirstOrDefault(m => m.Id == matchId);

            match.Referee = user.Referee;

            this.dbContext.SaveChanges();
        }

        public IQueryable<TModel> Referees<TModel>()
           => this.dbContext.Referees.AsQueryable().ProjectTo<TModel>();
    }
}