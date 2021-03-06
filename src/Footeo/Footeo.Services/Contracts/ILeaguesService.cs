﻿namespace Footeo.Services.Contracts
{
    using System;
    using System.Linq;

    public interface ILeaguesService
    {
        void CreateLeague(string name, string description, DateTime startDate, DateTime endDate, string town);
        bool LeagueExistsById(int id);
        bool LeagueExistsByName(string name);
        TModel GetLeagueById<TModel>(int id);
        IQueryable<TModel> AllPendingLeagues<TModel>();
        IQueryable<TModel> AllInProgressLeagues<TModel>();
        IQueryable<TModel> AllCompletedLeagues<TModel>();
        void SetLeagueStatusToInProgress(int id);
        void SetLeagueStatusToCompleted(int id);
    }
}