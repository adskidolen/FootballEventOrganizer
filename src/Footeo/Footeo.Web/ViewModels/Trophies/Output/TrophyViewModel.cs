namespace Footeo.Web.ViewModels.Trophies.Output
{
    public class TrophyViewModel
    {
        private const string NameSuffix = "Cup";

        public string Name { get; set; }

        public string ShowName => $"{this.Name} {NameSuffix}";
    }
}