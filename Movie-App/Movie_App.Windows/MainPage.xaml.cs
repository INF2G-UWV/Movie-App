using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Movie_App.DataModel;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Movie_App
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets the current location using the freegeoip service.
        /// </summary>
        public async void GetLocation()
        {
            try
            {
                var API_CALL = "http://www.telize.com/geoip";
                var wc = new HttpClient();
                var response = await wc.GetStringAsync(API_CALL);

                dynamic rt = JsonConvert.DeserializeObject(response);

                NameStorage.City = (string) rt.city;
            }
            catch
            {
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
            NameStorage.MovieTitle = ((MovieData) buttonData).Title;
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

        private void SearchBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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

        private void SearchBox_QueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
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
            NameStorage.QuerySearch = args.QueryText;
            Frame.Navigate(typeof (BasicPage2), args.QueryText);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetLocation();
            // TODO: Add event handler implementation here.
        }
    }
}