namespace Movie_App.DataModel
{
    // This class represents a movie. It contains properties the describe the movie.
    public class MovieData
    {
        public MovieData(string title, string summary, string image, string runtime, string release, string rating)
        {
            Title = title;
            Summary = summary;
            Image = image;
            Runtime = runtime;
            Release = release;
            Rating = rating;
        }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public string Runtime { get; set; }
        public string Release { get; set; }
        public string Rating { get; set; }
    }
}