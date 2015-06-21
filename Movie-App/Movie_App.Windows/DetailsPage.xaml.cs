using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Movie_App.Common;
using Movie_App.DataModel;
using Movie_App.DataModel.RottenTomatoes;
using Movie_App.DataModel.RottenTomatoesClips;
using Newtonsoft.Json;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Movie_App
{
    /// <summary>
    ///     A page that displays details for a single item within a group while allowing gestures to
    ///     flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

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

            LoadTrailerData();
            NameStorage.TheaterList.Clear();
            TheaterButton.IsEnabled = false;
            GetScrapeData(NameStorage.MovieTitle, NameStorage.City, 0);

            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
            // TODO: Assign the selected item to this.flipView.SelectedItem
        }

        private async void LoadTrailerData()
        {
            NameStorage.PublishId = -1;
            var API_CALL =
                string.Format(
                    "http://api.internetvideoarchive.com/2.0/DataService/EntertainmentPrograms()?$filter=substringof('{0}',Title)&format=json&developerid=14c9d1e0-329a-47d9-a29a-2d8268a67a48",
                    NameStorage.MovieTitle);
            var wc = new HttpClient();
            var response = await wc.GetStringAsync(API_CALL);

            var rt = JsonConvert.DeserializeObject<TrailerItem>(response);

            foreach (var node in rt.value)
            {
                try
                {
                    if (node.MediaId == 0)
                    {
                        NameStorage.PublishId = node.Publishedid;
                    }
                }
                catch
                {
                }
            }
        }

        private async void GetScrapeData(string movie, string location, int dateoffset)
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

                if (response != "[]" || response != "null")
                {
                    dynamic rt = JsonConvert.DeserializeObject(response);
                    foreach (var m in rt)
                    {
                        var biosItem = new ScrapeItem();

                        biosItem.Title = (string) m.title;
                        biosItem.Name = (string) m.name;
                        biosItem.Address = (string) m.address;
                        //biosItem.times = m.times;
                        biosItem.timeTable = (string) m.times;
                        //for (int i = 0; i < biosItem.Time.Count; i++)
                        //{

                        //}
                        //biosItem.timeTable += FormatTime((string)m.times) + "\n\n";

                        biosItem.Time = FormatTime((string) m.times);
                        // scrapeItems.Add(biosItem);
                        NameStorage.TheaterList.Add(biosItem);
                    }
                    TheaterButton.IsEnabled = true;
                    //Merge();
                }
            }
            catch (Exception ex)
            {
                //Uh Oh! Error! Really not found this time! Normally program would crash and burn!!
                //MessageBox.Show("Movie not found!", "Search error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     Format nasty HTML scraped time text to something more usable.
        /// </summary>
        /// <param name="time">string with html time text</param>
        /// <returns>list of strings with the times!</returns>
        private List<string> FormatTime(string time)
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
                NameStorage.MovieId = obj.MovieId;
            }
        }

        /// <summary>
        ///     When the searchBox is submitted it will call the the searchmodel that's bind by BasicPage2.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            NameStorage.QuerySearch = args.QueryText;
            Frame.Navigate(typeof (BasicPage2), args.QueryText);
        }

        #endregion
    }
}