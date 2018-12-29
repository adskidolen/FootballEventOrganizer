namespace Footeo.Web.ViewModels.Referees.Output
{
    public class RefereeViewModel
    {
        private const string NoMatchAttendancesMessage = "N/A";

        public string FullName { get; set; }

        public int MatchAttendances { get; set; }
        public string ShowMatchAttendances => this.MatchAttendances == 0 ? NoMatchAttendancesMessage : this.MatchAttendances.ToString();
    }
}