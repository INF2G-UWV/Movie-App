using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            foreach (rottenTomatoes.RootObject m in rt.movies)
            {
                rottenTomatoes temp = new rottenTomatoes();
                temp.Title = m.title;
                temp.Runtime = m.runtime.ToString();
                temp.Year = m.year.ToString();
                temp.Synopsis = m.synopsis;
                this.Add(temp);
            }
        }

        

    }
}
