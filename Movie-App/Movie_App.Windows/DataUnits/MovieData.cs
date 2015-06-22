namespace Movie_App.DataUnits
{
    /// <summary>
    ///     This class represents a movie. It contains properties the describe the movie.
    /// </summary>
    public class MovieData
    {
        /// <summary>
        ///     Constructor of MovieData
        /// </summary>
        /// <param name="title">string - movie title</param>
        /// <param name="summary">string - summary</param>
        /// <param name="image">string - url of image</param>
        /// <param name="runtime">string - runtime</param>
        /// <param name="year">string - year</param>
        /// <param name="rating">string - rating</param>
        public MovieData(string title, string summary, string image, string runtime, string year, string rating)
        {
            Title = title;
            Summary = summary;
            Image = image;
            Runtime = runtime;
            Year = year;
            Rating = rating;
        }

        //Fields
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public string Runtime { get; set; }
        public string Year { get; set; }
        public string Rating { get; set; }
    }
}