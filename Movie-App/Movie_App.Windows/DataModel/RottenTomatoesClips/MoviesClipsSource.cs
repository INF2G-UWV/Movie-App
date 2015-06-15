using System.Collections.ObjectModel;
using System.Net.Http;
using Movie_App.DataModel.RottenTomatoesClips;
using Newtonsoft.Json;

namespace Movie_App.DataModel.RottenTomatoes
{
    internal class MoviesClipsSource : ObservableCollection<RottenTomatoesClips>
    {
        private const string apiKey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private const string baseURL = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly string API_CALL;
        private readonly TheaterScraper scraper = new TheaterScraper();
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
            LoadData();
        }

        private async void LoadData()
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

                Add(temp);
            }

            scraper.Scrape(Items[0].clips[0].title, "Emmen", 0);
        }
    }
}