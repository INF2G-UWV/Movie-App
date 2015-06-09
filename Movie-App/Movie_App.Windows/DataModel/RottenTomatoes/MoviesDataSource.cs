using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Movie_App.DataModel.RottenTomatoes
{
    internal class MoviesDataSource : ObservableCollection<RottenTomatoes>
    {
        private const string apiKey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private const string baseURL = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly string API_CALL;
        // private const string API_CALL = movieSearch;

        public MoviesDataSource()
        {
            if (NameStorage.MovieTitle != null)
            {
                API_CALL = baseURL + "/movies.json?apikey=" + apiKey + "&q=" + NameStorage.MovieTitle + "&page_limit=1";
            }
            else
            {
                API_CALL = baseURL + "/movies.json?apikey=" + apiKey + "&q=terminator&page_limit=1";
            }
            LoadData();
        }

        private async void LoadData()
        {
            var wc = new HttpClient();
            var response = await wc.GetStringAsync(API_CALL);

            var rt = JsonConvert.DeserializeObject<RottenTomatoes>(response);
            foreach (var m in rt.movies)
            {
                var temp = new RottenTomatoes();
                temp.Title = m.title;
                temp.Runtime = m.runtime.ToString();
                temp.ReleaseDatesTheater = m.release_dates.theater;
                temp.Synopsis = m.synopsis;
                temp.RatingsAudience = m.ratings.audience_score.ToString();
                //temp.NameActor = m.abridged_cast.name; (kan geen 'name' vinden)
                for (var i = 0; i < m.abridged_cast.Count; i++)
                {
                    temp.NameActor += m.abridged_cast[i].name + "\n\n";
                }
                // using a temponary variable to store the image source of hte api, so it could be manipulated with regex
                var imageTemp = m.posters.original;
                var replacement = "http://";
                var rgx = "(http://resizing.flixster.com(.*((54x77)|(54x80)|(54x81)|(52x81)|(51x81)|(53x81))/))";
                temp.PosterOriginal = Regex.Replace(imageTemp, rgx, replacement);

                Add(temp);
            }
        }
    }
}