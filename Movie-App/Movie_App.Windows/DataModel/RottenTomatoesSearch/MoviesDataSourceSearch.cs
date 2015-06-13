using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using Newtonsoft.Json;

namespace Movie_App.DataModel.RottenTomatoesSearch
{
    public class MoviesDataSourceSearch
    {
        /// <summary>
        ///     Constant fields
        /// </summary>
        private bool hasExecutedQuery;

        public ObservableCollection<GetSearchData> results = new ObservableCollection<GetSearchData>();
        public Uri Uri { get; set; }
        // Executes a query to obtain information about movies.
        // This property also stores the movies in a collection class.
        public ObservableCollection<GetSearchData> Results
        {
            get
            {
                if (!hasExecutedQuery)
                {
                    LoadData();
                }

                return results;
            }
        }

        /// <summary>
        ///     This function loads data from the calling RottenTomatoes database, It serialize the to an object.
        ///     So it could be used on the page.
        /// </summary>
        private async void LoadData()
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
                    string imageTemp = movie.posters.profile;
                    string year = movie.year;
                    const string replacement = "http://";
                    const string rgx =
                        "(http://resizing.flixster.com(.*((54x77)|(54x80)|(54x81)|(52x81)|(51x81)|(53x81))/))";
                    var image = Regex.Replace(imageTemp, rgx, replacement);


                    // Create a Moviedata object by using movie information and add 
                    // that object to a collection.
                    results.Add(new GetSearchData(image, title, year));
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