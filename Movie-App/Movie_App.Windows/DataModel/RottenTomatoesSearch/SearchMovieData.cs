namespace Movie_App.DataModel.RottenTomatoesSearch
{
    public class SearchMovieData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="image">string image</param>
        /// <param name="title">string title</param>
        /// <param name="year">string year</param>
        public SearchMovieData(string image, string title, string year)
        {
            Title = title;
            Image = image;
            Year = year;
        }

        /// <summary>
        /// Methods accessors and mutators
        /// </summary>
        public string Title { get; set; }
        public string Image { get; set; }
        public string Year { get; set; }
    }
}