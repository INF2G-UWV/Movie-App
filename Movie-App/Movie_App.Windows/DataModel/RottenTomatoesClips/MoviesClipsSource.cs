using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    internal class MoviesClipsSource : ObservableCollection<TheaterItem>
    {
        private const string apiKey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private const string baseURL = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly string API_CALL;
        private readonly TheaterScraper scraper = new TheaterScraper();
        private List<ScrapeItem> scrapeItems = new List<ScrapeItem>();
        private List<RottenTomatoesClips> rottenTomatoesClipItems = new List<RottenTomatoesClips>();
        // private const string API_CALL = movieSearch;

        public MoviesClipsSource()
        {
            if (NameStorage.MovieId != null)
            {
                API_CALL = baseURL + "/movies/" + NameStorage.MovieId + "/clips.json?apikey=" + apiKey;
            }
            else
            {
                API_CALL = baseURL + "/movies/770687943/clips.json?apikey=" + apiKey;
            }
            GetClipData();
        }


        private async void GetScrapeData(string movie, string location, int dateoffset)
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

                if (response != "[]" || response != "null")
                {
                    dynamic rt = JsonConvert.DeserializeObject(response);
                    foreach (var m in rt)
                    {
                        var biosItem = new ScrapeItem();

                        biosItem.Title = (string)m.title;
                        biosItem.Name = (string)m.name;
                        biosItem.Address = (string)m.address;
                        //biosItem.times = m.times;
                        biosItem.Time = FormatTime((string)m.times);
                        scrapeItems.Add(biosItem);
                    }
                    Merge();
                }
            }
            catch (Exception ex)
            {
                //Uh Oh! Error! Really not found this time! Normally program would crash and burn!!
                //MessageBox.Show("Movie not found!", "Search error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private async void GetClipData()
        {
            var wc = new HttpClient();
            var response = await wc.GetStringAsync(API_CALL);

            var rt = JsonConvert.DeserializeObject<RottenTomatoesClips>(response);
            foreach (var m in rt.clips)
            {
                
                var temp = new RottenTomatoesClips();
                temp.clips.Add(m);
                //temp.title = m.clips[0].title;
                //temp.duration = m.clips[0].duration;
                //temp.alternateClip = m.clips[0].links.alternate;
                //temp.thumbnail = m.clips[0].thumbnail;
                //temp.Title = m.title;
                //temp.Runtime = m.runtime.ToString();
                //temp.ReleaseDatesTheater = m.release_dates.theater;
                //temp.Synopsis = m.synopsis;
                //temp.RatingsAudience = m.ratings.audience_score.ToString();
                //temp.movieId = m.id;
                ////temp.NameActor = m.abridged_cast.name; (kan geen 'name' vinden)
                //for (var i = 0; i < m.abridged_cast.Count; i++)
                //{
                //    temp.NameActor += m.abridged_cast[i].name + "\n\n";
                //}
                //// using a temponary variable to store the image source of hte api, so it could be manipulated with regex
                //var imageTemp = m.posters.original;
                //var replacement = "http://";
                //var rgx = "(http://resizing.flixster.com(.*((54x77)|(54x80)|(54x81)|(52x81)|(51x81)|(53x81))/))";
                //temp.PosterOriginal = Regex.Replace(imageTemp, rgx, replacement);
               
                rottenTomatoesClipItems.Add(temp);
            }

            //scraper.Scrape(Items[0].clips[0].title, "Emmen", 0);
            GetScrapeData(rottenTomatoesClipItems[0].clips[0].title,"Emmen",0);
        }

        private void Merge()
        {
            var temp = new TheaterItem();
            foreach (var item in rottenTomatoesClipItems)
            {
              
                temp.ClipList.Add(item);
            }
            foreach (var item in scrapeItems)
            {
               
                temp.ScrapeList.Add(item);
            }
            this.Add(temp);

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