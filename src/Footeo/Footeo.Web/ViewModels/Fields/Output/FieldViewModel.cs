namespace Footeo.Web.ViewModels.Fields.Output
{
    public class FieldViewModel
    {
        private const string IndoorsName = "Indoor";
        private const string OutdoorsName = "Outdoor";

        public string Name { get; set; }

        public string Address { get; set; }
        public string TownName { get; set; }
        public string ShowLocation => $"{this.Address}, {this.TownName}";

        public bool IsIndoors { get; set; }
        public string ShowIsIndoor => this.IsIndoors ? IndoorsName: OutdoorsName;

    }
}