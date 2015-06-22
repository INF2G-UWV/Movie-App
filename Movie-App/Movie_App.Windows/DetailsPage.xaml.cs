using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Movie_App.Common;
using Movie_App.DataModel.RottenTomatoes;
using Movie_App.DataUnits;
using Movie_App.Util;
using Newtonsoft.Json;

namespace Movie_App
{
    /// <summary>
    ///     A page that displays details for a single item within a group while allowing gestures to
    ///     flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        ///     Constructor of DetailsPage
        /// </summary>
        public DetailsPage()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        ///     This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
        }

        /// <summary>
        ///     NavigationHelper is used on each page to aid in navigation and
        ///     process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper { get; private set; }

        /// <summary>
        ///     Populates the page with content passed during navigation.  Any saved state is also
        ///     provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        ///     The source of the event; typically <see cref="NavigationHelper" />
        /// </param>
        /// <param name="e">
        ///     Event data that provides both the navigation parameter passed to
        ///     <see cref="Frame.Navigate(Type, Object)" /> when this page was initially requested and
        ///     a dictionary of state preserved by this page during an earlier
        ///     session.  The state will be null the first time a page is visited.
        /// </param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            object navigationParameter;
            if (e.PageState != null && e.PageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = e.PageState["SelectedItem"];
            }

            //Load all data
            LoadTrailerData();
            DataStorage.TheaterList.Clear();
            TheaterButton.IsEnabled = false;
            GetTheaterData(DataStorage.MovieTitle, DataStorage.City, 0);
        }

        /// <summary>
        ///     Loads trailer data in storage.
        ///     Uses Internet Video Archiva ODATA API to fetch trailer PublishId.
        /// </summary>
        private async void LoadTrailerData()
        {
            //Initialize variables
            var isCallSuccesfull = true;
            DataStorage.PublishId = -1;
            var response = "";
            var API_CALL = "";
            var wc = new HttpClient();

            //Try to get trailer data normally
            try
            {
                API_CALL = string.Format(
                    "http://api.internetvideoarchive.com/2.0/DataService/EntertainmentPrograms()?$filter=substringof('{0}',Title)&format=json&developerid=14c9d1e0-329a-47d9-a29a-2d8268a67a48",
                    DataStorage.MovieTitle);
                response = await wc.GetStringAsync(API_CALL);
            }
            catch
            {
                isCallSuccesfull = false;
            }

            //Try to get trailer data with filtered movietitle parameter
            if (!isCallSuccesfull)
            {
                API_CALL = string.Format(
                    "http://api.internetvideoarchive.com/2.0/DataService/EntertainmentPrograms()?$filter=substringof('{0}',Title)&format=json&developerid=14c9d1e0-329a-47d9-a29a-2d8268a67a48",
                    TitleParse(DataStorage.MovieTitle));
                response = await wc.GetStringAsync(API_CALL);
            }

            //If the response isn't empty
            if (response != null)
            {
                //Deserialize
                var rt = JsonConvert.DeserializeObject<TrailerItem>(response);

                //Loop and add PublishId
                foreach (var node in rt.value)
                {
                    try
                    {
                        if (node.MediaId == 0)
                        {
                            DataStorage.PublishId = node.Publishedid;
                        }
                    }
                    catch
                    {
                        ErrorMessage.Show("Communication error", "Error retrieving trailer information!", "Ok");
                    }
                }
            }
            else
            {
                ErrorMessage.Show("Communication error", "Error retrieving trailer information!", "Ok");
            }
        }

        /// <summary>
        ///     Removes all non-alphanumeric characters from title (string).
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>parsed string</returns>
        private static string TitleParse(string input)
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(input, "");
        }

        /// <summary>
        ///     Fetches movietheater information for current title.
        ///     Checks if Title is currently playing,
        ///     and adds adress, theatername and showtimes to storage.
        ///     The dateoffset is used to select the date of the requested title and showtimes.
        ///     Zero (0) is the current date (today) and every single increment will advance the date by one day.
        ///     Conversely, every single decrement (negative value) will move the date back by one day.
        /// </summary>
        /// <param name="movie">Movietitle</param>
        /// <param name="location">Cityname</param>
        /// <param name="dateoffset">Offset for date</param>
        private async void GetTheaterData(string movie, string location, int dateoffset)
        {
            //Set url
            var url = string.Format("http://movies.waveshapes.nl/get.php?location={0}&movie={1}&offset={2}",
                location.ToLower(),
                movie.ToLower(), dateoffset);
            var wc = new HttpClient();
            try
            {
                //See if exists
                var response = await wc.GetStringAsync(url);

                //If response is not empty
                if (response != "[]" || response != "null")
                {
                    //Deserialize Json response and loop through elements
                    dynamic rt = JsonConvert.DeserializeObject(response);
                    foreach (var m in rt)
                    {
                        var theaterItem = new TheaterItem();
                        theaterItem.Title = (string) m.title;
                        theaterItem.Name = (string) m.name;
                        theaterItem.Address = (string) m.address;
                        theaterItem.Time = FormatTime((string) m.times);
                        DataStorage.TheaterList.Add(theaterItem);
                    }
                    TheaterButton.IsEnabled = true;
                }
            }
            catch
            {
                ErrorMessage.Show("Communcation error!", "Error retrieving theater information!", "Ok");
            }
        }

        /// <summary>
        ///     Format nasty HTML scraped time text to something more usable.
        /// </summary>
        /// <param name="time">string with html time text</param>
        /// <returns>list of strings with the times</returns>
        private static List<string> FormatTime(string time)
        {
            try
            {
                var timeB = new StringBuilder(time);
                timeB.Replace("<!--  -->", "");
                timeB.Replace("&nbsp", "");
                var timeList =
                    timeB.ToString().Trim().Split(null as char[], StringSplitOptions.RemoveEmptyEntries).ToList();
                return timeList;
            }
            catch
            {
                return null;
            }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="Common.NavigationHelper.LoadState" />
        /// and
        /// <see cref="Common.NavigationHelper.SaveState" />
        /// .
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        /// <summary>
        ///     Button click to navigate through page and gives parameter through a static variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buttonIdData = ((Button) e.OriginalSource).DataContext;

            foreach (var obj in (DetailsMovieController) buttonIdData)
            {
                DataStorage.MovieId = obj.MovieId;
            }
        }

        /// <summary>
        ///     When the searchBox is submitted it will call the the searchmodel that's bind by BasicPage2.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            DataStorage.QuerySearch = args.QueryText;
            Frame.Navigate(typeof (SearchPage), args.QueryText);
        }

        #endregion
    }
}