using System.Collections.Generic;

namespace Movie_App.DataUnits
{
    public static class DataStorage
    {
        static DataStorage()
        {
            TheaterList = new List<ScrapeItem>();
            City = "Emmen";
        }

        public static string MovieTitle { get; set; }
        public static string MovieId { get; set; }
        public static string QuerySearch { get; set; }
        public static int PublishId { get; set; }
        public static List<ScrapeItem> TheaterList { get; set; }
        public static string City { get; set; }
    }
}