using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using Newtonsoft.Json;

namespace Movie_App.DataModel
{
    public class Query
    {
        private bool hasExecutedQuery;
        // A collection class used to store a list of MovieData objects.
        public ObservableCollection<MovieData> results = new ObservableCollection<MovieData>();

        public Query(Uri uri)
        {
            Uri = uri;
        }

        protected Uri Uri { get; private set; }
        // Executes a query to obtain information about movies.
        // This property also stores the movies in a collection class.
        public ObservableCollection<MovieData> Results
        {
            get
            {
                if (!hasExecutedQuery)
                {
                    ExecuteQuery();
                }

                return results;
            }
        }

        // Calls a service to obtain a list of movies. This method stores each movie
        // in a collection object.
        private async void ExecuteQuery()
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(Uri);

                var responseText = "";

                var headerValues = Enumerable.Empty<string>();

                if (response.Content.Headers.TryGetValues("Content-Encoding", out headerValues))
                {
                    foreach (var headerValue in response.Content.Headers.GetValues("Content-Encoding"))
                    {
                        if (headerValue == "gzip")
                        {
                            using (var responseStream = await response.Content.ReadAsStreamAsync())
                            using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                            using (var streamReader = new StreamReader(decompressedStream))
                            {
                                responseText = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                if (responseText == "")
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var streamReader = new StreamReader(stream);
                    responseText = streamReader.ReadToEnd();
                }


                // Convert the stream to JSON so that it is easier to iterate through
                // each item in the stream.
                dynamic myObj = JsonConvert.DeserializeObject(responseText);
                dynamic movies = myObj.movies;
                dynamic total = myObj.total;

                // Iterate through the collection of JSON objects to obtain information
                // each movie in the collection.
                foreach (var movie in movies)
                {
                    string title = movie.title;
                    string summary = movie.synopsis;
                    string imageTemp = movie.posters.profile;
                    const string replacement = "http://";
                    const string rgx = "(http://resizing.flixster.com(.*((54x77)|(54x80)|(54x81)|(52x81)|(51x81)|(53x81))/))";
                    var image = Regex.Replace(imageTemp, rgx, replacement);
                    string runtime = movie.runtime;
                    string year = movie.year;
                    string rating = movie.mpaa_rating;
                   

                    // Create a Moviedata object by using movie information and add 
                    // that object to a collection.
                    results.Add(new MovieData(title, summary, image, runtime, year, rating));
                }

                hasExecutedQuery = true;
            }
            catch (Exception)
            {
                hasExecutedQuery = false;
                showErrorMessage();
            }
        }

        private async void showErrorMessage()
        {
            var msg = new MessageDialog
                ("The service is unavailable or there was a problem with the service.");

            msg.Commands.Add(new UICommand("Try again?"));

            msg.Commands.Add(new UICommand("I'll try again later."));

            msg.DefaultCommandIndex = 0;
            msg.CancelCommandIndex = 1;

            var results = await msg.ShowAsync();

            if (results.Label == "I'll try again later.")
            {
                hasExecutedQuery = true;
                Results.Clear();
            }
            else
            {
                hasExecutedQuery = false;
                Results.Clear();
            }
        }
    }
}