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
            var town = this.townsService.GetTownByName<Town>(townName);

            if (town == null)
            {
                town = this.townsService.CreateTown(townName);
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

        public bool FieldExistsByName(string name)
            => this.dbContext.Fields.Any(f => f.Name == name);

        public IQueryable<TModel> AllFields<TModel>()
            => this.dbContext.Fields.AsQueryable().ProjectTo<TModel>();
    }
}