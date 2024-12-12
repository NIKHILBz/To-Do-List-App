namespace To_Do_List_App.Models
{
    public class Filters
    {
        public Filters(string filterstring) {
            Filterstring = filterstring ?? "all-all-all";
            string[] filter = Filterstring.Split('-');
             CatergoryId = filter[0];
            Due = filter[1];
            Statusid = filter[2];
        }

        public string Filterstring { get; }
        public string CatergoryId { get; }

        public string Due { get; }

        public string Statusid { get; }
        public bool HasCategory => CatergoryId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasStatus => Statusid.ToLower() != "all";
        public static Dictionary<string, string> DueFilterValues =>new Dictionary<string, string> 
        {
            {"future", "Future"},
            {"past", "Past"},
            {"today", "Today" }
        };
    }
}
