namespace Footeo.Web.ViewModels.Fixtures.Output
{
    using System;

    public class FixturesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string Info => $"{this.Name} - {this.Date}";
    }
}