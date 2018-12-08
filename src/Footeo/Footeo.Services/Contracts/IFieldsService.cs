namespace Footeo.Services.Contracts
{
    using System.Linq;

    public interface IFieldsService
    {
        void CreateField(string name, string address, bool isIndoors, string townName);
        bool FieldExistsById(int id);
        bool FieldExistsByName(string name);
        TModel GetFieldById<TModel>(int id);
        TModel GetFieldByName<TModel>(string name);
        IQueryable<TModel> AllFields<TModel>();
    }
}
