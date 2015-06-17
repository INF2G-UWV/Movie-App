using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using Windows.UI.Popups;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    public class MoviesClipsSource : ObservableCollection<TheaterItem>
    {
        private readonly TheaterScraper scraper = new TheaterScraper();
        private List<ScrapeItem> scrapeItems = new List<ScrapeItem>();
        private List<RottenTomatoesClips> rottenTomatoesClipItems = new List<RottenTomatoesClips>();
        // private const string API_CALL = movieSearch;
        /// <summary>
        ///     Constant fields
        /// </summary>
        private bool hasExecutedQuery;

        public ObservableCollection<TheaterItem> results = new ObservableCollection<TheaterItem>();
        public Uri Uri { get; set; }
        // Executes a query to obtain information about movies.
        // This property also stores the movies in a collection class.
        public ObservableCollection<TheaterItem> Results
        {
            get
            {
                if (!hasExecutedQuery)
                {
                    GetClipData();
                    
                }

                return results;
            }
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
            try
            {
                var wc = new HttpClient();
                var response = await wc.GetAsync(Uri);

                var responseText = "";

                var headerValues = Enumerable.Empty<string>();

                if (response.Content.Headers.TryGetValues("Content-Encoding", out headerValues))
                {
                    foreach (var headerValue in response.Content.Headers.GetValues("Content-Encoding"))
                    {
                        if (headerValue == "gzip")
                        {
                            using (var responseStream = await response.Content.ReadAsStreamAsync())
                            using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                            using (var streamReader = new StreamReader(decompressedStream))
                            {
                                responseText = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                if (responseText == "")
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var streamReader = new StreamReader(stream);
                    responseText = streamReader.ReadToEnd();
                }

                // Convert the stream to JSON so that it is easier to iterate through
                // each item in the stream.
                dynamic myObj = JsonConvert.DeserializeObject(responseText);
                dynamic movies = myObj.movies;
                dynamic total = myObj.total;
                foreach (var m in movies.clips)
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
                GetScrapeData(rottenTomatoesClipItems[0].clips[0].title, "Emmen", 0);
                hasExecutedQuery = true;
            }
            catch(Exception)
            {
                hasExecutedQuery = false;
                showErrorMessage();
            }
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

        private async void showErrorMessage()
        {
            var msg = new MessageDialog
                ("The service is unavailable or there was a problem with the service.");

            msg.Commands.Add(new UICommand("Try again?"));

            msg.Commands.Add(new UICommand("I'll try again later."));

            msg.DefaultCommandIndex = 0;
            msg.CancelCommandIndex = 1;

            var results = await msg.ShowAsync();

            if (results.Label == "I'll try again later.")
            {
                hasExecutedQuery = true;
                Results.Clear();
            }
            else
            {
                hasExecutedQuery = false;
                Results.Clear();
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