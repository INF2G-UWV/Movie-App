using System.Collections.Generic;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    internal class TheaterItem
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public TheaterItem()
        {
            Time = new List<string>();
        }

        ////fields
        public string Title { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<string> Time { get; set; }
    }
}