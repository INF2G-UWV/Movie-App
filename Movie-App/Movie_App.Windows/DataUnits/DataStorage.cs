using System.Collections.Generic;

namespace Movie_App.DataUnits
{
    /// <summary>
    ///     Public static data container to communicate between pages and classes
    /// </summary>
    public static class DataStorage
    {
        /// <summary>
        ///     Constructor of DataStorage
        /// </summary>
        static DataStorage()
        {
            //Set list and initialize default values
            TheaterList = new List<TheaterItem>();
            City = "Emmen";
        }

        //Fields
        public static string MovieTitle { get; set; }
        public static string MovieId { get; set; }
        public static string QuerySearch { get; set; }
        public static int PublishId { get; set; }
        public static List<TheaterItem> TheaterList { get; set; }
        public static string City { get; set; }
    }
}