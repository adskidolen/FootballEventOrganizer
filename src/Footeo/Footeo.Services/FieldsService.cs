namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FieldsService : IFieldsService
    {
        private readonly FooteoDbContext dbContext;
        private readonly ITownsService townsService;

        public FieldsService(FooteoDbContext dbContext, ITownsService townsService)
        {
            this.dbContext = dbContext;
            this.townsService = townsService;
        }

        public void CreateField(string name, string address, bool isIndoors, string townName)
        {
            var town = this.townsService.GetTownByName<Town>(townName);

            if (town == null)
            {
                town = this.townsService.CreateTown(townName);
            }

            if (this.FieldExistsByName(name))
            {
                // TODO: Error for existing field
            }

            var field = new Field
            {
                Name = name,
                Address = address,
                IsIndoors = isIndoors,
                Town = town
            };

            this.dbContext.Fields.Add(field);
            this.dbContext.SaveChanges();
        }

        public bool FieldExistsById(int id)
            => this.dbContext.Fields.Any(f => f.Id == id);

        public bool FieldExistsByName(string name)
            => this.dbContext.Fields.Any(f => f.Name == name);

        public IQueryable<TModel> AllFields<TModel>()
            => this.dbContext.Fields.AsQueryable().ProjectTo<TModel>();

        public TModel GetFieldById<TModel>(int id)
            => By<TModel>(f => f.Id == id).SingleOrDefault();

        public TModel GetFieldByName<TModel>(string name)
            => this.By<TModel>(f => f.Name == name).SingleOrDefault();

        private IEnumerable<TModel> By<TModel>(Func<Field, bool> predicate)
            => this.dbContext.Fields.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}