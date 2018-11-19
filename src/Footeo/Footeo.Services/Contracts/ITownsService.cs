namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    public interface ITownsService
    {
        Town Create(string name);
        bool Exists(string name);
        bool Exists(int name);
        Town GetById(int id);
        Town GetByName(string name);
    }
}