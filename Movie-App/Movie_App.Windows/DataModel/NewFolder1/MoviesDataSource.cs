using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Movie_App.DataModel.NewFolder1
{
    class MoviesDataSource : ObservableCollection<rottenTomatoes>
    {
        private const string apiKey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private const string baseURL = "http://api.rottentomatoes.com/api/public/v1.0";
        private const string movieSearch = baseURL + "/movies.json?apikey=" + apiKey + "&q=San&page_limit=1";
        private const string API_CALL = movieSearch;

        public MoviesDataSource()
        {
            loadData();
        }

        async private void loadData()
        {
            HttpClient wc = new HttpClient();
            string response = await wc.GetStringAsync(API_CALL);

            rottenTomatoes rt = (rottenTomatoes)JsonConvert.DeserializeObject<rottenTomatoes>(response);
            foreach (rottenTomatoes.Movie m in rt.movies)
            {
                rottenTomatoes temp = new rottenTomatoes();
                temp.Title = m.title;
                temp.Runtime = m.runtime.ToString();
                temp.ReleaseDatesTheater = m.release_dates.theater;
                temp.Synopsis = m.synopsis;
                temp.RatingsAudience = m.ratings.audience_score.ToString();
                //temp.NameActor = m.abridged_cast.name; (kan geen 'name' vinden)
                
                // using a temponary variable to store the image source of hte api, so it could be manipulated with regex
                string imageTemp = m.posters.original;
                string replacement = "http://";
                string rgx = "(http://resizing.flixster.com(.*((54x77)|(54x80)|(54x81)|(51x81)|(53x81))/))";
                temp.PosterOriginal = Regex.Replace(imageTemp, rgx, replacement);

                this.Add(temp);
            }
        }
    }
}
