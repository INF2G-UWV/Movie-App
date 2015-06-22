using System.Collections.Generic;

namespace Movie_App.DataUnits
{
    /// <summary>
    ///     This class holds the data for fetched movie theaters
    /// </summary>
    public class TheaterItem
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public TheaterItem()
        {
            Time = new List<string>();
        }

        //fields
        public string Title { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<string> Time { get; set; }
    }
}