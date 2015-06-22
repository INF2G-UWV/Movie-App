using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Movie_App.Common;
using Movie_App.DataUnits;

namespace Movie_App
{
    /// <summary>
    ///     A page that displays details for a single item within a group while allowing gestures to
    ///     flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class InTheathers : Page
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public InTheathers()
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
        }

        /// <summary>
        ///     Load the videoView with a trailer.
        /// </summary>
        private void LoadIframe()
        {
            if (DataStorage.PublishId > 0)
            {
                var str =
                    string.Format(
                        @"<body style='background-image:url(http://movies.waveshapes.nl/bg.jpg)'><iframe width='960' height='540' src='http://www.videodetective.com/embed/video/?publishedid=" +
                        DataStorage.PublishId +
                        "&amp;options=false&amp;autostart=true&amp;playlist=none&amp;width=960&amp;height=540' runat='server' frameborder='0' scrolling='no'></iframe>");
                videoView.NavigateToString(str);
            }
            else
            {
                videoView.Visibility = Visibility.Collapsed;
                NoTrailerImg.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        ///     Load the theaterdata into the interface.
        /// </summary>
        public void LoadTheaterData()
        {
            //Check if not empty
            if (DataStorage.TheaterList.Count > 0)
            {
                foreach (var item in DataStorage.TheaterList)
                {
                    //If item not null
                    if (item.Name != null)
                    {
                        CinemaBox.Items.Add(item.Name);
                    }
                }
                //Display text
                SplashText.Text = string.Format("Currently playing near {0} today:", DataStorage.City);
                //Autoselect first item in CinemaBox
                CinemaBox.SelectedIndex = 0;
            }
            else
            {
                //When no theater data is found, hide all the irrelevant elements and display text.
                TheaterUnavailable.Visibility = Visibility.Visible;
                SplashText.Visibility = Visibility.Collapsed;
                Place.Visibility = Visibility.Collapsed;
                Times.Visibility = Visibility.Collapsed;
                Cinema.Visibility = Visibility.Collapsed;
                CinemaBox.Visibility = Visibility.Collapsed;
                PlaceValue.Visibility = Visibility.Collapsed;
                TimesValue.Visibility = Visibility.Collapsed;
                TheaterUnavailable.Text = string.Format("This movie is currently not playing in any theaters near {0}!",
                    DataStorage.City);
            }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="GridCS.Common.NavigationHelper.LoadState" />
        /// and
        /// <see cref="GridCS.Common.NavigationHelper.SaveState" />
        /// .
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        ///     Get where is navigated from
        /// </summary>
        /// <param name="e">parameter</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        /// <summary>
        ///     Execute when Grid is loaded.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">parameter</param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadIframe();
            LoadTheaterData();
        }


        private void videoView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
        }

        private void videoView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
        }

        /// <summary>
        ///     Executes when selection is changed in CinemaBox.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">parameter</param>
        private void CinemaBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //If selectedvalue is not negative
            if (CinemaBox.SelectedIndex >= 0)
            {
                //If address value is not null, add to PlaceValue.
                if (DataStorage.TheaterList[CinemaBox.SelectedIndex].Address != null)
                {
                    PlaceValue.Text = DataStorage.TheaterList[CinemaBox.SelectedIndex].Address;
                }

                //If any movietimes exist in list, add to TimesValue.
                if (DataStorage.TheaterList[CinemaBox.SelectedIndex].Time.Any())
                {
                    var timeString = "";
                    foreach (var timeItem in DataStorage.TheaterList[CinemaBox.SelectedIndex].Time)
                    {
                        timeString += timeItem + "  ";
                    }
                    TimesValue.Text = timeString;
                }
            }
        }


        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            const string str = @"<body style='background-image:url(http://movies.waveshapes.nl/bg.jpg)'>";
            videoView.NavigateToString(str);
            videoView.Stop();
        }


        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            const string str = @"<body style='background-image:url(http://movies.waveshapes.nl/bg.jpg)'>";
            videoView.NavigateToString(str);
            videoView.Stop();
        }

        #endregion
    }
}