using System.Collections.Generic;
using Movie_App.DataModel.RottenTomatoesClips;

namespace Movie_App.DataModel
{
    public static class NameStorage
    {
        static NameStorage()
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