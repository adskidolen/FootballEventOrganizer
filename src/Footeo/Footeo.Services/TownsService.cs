namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

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

        public bool ExistsByName(string name) => this.dbContext.Towns.Any(t => t.Name == name);

        public bool ExistsById(int id) => this.dbContext.Towns.Any(t => t.Id == id);

        public Town GetById(int id) => this.dbContext.Towns.SingleOrDefault(t => t.Id == id);

        public Town GetByName(string name) => this.dbContext.Towns.SingleOrDefault(t => t.Name == name);
    }
}