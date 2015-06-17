using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    public class TheaterItem
    {
        private List<RottenTomatoesClips> clipList = new List<RottenTomatoesClips>(); 
        private List<ScrapeItem> scrapeList = new List<ScrapeItem>();

        public List<ScrapeItem> ScrapeList 
        {
            get
            {
                return scrapeList;
            }

            set { scrapeList = value; }
           
        }

        public List<RottenTomatoesClips> ClipList
        {
            get
            {
                return clipList;
            }

            set { clipList = value; }
        }
    }
}
