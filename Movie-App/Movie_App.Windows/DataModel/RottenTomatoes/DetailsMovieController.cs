using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using Movie_App.DataUnits;
using Newtonsoft.Json;

namespace Movie_App.DataModel.RottenTomatoes
{
    /// <summary>
    ///     Controller of DetailsPage
    /// </summary>
    internal class DetailsMovieController : ObservableCollection<DetailsMovieData>
    {
        /// <summary>
        ///     Constant fields
        /// </summary>
        private const string ApiKey = "xjndv3dfyfn2bzxvwmuqj8gz";

        private const string BaseUrl = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly string API_CALL;

        /// <summary>
        ///     Constructor to set a dynamic variable in the link
        /// </summary>
        public DetailsMovieController()
        {
            if (DataStorage.MovieTitle != null)
            {
                API_CALL = BaseUrl + "/movies.json?apikey=" + ApiKey + "&q=" + DataStorage.MovieTitle + "&page_limit=1";
            }
            else
            {
                API_CALL = BaseUrl + "/movies.json?apikey=" + ApiKey + "&q=terminator&page_limit=1";
            }
            LoadData();
        }

        /// <summary>
        ///     This function loads the data by calling the RottenTomatoes database,
        ///     It serializes the data to an object so it could be used on the page.
        /// </summary>
        private async void LoadData()
        {
            //Fields
            var wc = new HttpClient();
            var response = await wc.GetStringAsync(API_CALL);

            //Deserialize and iterate through it to fetch invididual items
            dynamic rt = JsonConvert.DeserializeObject(response);
            foreach (var m in rt.movies)
            {
                var temp = new DetailsMovieData();
                temp.Title = m.title;
                temp.Runtime = m.runtime.ToString();
                temp.ReleaseDatesTheater = m.release_dates.theater;
                temp.Synopsis = m.synopsis;
                temp.RatingsAudience = m.ratings.audience_score.ToString();
                temp.MovieId = m.id;

                //Looping through a list object and return the string accessor
                for (var i = 0; i < m.abridged_cast.Count; i++)
                {
                    temp.NameActor += m.abridged_cast[i].name + "\n\n";
                }
                //Using a temporary variable to store the image url of the api, to allow regex manipulation
                var imageTemp = m.posters.original;
                const string replacement = "http://";
                const string rgx =
                    "(http://resizing.flixster.com(.*((54x77)|(54x80)|(54x81)|(52x81)|(51x81)|(53x81))/))";
                temp.PosterOriginal = Regex.Replace((string) imageTemp, rgx, replacement);

                Add(temp);
            }
        }
    }
}