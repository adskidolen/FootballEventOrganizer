namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    public interface ITownsService
    {
        Town CreateTown(string name);
        TModel GetTownByName<TModel>(string name);
    }
}