namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

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
            var town = this.townsService.GetByName(townName);

            if (town == null)
            {
                town = this.townsService.CreateTown(townName);
            }

            if (this.ExistsByName(name))
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

        public bool ExistsById(int id)
            => this.dbContext.Fields.Any(f => f.Id == id);

        public bool ExistsByName(string name)
            => this.dbContext.Fields.Any(f => f.Name == name);

        public Field GetById(int id)
            => this.dbContext.Fields.SingleOrDefault(f => f.Id == id);

        public Field GetByName(string name)
            => this.dbContext.Fields.SingleOrDefault(f => f.Name == name);

        public IQueryable<TModel> All<TModel>()
            => this.dbContext.Fields.AsQueryable().ProjectTo<TModel>();
    }
}