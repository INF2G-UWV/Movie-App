using System;
using Movie_App.DataUnits;

namespace Movie_App.DataModel.RottenTomatoesSearch
{
    // Provides an object that returns movie data. You can bind to this class in the designer.
    // For example, by referencing this class in the ItemSource property of a GridView control. 
    public class SearchModel
    {
        // A Rotten Tomatoes API key is required
        private static readonly string apikey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private static readonly string baseUrl = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly SearchController search = new SearchController();
        // Property that returns a collection of MovieData objects. 
        public SearchController Search
        {
            get
            {
                if (DataStorage.QuerySearch != null)
                {
                    search.Uri = new Uri(baseUrl + "/movies.json?apikey=" + apikey + "&q=" + DataStorage.QuerySearch);
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