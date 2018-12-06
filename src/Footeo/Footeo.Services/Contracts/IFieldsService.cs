namespace Footeo.Services.Contracts
{
    using Footeo.Models;
    
    using System.Linq;

    public interface IFieldsService
    {
        void CreateField(string name, string address, bool isIndoors, string townName);
        bool ExistsById(int id);
        bool ExistsByName(string name);
        Field GetById(int id);
        Field GetByName(string name);
        IQueryable<TModel> All<TModel>();
    }
}
