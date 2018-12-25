namespace Footeo.Web.ViewModels.Fields.Output
{
    public class FieldViewModel
    {
        private const string IndoorsName = "Indoors";
        private const string OutdoorsName = "Outdoors";

        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsIndoors { get; set; }
        public string ShowIsIndoor => this.IsIndoors ? IndoorsName: OutdoorsName;

        public string TownName { get; set; }
    }
}