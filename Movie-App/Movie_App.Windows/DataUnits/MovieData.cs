namespace Movie_App.DataUnits
{
    // This class represents a movie. It contains properties the describe the movie.
    public class MovieData
    {
        public MovieData(string title, string summary, string image, string runtime, string year, string rating)
        {
            Title = title;
            Summary = summary;
            Image = image;
            Runtime = runtime;
            Year = year;
            Rating = rating;
        }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public string Runtime { get; set; }
        public string Year { get; set; }
        public string Rating { get; set; }
    }
}