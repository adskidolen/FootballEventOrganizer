﻿namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TownsService : ITownsService
    {
        private readonly FooteoDbContext dbContext;

        public TownsService(FooteoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Town CreateTown(string name)
        {
            var town = new Town
            {
                Name = name
            };

            this.dbContext.Towns.Add(town);
            this.dbContext.SaveChanges();

            return town;
        }

        public bool TownExistsByName(string name)
            => this.dbContext.Towns.Any(t => t.Name == name);

        public bool TownExistsById(int id)
            => this.dbContext.Towns.Any(t => t.Id == id);

        public TModel GetTownById<TModel>(int id)
            => this.By<TModel>(t => t.Id == id).SingleOrDefault();

        public TModel GetTownByName<TModel>(string name)
            => this.By<TModel>(t => t.Name == name).SingleOrDefault();

        private IEnumerable<TModel> By<TModel>(Func<Town, bool> predicate)
          => this.dbContext.Towns.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}