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
        private const string API_CALL = "http://api.rottentomatoes.com/api/public/v1.0/movies.json?apikey=xjndv3dfyfn2bzxvwmuqj8gz&q=san%20andreas&page_limit=1";

        public MoviesDataSource()
        {
            loadData();
        }

        async private void loadData()
        {
            HttpClient wc = new HttpClient();
            string response = await wc.GetStringAsync(API_CALL);

            rottenTomatoes rt = (rottenTomatoes)JsonConvert.DeserializeObject<rottenTomatoes>(response);
            foreach (rottenTomatoes.RTMovie m in rt.movies)
            {
                rottenTomatoes temp = new rottenTomatoes();
                temp.Title = m.title;
                temp.Rating = m.mpaa_rating;
                temp.Year = m.year.ToString();
                temp.Synopsis = m.synopsis;
                this.Add(temp);
            }
        }

        

    }
}
