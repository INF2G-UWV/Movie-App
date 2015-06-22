using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Movie_App.DataUnits;
using Movie_App.Util;
using Newtonsoft.Json;

namespace Movie_App
{
    /// <summary>
    ///     Main page of Movie-App.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets the current location in Json format using the telize service.
        /// </summary>
        public async void GetLocation()
        {
            try
            {
                const string API_CALL = "http://www.telize.com/geoip";
                var wc = new HttpClient();
                var response = await wc.GetStringAsync(API_CALL);

                dynamic rt = JsonConvert.DeserializeObject(response);

                //Store cityname in storage
                DataStorage.City = (string) rt.city;
            }
            catch
            {
                ErrorMessage.Show("Communication error!", "Could not Fetch location data!", "Ok");
            }
        }

        /// <summary>
        ///     After button is clicked assign the title to a static variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buttonData = ((Button) e.OriginalSource).DataContext;
            DataStorage.MovieTitle = ((MovieData) buttonData).Title;
        }

        /// <summary>
        ///     If selected index of the listview, assign the same index to the flipview slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void movieListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoviesView.SelectedIndex = movieListview.SelectedIndex;
        }

        /// <summary>
        ///     If selected index of the flipview slider, assign the same index to the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoviesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            movieListview.SelectedIndex = MoviesView.SelectedIndex;
        }

        private void SearchBox_QueryChanged_1(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
        }

        /// <summary>
        ///     When the searchBoxTemp is submitted it will call the the searchmodel that's bind by BasicPage2.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void searchBoxTemp_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            DataStorage.QuerySearch = args.QueryText;
            Frame.Navigate(typeof (SearchPage), args.QueryText);
        }

        /// <summary>
        ///     Executes when page is loaded,
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">parameter</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetLocation();
        }
    }
}