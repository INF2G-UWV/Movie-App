using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_App.DataModel
{
    // This class represents a movie. It contains properties the describe the movie.
    public class MovieData
    {
        public MovieData(string title, string summary, string image, string runtime, string release, string rating)
        {
            this.Title = title;
            this.Summary = summary;
            this.Image = image;
            this.Runtime = runtime;
            this.Release = release;
            this.Rating = rating;
        }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public string Runtime { get; set; }
        public string Release { get; set; }
        public string Rating { get; set; }
    }
}
