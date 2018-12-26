namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Linq;

    public interface IRefereesService
    {
        void CreateReferee(FooteoUser user, Referee referee);
        void AttendAMatch(string userName, int matchId);
        IQueryable<TModel> Referees<TModel>();
        void AddResultToMatch(int matchId, int homeTeamGoals, int awayTeamGoals);
    }
}