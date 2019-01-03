namespace Footeo.Services.Contracts
{
    using System.Linq;

    public interface IFieldsService
    {
        void CreateField(string name, string address, bool isIndoors, string townName);
        bool FieldExistsByName(string name);
        IQueryable<TModel> AllFields<TModel>();
    }
}
