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

        public Town Create(string name)
        {
            var town = new Town
            {
                Name = name
            };

            this.dbContext.Towns.Add(town);
            this.dbContext.SaveChanges();

            return town;
        }

        public bool Exists(string name) => this.dbContext.Towns.Any(t => t.Name == name);

        public bool Exists(int id) => this.dbContext.Towns.Any(t => t.Id == id);

        public Town GetById(int id) => this.dbContext.Towns.SingleOrDefault(t => t.Id == id);

        public Town GetByName(string name) => this.dbContext.Towns.SingleOrDefault(t => t.Name == name);
    }
}