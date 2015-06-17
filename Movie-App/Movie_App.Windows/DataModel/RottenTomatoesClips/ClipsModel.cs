using System;

namespace Movie_App.DataModel.RottenTomatoesClips
{
    // Provides an object that returns movie data. You can bind to this class in the designer.
    // For example, by referencing this class in the ItemSource property of a GridView control. 
    public class ClipsModel
    {
        // A Rotten Tomatoes API key is required
        private static readonly string apikey = "xjndv3dfyfn2bzxvwmuqj8gz";
        private static readonly string baseUrl = "http://api.rottentomatoes.com/api/public/v1.0";
        private readonly MoviesClipsSource clipsSource = new MoviesClipsSource();
        // Property that returns a collection of MovieData objects. 
        public MoviesClipsSource ClipsSource
        {
            get
            {
                if (NameStorage.MovieId != null)
                {
                    clipsSource.Uri = new Uri(baseUrl + "/movies.json?apikey=" + apikey + "&q=" + NameStorage.MovieId);
                }
                else
                {
                    clipsSource.Uri = new Uri(baseUrl + "/movies.json?apikey=" + apikey + "&q=jurassic");
                }
                return clipsSource;
            }
        }
    }
}