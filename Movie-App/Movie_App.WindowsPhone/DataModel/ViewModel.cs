using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_App.DataModel
{
    // Provides an object that returns movie data. You can bind to this class in the designer.
    // For example, by referencing this class in the ItemSource property of a GridView control. 
    public class ViewModel
    {
        // A Rotten Tomatoes API key is required
        private static readonly string apikey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private static readonly string baseUrl = "http://api.rottentomatoes.com/api/public/v1.0";
        private static readonly string moviesSearchUrl = baseUrl + "/lists/movies/box_office.json?limit=20&apikey=" + apikey;

        private static readonly Uri uri = new Uri(moviesSearchUrl);

        private readonly Query query = new Query(uri);

        // Property that returns a collection of MovieData objects. 
        public Query Query
        {
            get
            {
                return this.query;
            }
        }
    }
}
