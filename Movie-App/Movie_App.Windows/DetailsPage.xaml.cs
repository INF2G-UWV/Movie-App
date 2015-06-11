using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Movie_App.Common;
using Movie_App.DataModel;
using Movie_App.DataModel.RottenTomatoes;

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
        private string movieTitle;

        public DetailsPage()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += navigationHelper_LoadState;
            movieTitle = "";
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
            movieTitle = e.NavigationParameter.ToString();

            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
            // TODO: Assign the selected item to this.flipView.SelectedItem
        }

        public string getMoveTitle()
        {
            return movieTitle;
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

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	var buttonIdData = ((Button) e.OriginalSource).DataContext;
            NameStorage.MovieId = ((RottenTomatoes) buttonIdData).MovieId;
        }

        #endregion
    }
}