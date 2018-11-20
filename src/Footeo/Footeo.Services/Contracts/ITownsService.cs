namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    public interface ITownsService
    {
        Town CreateTown(string name);
        bool ExistsByName(string name);
        bool ExistsById(int id);
        Town GetById(int id);
        Town GetByName(string name);
    }
}