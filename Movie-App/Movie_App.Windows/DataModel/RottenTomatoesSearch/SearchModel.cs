using System;
using Movie_App.DataUnits;

namespace Movie_App.DataModel.RottenTomatoesSearch
{
    /// <summary>
    ///     Initializes the fetching of user invoked search
    /// </summary>
    public class SearchModel
    {
        // A Rotten Tomatoes API key is required
        private static readonly string apikey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private static readonly string baseUrl = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly SearchController search = new SearchController();

        /// <summary>
        ///     Property that returns a collection of MovieData objects.
        /// </summary>
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