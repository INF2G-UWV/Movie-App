﻿using System.Collections.Generic;

namespace Movie_App.DataModel.RottenTomatoesClips
{
   public class ScrapeItem
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public ScrapeItem()
        {
            Time = new List<string>();
        }

         
        ////fields
        public string Title { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string timeTable { get; set; }
        public List<string> Time { get; set; }
    }
}