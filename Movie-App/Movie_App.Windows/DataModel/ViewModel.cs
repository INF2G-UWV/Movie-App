using System;

namespace Movie_App.DataModel
{
    // Provides an object that returns movie data. You can bind to this class in the designer.
    // For example, by referencing this class in the ItemSource property of a GridView control. 
    public class ViewModel
    {
        // A Rotten Tomatoes API key is required
        private const string Apikey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private const string BaseUrl = "http://api.rottentomatoes.com/api/public/v1.0";

        private static readonly string moviesSearchUrl = BaseUrl + "/lists/movies/box_office.json?limit=20&apikey=" +
                                                         Apikey;

        private static readonly Uri Uri = new Uri(moviesSearchUrl);
        private readonly Query query = new Query(Uri);
        // Property that returns a collection of MovieData objects. 
        public Query Query
        {
            get { return query; }
        }
    }
}