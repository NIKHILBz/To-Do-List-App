namespace To_Do_List_App.Models
{
    public class Filters
    {
        public Filters(string filterstring) {
            Filterstring = filterstring ?? "all-all-all";
            string[] filter = Filterstring.Split('-');
             CategoryId = filter[0];
            Due = filter[1];
            StatusId = filter[2];
        }

        public string Filterstring { get; }
        public string CategoryId { get; }

        public string Due { get; }

        public string StatusId { get; }
        public bool HasCategory => CategoryId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";
        public static Dictionary<string, string> DueFilterValues =>new Dictionary<string, string> 
        {
            {"future", "Future"},
            {"past", "Past"},
            {"today", "Today" }
        };

        public bool IsPast => Due.ToLower() == "past";
        public bool IsFuture => Due.ToLower() == "today";
        public bool IsToday => Due.ToLower() == "future";

    }
}
