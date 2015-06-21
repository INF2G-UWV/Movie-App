using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Movie_App.Common;
using Movie_App.DataModel;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

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


            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
            // TODO: Assign the selected item to this.flipView.SelectedItem
        }

        /// <summary>
        ///     Loading Iframe with a trailervideo
        /// </summary>
        private void LoadIframe()
        {
            if (NameStorage.PublishId > 0)
            {
                var str =
                    string.Format(
                        @"<body style='background-image:url(http://movies.waveshapes.nl/bg.jpg)'><iframe width='960' height='540' src='http://www.videodetective.com/embed/video/?publishedid=" +
                        NameStorage.PublishId +
                        "&amp;options=false&amp;autostart=true&amp;playlist=none&amp;width=960&amp;height=540' runat='server' frameborder='0' scrolling='no'></iframe>");
                videoView.NavigateToString(str);
            }
            else
            {
                var str =
                    @"<body style='background-image:url(http://movies.waveshapes.nl/bg.jpg)'>";
                videoView.NavigateToString(str);
            }
        }

        public void LoadTheaterData()
        {
            if (NameStorage.TheaterList.Count > 0)
            {
                foreach (var item in NameStorage.TheaterList)
                {
                    if (item.Name != null)
                    {
                        CinemaBox.Items.Add(item.Name);
                    }
                }
                SplashText.Text = string.Format("Currently playing near {0} today:", NameStorage.City);
                CinemaBox.SelectedIndex = 0;
            }
            else
            {
                TheaterUnavailable.Visibility = Visibility.Visible;
                SplashText.Visibility = Visibility.Collapsed;
                Place.Visibility = Visibility.Collapsed;
                Times.Visibility = Visibility.Collapsed;
                Cinema.Visibility = Visibility.Collapsed;
                CinemaBox.Visibility = Visibility.Collapsed;
                PlaceValue.Visibility = Visibility.Collapsed;
                TimesValue.Visibility = Visibility.Collapsed;
                TheaterUnavailable.Text = string.Format("This movie is currently not playing in any theaters in {0}!",
                    NameStorage.City);
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add code to perform some action here.
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadIframe();
            LoadTheaterData();
        }


        /// <summary>
        ///     After button is click call the loadFrame function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //loadIframe();
        }

        private void videoView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            // TODO: Add event handler implementation here.
        }

        private void videoView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //loadIframe();
        }

        private void CinemaBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CinemaBox.SelectedIndex >= 0)
            {
                if (NameStorage.TheaterList[CinemaBox.SelectedIndex].Address != null)
                {
                    PlaceValue.Text = NameStorage.TheaterList[CinemaBox.SelectedIndex].Address;
                }
                if (NameStorage.TheaterList[CinemaBox.SelectedIndex].timeTable.Any())
                {
                    var timeString = "";
                    foreach (var timeItem in NameStorage.TheaterList[CinemaBox.SelectedIndex].Time)
                    {
                        timeString += timeItem + "  ";
                    }
                    TimesValue.Text = timeString;
                }
            }
        }

        #endregion
    }
}