using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    internal class TheaterScraper
    {
        private readonly List<TheaterItem> items = new List<TheaterItem>();

        public List<TheaterItem> Items
        {
            get { return items; }
        }

        /// <summary>
        ///     Scrape movie data from www.google.com/movies.
        /// </summary>
        /// <param name="movie">Movie title to search for</param>
        public async void Scrape(string movie, string location, int dateoffset)
        {
            //Set url
            var url = string.Format("http://movies.waveshapes.nl/get.php?location={0}&movie={1}&offset={2}",
                location.ToLower(),
                movie.ToLower(), dateoffset);
            var wc = new HttpClient();
            try
            {
                //See if exists
                var response = await wc.GetStringAsync(url);

                if (response != "[]")
                {
                    dynamic rt = JsonConvert.DeserializeObject(response);
                    foreach (var m in rt)
                    {
                        var biosItem = new TheaterItem();

                        biosItem.Title = (string) m.title;
                        biosItem.Name = (string) m.name;
                        biosItem.Address = (string) m.address;
                        //biosItem.times = m.times;
                        biosItem.Time = FormatTime((string) m.times);
                        items.Add(biosItem);
                    }

#if DEBUG
                    Debug.WriteLine("Scraper works! First theater name in {0} found for movie {1}: {2}", location,
                        Items[0].Title, Items[0].Name);
#endif
                }
            }
            catch (Exception ex)
            {
                //Uh Oh! Error! Really not found this time! Normally program would crash and burn!!
                //MessageBox.Show("Movie not found!", "Search error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     Format nasty HTML scraped time text to something more usable.
        /// </summary>
        /// <param name="time">string with html time text</param>
        /// <returns>list of strings with the times!</returns>
        private List<string> FormatTime(string time)
        {
            var timeB = new StringBuilder(time);
            timeB.Replace("<!--  -->", "");
            timeB.Replace("&nbsp", "");
            var timeList = timeB.ToString().Trim().Split(null as char[], StringSplitOptions.RemoveEmptyEntries).ToList();
            return timeList;
        }
    }
}