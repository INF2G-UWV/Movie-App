using System;

namespace Movie_App.DataModel.RottenTomatoesSearch
{
    // Provides an object that returns movie data. You can bind to this class in the designer.
    // For example, by referencing this class in the ItemSource property of a GridView control. 
    public class SearchModel
    {
        // A Rotten Tomatoes API key is required
        private static readonly string apikey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private static readonly string baseUrl = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly MoviesDataSourceSearch search = new MoviesDataSourceSearch();
        // Property that returns a collection of MovieData objects. 
        public MoviesDataSourceSearch Search
        {
            get
            {
                if (NameStorage.QuerySearch != null)
                {
                    search.Uri = new Uri(baseUrl + "/movies.json?apikey=" + apikey + "&q=" + NameStorage.QuerySearch);
                }
                else
                {
                    search.Uri = new Uri(baseUrl + "/movies.json?apikey=" + apikey + "&q=jurassic");
                }
                return search;
            }
        }
    }
}