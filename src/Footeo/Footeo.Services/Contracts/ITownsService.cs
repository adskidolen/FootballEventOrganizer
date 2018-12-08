namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    public interface ITownsService
    {
        Town CreateTown(string name);
        bool TownExistsByName(string name);
        bool TownExistsById(int id);
        TModel GetTownById<TModel>(int id);
        TModel GetTownByName<TModel>(string name);
    }
}